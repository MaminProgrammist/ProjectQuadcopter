using System.Collections;
using UnityEngine;
using General;
using Entities;

namespace Components
{
    public class Flyer : ConfigReceiver<PizzaConfig>
    {
        private Quadcopter _quadcopter;
        private Deliverer _deliveryrer;

        private void Awake()
        {
            _quadcopter = FindObjectOfType<Quadcopter>();
            _deliveryrer = _quadcopter.GetComponent<Deliverer>();
        }

        private void OnEnable() => StartCoroutine(Flight());

        private IEnumerator Flight()
        {
            while (transform.position != _quadcopter.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, _quadcopter.transform.position, 1 / _config.FlightTime);
                yield return null;
            }

            gameObject.SetActive(false);
            _deliveryrer.GrabPizza();
            yield break;
        }
    }
}