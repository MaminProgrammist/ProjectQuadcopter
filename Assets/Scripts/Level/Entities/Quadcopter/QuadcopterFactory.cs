﻿using UnityEngine;
using General;
using Services;
using Ads;
using UI;
using Components;
using Reactions;
using Cinemachine;
using Sound;

namespace Entities
{
    public class QuadcopterFactory : EntityFactory<Quadcopter, QuadcopterConfig>
    {
        private LifeDisplayer _lifeCounter;
        private DefeatPanel _defeatPanel;
        private AdsRewardedButton _rewardedButton;
        private RestartLevelButton _levelRestarter;

        public QuadcopterFactory(QuadcopterConfig config, Container container, LifeDisplayer lifeCounter, DefeatPanel defeatPanel, AdsRewardedButton rewardedButton, RestartLevelButton levelRestarter)
            : base(config, container) 
        {
            _lifeCounter = lifeCounter;
            _defeatPanel = defeatPanel;
            _rewardedButton = rewardedButton;
            _levelRestarter = levelRestarter;
        }

        public override Quadcopter GetCreated()
        {
            Quadcopter quadcopter = Object.Instantiate(_config.Prefab, _container.transform);

            Money.Instance.SetInitialAmount();

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
            deliverer.OnSuccessfulDelivery += () => Money.Instance.Add(_config.SuccessfulDeliveryReward);
            deliverer.OnDeliverySequenceFailed += () => Money.Instance.Spend(_config.FineForFailedDelivery);

            Pizza pizza = quadcopter.GetComponentInChildren<Pizza>();
            pizza.gameObject.SetActive(false);

            ParticleSystem poofParticle = Object.Instantiate(_config.PoofParticle, quadcopter.transform);
            ParticleSystem destroyParticle = Object.Instantiate(_config.DestroyingParticle, quadcopter.transform);

            deliverer.OnDeliverySequenceFailed += () =>
            {
                GameSound.Instance.PlaySound(quadcopter.transform, _config.PoofSound, 1f, 0f, false, false);
                poofParticle.Play();
                pizza.gameObject.SetActive(false);
            };

            deliverer.OnSuccessfulDelivery += () =>
            {
                GameSound.Instance.PlaySound(quadcopter.transform, _config.PoofSound, 1f, 0f, false, false);
                poofParticle.Play();
                pizza.gameObject.SetActive(false);
            };

            deliverer.OnPizzaGrabbed += () =>
            {
                GameSound.Instance.PlaySound(quadcopter.transform, _config.PoofSound, 1f, 0f, false, false);
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
