using UnityEngine;
using Level;

namespace Chunk
{
    public class PieceOfChunk : MonoBehaviour, IActor
    {
        public Vector3 Size { get; private set; }

        protected virtual void Awake() => Size = GetComponent<MeshRenderer>().bounds.size;
    }
}