using System;
using System.Collections.Generic;
using UnityEngine;
using General;
using Entities;
using System.Linq;

namespace Chunk
{
    public class ChunkGenerator : MonoBehaviour
    {
        public event Action<IEnumerable<Window>> OnChunkSpawned;

        [SerializeField] private ChunkConfig _config;
        [Space(30)]
        [SerializeField, Range(1, 100)] private int _initialAmount;
        [SerializeField] private float _verticalOffset = 3.5f;
        private WayMatrix _wayMatrix = new();
        private Pool<Chunk> _pool;
        private Chunk _last;
        private List<Window> _allWindows = new();
        private EntitySpawner _entitySpawner;

        private void Awake()
        {
            _entitySpawner = FindObjectOfType<EntitySpawner>();
        }

        public bool EnableChunks(Container container) 
        {
            _pool = new(new ChunkFactory(_config, SpawnChunk), container, _initialAmount);
            SpawnInitial();
            _entitySpawner.RequestPizza();
            return true;
        }

        public void ResetChunks()
        {
            RemoveAll();
            SpawnInitial();
        }

        private void RemoveAll()
        {
            _pool.ReleaseAll();
            _last = null;
        }

        private void SpawnInitial()
        {
            for (int i = 0; i != _initialAmount; i++)
                SpawnChunk();

            _entitySpawner.RequestPizza();
        }

        private void SpawnChunk()
        {
            Vector3 spawnPoint = _last ? _last.ConnectPoint : _wayMatrix.GetPosition(MatrixPosition.Down) + Vector3.up * _verticalOffset;
            _allWindows?.Clear();
            _last = _pool.Get(spawnPoint);
            _allWindows.AddRange(_last.GetWindows());
            OnChunkSpawned?.Invoke(_allWindows.AsEnumerable());
        }
    }
}