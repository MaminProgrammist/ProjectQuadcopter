using System;
using Chunk;
using Cinemachine;
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
        private CinemachineBrain _cameraBrain;

        private void Awake()
        {
            _cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
            GetComponent<Button>().onClick.AddListener(RestartLevel);
            _defeatPanel = FindObjectOfType<DefeatPanel>();
            _entitySpawner = FindObjectOfType<EntitySpawner>();
            _chunkGenerator = FindObjectOfType<ChunkGenerator>();
            _tapToStartButton = FindObjectOfType<TapToStart>().GetComponent<Button>();
        }

        public void RestartLevel()
        {
            _cameraBrain.enabled = false;
            GlobalSpeedService.Instance.enabled = false;
            ScoreService.Instance.CheckRecord();
            MoneyService.Instance.SetInitialAmount();
            DistanceService.Instance.ResetDistance();
            _defeatPanel.gameObject.SetActive(false);
            _entitySpawner.ResetEntities();
            _chunkGenerator.ResetChunks();
            _tapToStartButton.enabled = true;
            _tapToStartButton.gameObject.SetActive(true);
            _cameraBrain.enabled = true;
            LevelRestarted?.Invoke();
        }
    }
}

