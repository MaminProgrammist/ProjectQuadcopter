using Components;
using Entities;
using Services;
using UnityEngine;

namespace Reactions
{
    public class ResetQuadcopterReaction : Reaction
    {
        private Quadcopter _quadcopter;
        private Lifer _lifer;
        private SkinnedMeshRenderer _renderer;
        private Collider _collider;
        private Animator _animator;

        public ResetQuadcopterReaction(Quadcopter quadcopter,Lifer lifer)
        {
            _quadcopter = quadcopter;
            _collider = quadcopter.GetComponent<Collider>();
            _lifer = lifer;
            _renderer = quadcopter.GetComponentInChildren<SkinnedMeshRenderer>();
            _animator = quadcopter.GetComponentInChildren<Animator>(); 
        }

        public override void React()
        {
            _lifer.Restore();
            _renderer.enabled = true;
            _animator.Play(AnimationService.States.IdleStart);
            _quadcopter.transform.position = Vector3.zero;
            _collider.enabled = true;
        }

    }

}

