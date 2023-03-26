using Components;
using Reactions;
using UnityEngine;

namespace Entities
{
    public class RopeFactory : EntityFactory<Rope, RopeConfig>
    {
        public RopeFactory(RopeConfig config) : base(config)
        {
        }

        public override Rope GetCreated()
        {
            Rope rope = Object.Instantiate(_config.Prefab);
            rope.gameObject.AddComponent<Mover>().Receive(_config);
            rope.gameObject.AddComponent<Disappearer>();
            rope.AddReaction<CollisionDetector>(new RopeCutReaction());
            return rope;
        }
    }
}
