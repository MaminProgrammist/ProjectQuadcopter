using UnityEngine;
using DG.Tweening;
using General;
using Entities;
using Components;

namespace Reactions
{
    public class QuadcopterNextReaction : Reaction
    {
        private WayMatrix _wayMatrix = new();
        private Quadcopter _quadCopter;
        private SwipeController _swipeController;
        private float _takeNextDuration;
        private QuadcopterImmortalReaction _immortalReaction;

        public QuadcopterNextReaction(Quadcopter quadcopter, QuadcopterConfig config)
        {
            _quadCopter = quadcopter;
            _swipeController = quadcopter.GetComponent<SwipeController>();
            _takeNextDuration = config.MotionDuration * 3;
            _immortalReaction = new(quadcopter, config);
            
        }

        public override void React()
        {
            _immortalReaction.React();
            _quadCopter.transform.position = _wayMatrix.GetPosition(MatrixPosition.Center) + Vector3.forward * _wayMatrix.DisappearPoint / 5;
            _swipeController.SetPosition(MatrixPosition.Center);
            DOTween.Kill(_swipeController.transform);
            _quadCopter.transform.DOMove(_wayMatrix.GetPositionByArrayCoordinates(_swipeController.CurrentPosition), _takeNextDuration);
        }
    }
}
