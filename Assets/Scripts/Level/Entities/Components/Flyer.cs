using DG.Tweening;
using General;
using Entities;
using UnityEngine;

namespace Components
{
    public class Flyer : ConfigReceiver<PizzaConfig>
    {
        private Tweener _flightTweener;
        private Quadcopter _quadcopter;
        private Deliverer _deliveryrer;
        private SwipeController _swipeController;

        private void Awake()
        {
            _quadcopter = FindObjectOfType<Quadcopter>();
            _deliveryrer = _quadcopter.GetComponent<Deliverer>();
            _swipeController = _quadcopter.GetComponent<SwipeController>();
        }

        private void OnEnable()
        {
            _flightTweener = transform
                .DOMove(_quadcopter.transform.position, _config.FlightTime)
                .SetEase(Ease.Linear)
                .SetAutoKill(false)
                .OnComplete(() => 
                {
                    gameObject.SetActive(false);
                    _deliveryrer.GrabPizza();
                });

            _swipeController.OnMove += SetTarget;
        }

        private void SetTarget(Vector3 quadcopterPosition) => _flightTweener?.ChangeEndValue(quadcopterPosition, true).Restart();

        private void OnDisable()
        {
            _flightTweener.Kill();
            _swipeController.OnMove -= SetTarget;
        }
    }
}