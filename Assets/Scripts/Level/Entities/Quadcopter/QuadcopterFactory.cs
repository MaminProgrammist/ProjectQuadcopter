using UnityEngine;
using General;
using Services;
using Ads;
using UI;
using Components;
using Reactions;
using Cinemachine;

namespace Entities
{
    public class QuadcopterFactory : EntityFactory<Quadcopter, QuadcopterConfig>
    {
        private DefeatPanel _defeatPanel;
        private AdsRewardedButton _rewardedButton;
        private RestartLevelButton _levelRestarter;

        public QuadcopterFactory(QuadcopterConfig config, Container container, DefeatPanel defeatPanel, AdsRewardedButton rewardedButton, RestartLevelButton levelRestarter)
            : base(config, container) 
        {
            _defeatPanel = defeatPanel;
            _rewardedButton = rewardedButton;
            _levelRestarter = levelRestarter;
        }

        public override Quadcopter GetCreated()
        {
            Quadcopter quadcopter = Object.Instantiate(_config.Prefab, _container.transform);

            Money.Instance.SetInitialAmount();

            AudioSource audioSource = quadcopter.GetComponent<AudioSource>();

            SwipeController swipeController = quadcopter.gameObject.AddComponent<SwipeController>();
            swipeController.Receive(_config);
            swipeController.enabled = false;

            Lifer lifer = quadcopter.gameObject.AddComponent<Lifer>();
            lifer.Receive(_config);
            lifer.Restore();
            lifer.OnDeath += () => _defeatPanel.gameObject.SetActive(true);

            Deliverer deliverer = quadcopter.gameObject.AddComponent<Deliverer>();
            deliverer.Receive(_config);
            deliverer.OnSuccessfulDelivery += () => Money.Instance.Add(_config.SuccessfulDeliveryReward);
            deliverer.OnDeliverySequenceFailed += () => Money.Instance.Spend(_config.FineForFailedDelivery);

            Pizza pizza = quadcopter.GetComponentInChildren<Pizza>();
            pizza.gameObject.SetActive(false);

            ParticleSystem poofParticle = Object.Instantiate(_config.PoofParticle, quadcopter.transform);
            ParticleSystem destroyParticle = Object.Instantiate(_config.DestroyingParticle, quadcopter.transform);

            deliverer.OnDeliverySequenceFailed += () =>
            {
                audioSource.clip = _config.PoofSound;
                audioSource.Play();
                poofParticle.Play();
                pizza.gameObject.SetActive(false);
            };

            deliverer.OnSuccessfulDelivery += () =>
            {
                audioSource.clip = _config.PoofSound;
                audioSource.Play();
                poofParticle.Play();
                pizza.gameObject.SetActive(false);
            };

            deliverer.OnPizzaGrabbed += () =>
            {
                audioSource.clip = _config.PoofSound;
                audioSource.Play();
                poofParticle.Play();
                pizza.gameObject.SetActive(true);
            };

            quadcopter.AddReaction<CollisionDetector, Bird, Car, Weapon, Rope>(new TakeDamageReaction(quadcopter, _config, destroyParticle));

            quadcopter.GetComponentInChildren<CinemachineVirtualCamera>().transform.SetParent(_container.transform);

            _rewardedButton.OnShowCompleted += () =>
            {
                lifer.Restore();
                new QuadcopterNextReaction(quadcopter, _config).React();
            };

            ResetQuadcopterReaction resetQuadcopter = new(quadcopter, lifer);
            _levelRestarter.LevelRestarted += () => resetQuadcopter.React();

            return quadcopter;
        }
    }
}
