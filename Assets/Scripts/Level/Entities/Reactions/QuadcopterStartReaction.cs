﻿using System.Collections;
using UnityEngine;
using General;
using Services;
using Entities;
using Components;

namespace Reactions
{
    public class QuadcopterStartReaction : Reaction
    {
        private Animator _animator;
        private SwipeController _swipeController;

        public QuadcopterStartReaction(Quadcopter quadcopter)
        {
            _animator = quadcopter.GetComponentInChildren<Animator>();
            _swipeController = quadcopter.GetComponent<SwipeController>();
        }

        public override void React()
        {
            _swipeController.SetPosition(MatrixPosition.Center);
            _animator.SetTrigger(AnimationService.States.Start);
        }
    }
}
