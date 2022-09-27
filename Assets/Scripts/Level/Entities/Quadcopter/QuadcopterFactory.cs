using UnityEngine;
using General;
using Services;
using Ads;
using UI;
using Components;
using Reactions;

namespace Entities
{
    public class QuadcopterFactory : EntityFactory<Quadcopter, QuadcopterConfig>
    {
        private LifeDisplayer _lifeCounter;
        private DefeatPanel _defeatPanel;
        private AdsRewardedButton _rewardedButton;

        public QuadcopterFactory(QuadcopterConfig config, Container container, LifeDisplayer lifeCounter, DefeatPanel defeatPanel, AdsRewardedButton rewardedButton)
            : base(config, container) 
        {
            _lifeCounter = lifeCounter;
            _defeatPanel = defeatPanel;
            _rewardedButton = rewardedButton;
        }

        public override Quadcopter GetCreated()
        {
            Quadcopter quadcopter = Object.Instantiate(_config.Prefab, _container.transform);

            MoneyService.SetInitialAmount();

            AudioSource audioSource = quadcopter.GetComponent<AudioSource>();

            SwipeController swipeController = quadcopter.gameObject.AddComponent<SwipeController>();
            swipeController.Receive(_config);
            swipeController.enabled = false;

            Lifer lifer = quadcopter.gameObject.AddComponent<Lifer>();
            lifer.OnChanged += _lifeCounter.Display;
            lifer.Receive(_config);
            lifer.Restore();
            lifer.OnDeath += () => _defeatPanel.gameObject.SetActive(true);

            Deliverer deliverer = quadcopter.gameObject.AddComponent<Deliverer>();
            deliverer.Receive(_config);
            deliverer.OnSuccessfulDelivery += () => MoneyService.AddMoney(_config.SuccessfulDeliveryReward);
            deliverer.OnDeliverySequenceFailed += () => MoneyService.SubtractMoney(_config.FineForFailedDelivery);

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

            quadcopter.AddReaction<CollisionDetector, Bird, Car, Weapon>(new TakeDamageReaction(quadcopter, _config, destroyParticle));

            GlobalSpeedService.OnStartup += () =>
            {
                swipeController.enabled = true;
                new QuadcopterStartReaction(quadcopter).React();
            };
            

            quadcopter.GetComponentInChildren<Camera>().transform.SetParent(_container.transform);

            _rewardedButton.OnShowCompleted += () =>
            {
                lifer.Restore();
                new QuadcopterNextReaction(quadcopter, _config).React();
            };

            return quadcopter;
        }
    }
}
