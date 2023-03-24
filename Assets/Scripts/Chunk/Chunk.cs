using System.Collections.Generic;
using UnityEngine;
using Level;
using System.Linq;

namespace Chunk
{
    public class Chunk : MonoBehaviour, IActor
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private List<Window> _windows;
        public Vector3 ConnectPoint => new(transform.position.x, transform.position.y, transform.position.z + _meshRenderer.bounds.size.z);

        public IEnumerable<Window> GetWindows() => _windows.AsEnumerable();
    }
}