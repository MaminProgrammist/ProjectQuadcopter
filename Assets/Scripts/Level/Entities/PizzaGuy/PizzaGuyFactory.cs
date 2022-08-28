using UnityEngine;
using Components;
using Reactions;

namespace Entities
{
    [System.Obsolete]
    public class PizzaGuyFactory : EntityFactory<PizzaGuy, PizzaGuyConfig>
    {
        private Deliverer _deliverer;
        private Pizza _flyingPizza;

        public PizzaGuyFactory(PizzaGuyConfig config, Deliverer deliverer, Pizza pizza, Quadcopter quadcopter) : base(config)
        {
            _deliverer = deliverer;
            _flyingPizza = pizza;
        }

        public override PizzaGuy GetCreated()
        {
            PizzaGuy pizzaGuy = Object.Instantiate(_config.Prefab);

            PizzaPoint pizzaPoint = pizzaGuy.gameObject.GetComponentInChildren<PizzaPoint>();

            pizzaGuy.gameObject.SetActive(false);
            pizzaGuy.gameObject.AddComponent<Disappearer>().OnDisappear += () => _deliverer.DropPizza();

            _deliverer.OnPizzaGrabbed += () => pizzaGuy.gameObject.SetActive(false);
            _deliverer.OnDeliverySequenceFailed += () => pizzaGuy.gameObject.SetActive(false);

            //BoxDetector boxDetector = pizzaGuy
            //    .AddReaction<BoxDetector, Quadcopter>(new PizzaThrowingReaction(_deliverer, _flyingPizza, pizzaPoint, pizzaGuy));

            //boxDetector.Receive(_config);

            pizzaGuy.gameObject
                .AddComponent<Mover>()
                .Receive(_config);

            return pizzaGuy;
        }
    }
}
 