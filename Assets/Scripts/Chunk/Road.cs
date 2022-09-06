using UnityEngine;

namespace Chunk
{
    public class Road : PieceOfChunk 
    {
        public Vector3 LeftConnectPosition => new(transform.position.x - Size.x / 2, transform.position.y, transform.position.z);
        public Vector3 CentralConnectPosition => new(transform.position.x, transform.position.y, transform.position.z + Size.z);
        public Vector3 RightConnectPosition => new(transform.position.x + Size.x / 2, transform.position.y, transform.position.z);
    }
}