using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using General;
using Components;

namespace Chunk
{
    public class ChunkGenerator : MonoBehaviour
    {
        public event Action<IEnumerable<Window>> OnSpawnChunk;
        public event Action<PizzaDispensePoint> OnPizzeriaSpawned;

        [SerializeField] private ChunkConfig _chunkDatabase;
        [Space(30)]
        [SerializeField][Range(1, 100)] private int _startableChunksCount;

        private bool _isPizzeriaRequested;
        private WayMatrix _wayMatrix = new();
        private Pool<Road> _roadPool;
        private Pool<District> _districtPool;
        private Pool<PizzeriaDistrict> _pizzeriaPool;
        private Road _lastRoad;
        private List<Window> _windows = new();
        private Container _chunksContainer;

        public bool EnableChunks(Container chunksContainer) 
        {
            _roadPool = new(new RoadFactory(_chunkDatabase, SpawnChunk), chunksContainer, _startableChunksCount);
            _districtPool = new(new DistrictFactory(_chunkDatabase), chunksContainer, _chunkDatabase.DistrictsPrefabsCount);
            _pizzeriaPool = new(new DistrictWithPizzeriaFactory(_chunkDatabase), chunksContainer, _chunkDatabase.DistrictsWithPizzeeriaPrefabsCount);
            SpawnStartableChunks(chunksContainer, _startableChunksCount);
            _chunksContainer = chunksContainer;
            _isPizzeriaRequested = true;
            return true;
        }

        public void ResetChunks()
        {
            RemoveAllChunks();
            SpawnStartableChunks(_chunksContainer, _startableChunksCount);
        }

        private void RemoveAllChunks()
        {
            PieceOfChunk[] chunks = FindObjectsOfType<PieceOfChunk>();
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i].gameObject.SetActive(false);
            }
        }

        public void RequestPizzeria() => _isPizzeriaRequested = true;

        private void SpawnStartableChunks(Container chunkContainer, int chunksCount)
        {
            float offset = 3.5f;
            _lastRoad = Instantiate(_chunkDatabase.StartableChunk,
            _wayMatrix.GetPosition(MatrixPosition.Down) + Vector3.down * offset,
            _chunkDatabase.StartableChunk.transform.rotation, chunkContainer.transform);
            _lastRoad.gameObject.AddComponent<Mover>().Receive(_chunkDatabase);
            _lastRoad.gameObject.AddComponent<Disappearer>();

            for (int i = 0; i < chunksCount; i++)
                SpawnChunk();

            _isPizzeriaRequested = true;
        }

        private void SpawnChunk()
        {
            _windows.Clear();
            _lastRoad = _roadPool.Get(_lastRoad.CentralConnectPosition);
            int side1 = Random.Range(0, 2) == 0 ? -1 : 1;
            int side2 = side1 * -1;
            GetPieceOfChunk(side1);
            GetPieceOfChunk(side2);
            OnSpawnChunk?.Invoke(_windows);
        }

        private PieceOfChunk GetPieceOfChunk(float side)
        {
            Vector3 position = side == 1 ? _lastRoad.RightConnectPosition : _lastRoad.LeftConnectPosition;
            PieceOfChunk pieceOfChunk;
            PizzeriaDistrict pizzeria = null;
            if (_isPizzeriaRequested)
            {
                pizzeria = _pizzeriaPool.Get(position);
                pieceOfChunk = pizzeria;
                _isPizzeriaRequested = false;
            }

            else
            {
                District district;
                district = _districtPool.Get(position) ;
                _windows.AddRange(district.GetWindows());
                pieceOfChunk = district;
            }

            pieceOfChunk.transform.position += new Vector3(side, 0) * pieceOfChunk.Size.z / 2;
            float rotation = side == 1 ? -90 : 90;
            pieceOfChunk.transform.localEulerAngles = new Vector3(0, rotation, 0);
            if(pizzeria != null) OnPizzeriaSpawned?.Invoke(pizzeria.PizzaDispensePoint);
            return pieceOfChunk;
        }

    }
}