using UnityEngine;
using General;
using Entities;

namespace Chunk
{
    [CreateAssetMenu(menuName = "Config/Chunk Config", fileName = "New Chunk Config")]
    public class ChunkConfig : Config, ICanMove
    {
        [field: SerializeField] public Chunk Prefab { get; private set; }
        public float SelfSpeed => 0;
    }
}
