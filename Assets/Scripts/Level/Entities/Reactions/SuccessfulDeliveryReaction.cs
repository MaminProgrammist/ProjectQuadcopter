using UnityEngine;
using Entities;
using Components;

namespace Reactions
{
    public class SuccessfulDeliveryReaction : Reaction
    {
        private Client _client;
        private Deliverer _deliverer;

        public SuccessfulDeliveryReaction(Client client, Deliverer deliverer) {
            _client = client;
            _deliverer = deliverer;
        }

        public override void React()
        {
            _client.gameObject.SetActive(false);
            _deliverer.DropPizza(true);
        }
    }

}

