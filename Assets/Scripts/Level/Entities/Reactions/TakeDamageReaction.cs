﻿using System.Collections;
using UnityEngine;
using Services;
using Entities;
using Components;
using General;
using Sound;

namespace Reactions
{
    public class TakeDamageReaction : Reaction
    {
        private QuadcopterConfig _config;
        private Lifer _lifer;
        private SkinnedMeshRenderer _renderer;
        private Collider _collider;
        private SwipeController _swipeController;
        private QuadcopterNextReaction _nextReaction;
        private PizzaFallenReaction _pizzaFallenReaction;
        private ParticleSystem _destroyingParticle;

        public TakeDamageReaction(Quadcopter quadcopter, QuadcopterConfig config, ParticleSystem destroyingParticle)
        {
            _config = config;
            _lifer = quadcopter.GetComponent<Lifer>();
            _renderer = quadcopter.GetComponentInChildren<SkinnedMeshRenderer>();
            _collider = quadcopter.GetComponent<Collider>();
            _swipeController = quadcopter.GetComponentInChildren<SwipeController>();
            _nextReaction = new QuadcopterNextReaction(quadcopter, config);
            _pizzaFallenReaction = new(quadcopter.GetComponent<Deliverer>());
            _destroyingParticle = destroyingParticle;
        }

        public override void React()
        {
            GlobalSpeed.Instance.enabled = false;
            _swipeController.enabled = false;
            _lifer.TakeDamage();

            if (_lifer.IsDdeath == false)
                _lifer.StartCoroutine(Focus());

            _renderer.enabled = false;
            _collider.enabled = false;
            _pizzaFallenReaction.React();
        }

        private IEnumerator Focus()
        {
            if (_detectableEntity is Weapon == false)
            {
                GameSound.Instance.PlaySound(_lifer.transform, _config.DestroyingSound, 1f, 0f, false, false);
                _destroyingParticle.Play();
            }

            yield return new WaitForSeconds(1);
            _nextReaction.React();
            GlobalSpeed.Instance.enabled = true;
            yield break;

        }
    }
}
