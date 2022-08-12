﻿using System.Collections;
using UnityEngine;
using Services;
using Entities;

namespace Reactions
{
    public class ShoveOutReaction : Reaction
    {
        private NetGuy _netGuy;
        private NetGuyConfig _config;
        private Animator _animator;
        private float _shoveOutSpeed;

        public ShoveOutReaction(NetGuy netGuy, NetGuyConfig config)
        {
            _netGuy = netGuy;
            _config = config;
            _animator = netGuy.GetComponentInChildren<Animator>();
        }

        private void CalculateShoveOuteTime()
        {
            float speed = GlobalSpeedService.Speed;
            float acceleretion = GlobalSpeedService.Acceleration;
            float time;

            time = (Mathf.Sqrt(speed * speed + 2 * acceleretion * _config.ZDetectionDistanceBackward * 0.5f) - speed) / acceleretion;
            _shoveOutSpeed = 1f / time;
        }

        private IEnumerator ShoveOut()
        {
            float currentSide = 0;
            float targetSide = -Mathf.Clamp(_netGuy.transform.position.x, -1, 1);

            CalculateShoveOuteTime();

            while (Mathf.Approximately(currentSide, targetSide) == false)
            {
                _animator.SetFloat(AnimationService.Parameters.Side, currentSide);
                currentSide = Mathf.MoveTowards(currentSide, targetSide, _shoveOutSpeed * Time.deltaTime);
                yield return null;
            }

            yield break;
        }

        public override void React() => _netGuy.StartCoroutine(ShoveOut());
    }
}
