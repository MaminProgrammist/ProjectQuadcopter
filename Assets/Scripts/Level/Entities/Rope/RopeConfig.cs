using General;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "Config/Rope", fileName = "New Rope Config")]
    public class RopeConfig : Config, ICanMove
    {
        [field: SerializeField] public Rope Prefab { get; internal set; }

        public float SelfSpeed => 0;
    }
}
