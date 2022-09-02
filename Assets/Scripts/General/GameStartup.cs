using UnityEngine;
using Services;
using UI;
using Chunk;
using Level;
using Entities;

namespace General
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private City _city;

        private DefeatPanel _defeatPanel;
        private ChunkGenerator _chunkGenerator;
        private EntitySpawner _entitySpawner;

        private void Awake()
        {
            _defeatPanel = FindObjectOfType<DefeatPanel>();
            _entitySpawner = GetComponentInChildren<EntitySpawner>();
            _chunkGenerator = GetComponentInChildren<ChunkGenerator>();
        }

        private void Start()
        {
            Container chunkContainer = ContainerService.GetCreatedContainer("Chunks", _city.transform);
            Container entityContainer = ContainerService.GetCreatedContainer("Entities", _city.transform);
            _chunkGenerator.EnableChunks(chunkContainer);
            _entitySpawner.EnableQuadcopter(entityContainer, _defeatPanel);
            _entitySpawner.EnableCarTraffic(entityContainer);
            _entitySpawner.EnableBirds(entityContainer);
            _entitySpawner.EnableNetGuys(entityContainer);
            //_entitySpawner.EnableBatteries(entityContainer);
            _entitySpawner.EnableDelivery(entityContainer, _chunkGenerator);
            GlobalSpeedService.Instance.enabled = false;
            _defeatPanel.gameObject.SetActive(false);
        }
    }
}