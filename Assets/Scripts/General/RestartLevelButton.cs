using System;
using Chunk;
using Entities;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class RestartLevelButton : MonoBehaviour
    {
        public event Action LevelRestarted;

        private DefeatPanel _defeatPanel;
        private ChunkGenerator _chunkGenerator;
        private EntitySpawner _entitySpawner;
        private Button _tapToStartButton;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(RestartLevel);
            
            _defeatPanel = FindObjectOfType<DefeatPanel>();
            _entitySpawner = FindObjectOfType<EntitySpawner>();
            _chunkGenerator = FindObjectOfType<ChunkGenerator>();
            _tapToStartButton = FindObjectOfType<TapToStart>().GetComponent<Button>();
        }

        //TODO add fade
        public void RestartLevel()
        {
            GlobalSpeedService.Instance.enabled = false;
            ScoreService.Instance.CheckRecord();
            MoneyService.Instance.SetInitialAmount();
            DistanceService.Instance.ResetDistance();
            _defeatPanel.gameObject.SetActive(false);
            _entitySpawner.ResetEntities();
            _chunkGenerator.ResetChunks();
            _tapToStartButton.enabled = true;
            _tapToStartButton.gameObject.SetActive(true);
            LevelRestarted?.Invoke();
        }
    }
}

