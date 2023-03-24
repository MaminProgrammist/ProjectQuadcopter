using UnityEngine;
using General;
using Services;
using Entities;

namespace Components
{
    class Rotator : ConfigReceiver<BatteryConfig>
    {
        private readonly float _maxSpeedFactor = 100;
        private Vector3 _speedFactor;


        private void OnEnable()
        {
            SetSpeedFactor();
            Updater.Instance.OnUpdate += Rotate;
        }

        private void SetSpeedFactor() => _speedFactor = new Vector3(Random.Range(0, _maxSpeedFactor), Random.Range(0, _maxSpeedFactor), Random.Range(0, _maxSpeedFactor));

        private void Rotate() => gameObject.transform.Rotate(Time.deltaTime * _config.RotationSpeed * _speedFactor);

        private void OnDisable() => Updater.Instance.OnUpdate -= Rotate;
    }
}
