using UnityEngine;
using Services;
using UI;
using Chunk;
using Level;
using Entities;
using System.Collections;
using UnityEngine.UI;

namespace General
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private City _city;

        private DefeatPanel _defeatPanel;
        private ChunkGenerator _chunkGenerator;
        private EntitySpawner _entitySpawner;
        private Button _tapToStartButton;

        private void Awake()
        {
            _defeatPanel = FindObjectOfType<DefeatPanel>();
            _entitySpawner = GetComponentInChildren<EntitySpawner>();
            _chunkGenerator = GetComponentInChildren<ChunkGenerator>();
            _tapToStartButton = FindObjectOfType<TapToStart>().GetComponent<Button>();
        }

        private void Start()
        {
            Container chunkContainer = ContainerService.Instance.GetCreatedContainer("Chunks", _city.transform);
            Container entityContainer = ContainerService.Instance.GetCreatedContainer("Entities", _city.transform);
            StartCoroutine(GameInitialization(entityContainer, chunkContainer));
        }

        private IEnumerator GameInitialization(Container entityContainer, Container chunkContainer)
        {
            _tapToStartButton.enabled = false;
            yield return new WaitUntil(() => _chunkGenerator.EnableChunks(chunkContainer));
            yield return new WaitUntil(() => _entitySpawner.EnableQuadcopter(entityContainer, _defeatPanel, out Quadcopter quadcopter));
            yield return new WaitUntil(() => _entitySpawner.EnableCarTraffic(entityContainer));
            yield return new WaitUntil(() => _entitySpawner.EnableBirds(entityContainer));
            yield return new WaitUntil(() => _entitySpawner.EnableNetGuys(entityContainer));
            //yield return new WaitUntil(() => _entitySpawner.EnableBatteries(entityContainer));
            yield return new WaitUntil(() => _entitySpawner.EnableDelivery(entityContainer, _chunkGenerator));
            GlobalSpeedService.Instance.enabled = false;
            _defeatPanel.gameObject.SetActive(false);
            _tapToStartButton.enabled = true;
        }
    }
}