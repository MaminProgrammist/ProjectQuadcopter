using System.Collections;
using UnityEngine;
using Services;
using Entities;
using Components;

namespace Reactions
{
    public class TakeDamageReaction : Reaction
    {
        private Lifer _lifer;
        private SkinnedMeshRenderer _renderer;
        private Collider _collider;
        private SwipeController _swipeController;
        private QuadcopterNextReaction _nextReaction;
        private PizzaFallenReaction _pizzaFallenReaction;

        public TakeDamageReaction(Quadcopter quadcopter, QuadcopterConfig config)
        {
            _lifer = quadcopter.GetComponent<Lifer>();
            _renderer = quadcopter.GetComponentInChildren<SkinnedMeshRenderer>();
            _collider = quadcopter.GetComponent<Collider>();
            _swipeController = quadcopter.GetComponentInChildren<SwipeController>();
            _nextReaction = new QuadcopterNextReaction(quadcopter, config);
            _pizzaFallenReaction = new(quadcopter.GetComponent<Deliverer>());
        }

        public override void React()
        {
            GlobalSpeedService.Instance.enabled = false;
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
            //Фокусируемся на месте смерти.
            yield return new WaitForSeconds(1);
            _nextReaction.React();
            GlobalSpeedService.Instance.enabled = true;
            yield break;
        }
    }
}
