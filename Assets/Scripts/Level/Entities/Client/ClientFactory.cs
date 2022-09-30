using UnityEngine;
using Components;
using Reactions;

namespace Entities
{
    public class ClientFactory : EntityFactory<Client, ClientConfig>
    {
        private Deliverer _deliverer;

        public ClientFactory(ClientConfig config, Deliverer deliverer) : base(config)
        {
            _deliverer = deliverer;
        }

        public override Client GetCreated()
        {
            Client client = Object.Instantiate(_config.Prefab);

            client.gameObject.AddComponent<Disappearer>().OnDisappear += () => _deliverer.DropPizza();

            client.AddReaction<BoxDetector, Quadcopter>(new SuccessfulDeliveryReaction(client, _deliverer)).Receive(_config);

            client.gameObject
                .AddComponent<Mover>()
                .Receive(_config);

            //client.gameObject.AddComponent<Markerer>().SetImage(Object.FindObjectOfType<ClientMarker>().GetComponent<Image>());

            _deliverer.OnDeliverySequenceFailed += () => client.gameObject.SetActive(false);
            return client;
        }
    }
}
