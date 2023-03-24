using UnityEngine;

namespace General
{
    public interface IPool<T> 
    {
        bool IsInitialized { get; }
        T Get(Vector3 spawnPosition);
        void ReleaseAll();
    }
}
