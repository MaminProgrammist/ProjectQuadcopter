using Components;
using Entities;
using General;
using Services;
using UnityEngine;

namespace Reactions
{
    public class ResetQuadcopterReaction : Reaction
    {
        private Lifer _lifer;
        private SkinnedMeshRenderer _renderer;
        private Collider _collider;
        private Animator _animator;
        private SwipeController _swipeController;

        public ResetQuadcopterReaction(Quadcopter quadcopter, Lifer lifer)
        {
            _collider = quadcopter.GetComponent<Collider>();
            _lifer = lifer;
            _renderer = quadcopter.GetComponentInChildren<SkinnedMeshRenderer>();
            _animator = quadcopter.GetComponentInChildren<Animator>(); 
            _swipeController = quadcopter.GetComponent<SwipeController>();
        }

        public override void React()
        {
            _lifer.Restore();
            _renderer.enabled = true;
            _swipeController.SetPosition(MatrixPosition.Center);
            _animator.SetTrigger(AnimationService.States.Start);
            _collider.enabled = true;
            GlobalSpeed.Instance.enabled = true;
            _swipeController.enabled = true;
        }

    }

}

