using UnityEngine;
using General;
using Services;
using Entities;

namespace Components
{
    public sealed class Mover : ConfigReceiver<ICanMove>
    {
        private float _pushingSpeed;

        public float SelfSpeed => _config.SelfSpeed;

        private void OnEnable()
        {
            _pushingSpeed = 0;
            UpdateService.Instance.OnFixedUpdate += Move;
        }

        private void Move()
        {
            if (GlobalSpeedService.Instance.Speed > 0)
                transform.position += (GlobalSpeedService.Instance.Speed + SelfSpeed + _pushingSpeed) * Time.fixedDeltaTime * Vector3.back;
        }

        public void Push(float pusherSpeed) => _pushingSpeed = pusherSpeed - SelfSpeed;

        private void OnDisable() => UpdateService.Instance.OnFixedUpdate -= Move;
    }
}