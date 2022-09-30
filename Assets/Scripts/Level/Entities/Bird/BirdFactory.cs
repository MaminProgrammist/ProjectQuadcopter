using UnityEngine;
using Components;
using Reactions;

namespace Entities
{
    class BirdFactory : EntityFactory<Bird, BirdConfig>
    {
        public BirdFactory(BirdConfig config) : base(config) { }

        public override Bird GetCreated()
        {
            Bird bird = Object.Instantiate(_config.Prefab);

            bird.gameObject.AddComponent<Mover>().Receive(_config);
            bird.gameObject.AddComponent<Disappearer>();

            bird.AddReaction<CollisionDetector, Quadcopter, Car>(new BirdKillingReaction(bird)).Receive(_config);
            bird.AddReaction<CollisionDetector, Guy>(new FreezingReaction()).Receive(_config);
            bird.AddReaction<FrontDetector, Quadcopter>(new BirdFearOfCollisionReaction(bird)).Receive(_config);
            bird.GetComponentInChildren<Animator>().keepAnimatorControllerStateOnDisable = true;

            return bird;
        }
    }
}
