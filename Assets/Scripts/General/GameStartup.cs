using UnityEngine;
using Services;
using UI;
using Chunk;
using Level;
using Entities;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Reactions;

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

        private async void Start()
        {
            Container chunkContainer = ContainerFactory.Instance.GetCreated("Chunks", _city.transform);
            Container entityContainer = ContainerFactory.Instance.GetCreated("Entities", _city.transform);
            GlobalSpeed.Instance.enabled = false;
            _chunkGenerator.EnableChunks(chunkContainer);
            _entitySpawner.EnableQuadcopter(entityContainer, _defeatPanel, out Quadcopter quadcopter);
            _entitySpawner.EnableCarTraffic(entityContainer);
            _entitySpawner.EnableBirds(entityContainer);
            //_entitySpawner.EnableWindowGuys(entityContainer);
            //_entitySpawner.EnableBatteries(entityContainer);
            //_entitySpawner.EnableDelivery(entityContainer, _chunkGenerator);
            await LoadingScreen.Instance.Hide();
            _defeatPanel.gameObject.SetActive(false);
            new QuadcopterStartReaction(quadcopter).React();
        }
    }
}