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
            Updater.Instance.OnFixedUpdate += Move;
        }

        private void Move()
        {
            if (GlobalSpeed.Instance.Value > 0)
                transform.position += (GlobalSpeed.Instance.Value + SelfSpeed + _pushingSpeed) * Time.fixedDeltaTime * Vector3.back;
        }

        public void Push(float pusherSpeed) => _pushingSpeed = pusherSpeed - SelfSpeed;

        private void OnDisable() => Updater.Instance.OnFixedUpdate -= Move;
    }
}