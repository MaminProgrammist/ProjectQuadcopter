using UnityEngine;
using System.Collections;
using Entities;
using Components;

namespace Reactions
{
    public class PizzaThrowingReaction : Reaction
    {
        private readonly Deliverer _deliverer;
        private readonly Pizza _flyingPizza;
        private readonly PizzaEjector _pizzaEjector;

        public PizzaThrowingReaction(Deliverer deliverer, Pizza pizza, PizzaEjector pizzaEjector)
        {
            _flyingPizza = pizza;
            _deliverer = deliverer;
            _pizzaEjector = pizzaEjector;
        }

        public override void React() => _pizzaEjector.StartCoroutine(Throwing());

        private IEnumerator Throwing()
        {
            _flyingPizza.transform.position = _pizzaEjector.transform.position;
            yield return new WaitForEndOfFrame();
            _flyingPizza.gameObject.SetActive(true);
            _deliverer.ThrowPizza();
        }
        
    }
}

