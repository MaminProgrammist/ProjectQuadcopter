using UnityEngine;
using Components;
using Reactions;

namespace Entities
{
    public class PizzaEjectorFactory : EntityFactory<PizzaEjector, PizzaEjectorConfig>
    {
        private Deliverer _deliverer;
        private Pizza _flyingPizza;

        public PizzaEjectorFactory(PizzaEjectorConfig config, Deliverer deliverer, Pizza pizza) : base(config)
        {
            _deliverer = deliverer;
            _flyingPizza = pizza;
        }

        public override PizzaEjector GetCreated()
        {
            PizzaEjector pizzaEjector = Object.Instantiate(_config.Prefab);

            PizzaPoint pizzaPoint = pizzaEjector.gameObject.GetComponentInChildren<PizzaPoint>();

            pizzaEjector.gameObject.SetActive(false);
            pizzaEjector.gameObject.AddComponent<Disappearer>().OnDisappear += () => _deliverer.DropPizza();

            _deliverer.OnPizzaGrabbed += () => pizzaEjector.gameObject.SetActive(false);
            _deliverer.OnDeliverySequenceFailed += () => pizzaEjector.gameObject.SetActive(false);

            BoxDetector boxDetector = pizzaEjector
                .AddReaction<BoxDetector, Quadcopter>(new PizzaThrowingReaction(_deliverer, _flyingPizza, pizzaPoint, pizzaEjector));

            boxDetector.Receive(_config);

            pizzaEjector.gameObject
                .AddComponent<Mover>()
                .Receive(_config);

            return pizzaEjector;
        }
    }
}
