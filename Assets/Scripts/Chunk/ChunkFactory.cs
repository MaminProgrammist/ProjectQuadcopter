using Components;
using Level;
using System;

namespace Chunk
{
    public class ChunkFactory : ActorFactory<Chunk>
    {
        private ChunkConfig _config;
        private Action _spawnChunk;

        public ChunkFactory(ChunkConfig chunkDatabase, Action spawnChunk)
        {
            _config = chunkDatabase;
            _spawnChunk = spawnChunk;
        }

        public override Chunk GetCreated()
        {
            Chunk chunk = UnityEngine.Object.Instantiate(_config.Prefab);
            chunk.gameObject.AddComponent<Mover>().Receive(_config);
            chunk.gameObject.AddComponent<Disappearer>().OnDisappear += _spawnChunk;
            return chunk;
        }
    }
}