using UnityEngine;
using General;
using Services;
using Ads;
using UI;
using Components;
using Reactions;
using DG.Tweening;

namespace Entities
{
    public class QuadcopterFactory : EntityFactory<Quadcopter, QuadcopterConfig>
    {
        private LifeDisplayer _lifeCounter;
        private DefeatPanel _defeatPanel;
        private AdsRewardedButton _rewardedButton;
        private ParticleSystem _poofParticle;

        public QuadcopterFactory(QuadcopterConfig config, Container container, LifeDisplayer lifeCounter, DefeatPanel defeatPanel, AdsRewardedButton rewardedButton)
            : base(config, container) 
        {
            _lifeCounter = lifeCounter;
            _defeatPanel = defeatPanel;
            _rewardedButton = rewardedButton;
            _poofParticle = Resources.Load<ParticleSystem>("Art/Epic Toon FX/Prefabs/Environment/Smoke/White/SmokeExplosionWhite");
        }

        public override Quadcopter GetCreated()
        {
            Quadcopter quadcopter = Object.Instantiate(_config.Prefab, _container.transform);
            TakeDamageReaction takeDamageReaction = new(quadcopter, _config);

            MoneyService.SetInitialAmount();

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

            deliverer.OnDeliverySequenceFailed += () =>
            {
                Object.Instantiate(_poofParticle, quadcopter.transform);
                pizza.gameObject.SetActive(false);
            };

            deliverer.OnSuccessfulDelivery += () =>
            {
                Object.Instantiate(_poofParticle, quadcopter.transform);
                pizza.gameObject.SetActive(false);
            };

            deliverer.OnPizzaGrabbed += () =>
            {
                Object.Instantiate(_poofParticle, quadcopter.transform);
                pizza.gameObject.SetActive(true);
            };

            quadcopter.AddReaction<CollisionDetector, Bird, Car, Net>(new TakeDamageReaction(quadcopter, _config));
            quadcopter.AddReaction<CollisionDetector, Pizza>(new GrabPizzaReaction(pizza, deliverer));

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
