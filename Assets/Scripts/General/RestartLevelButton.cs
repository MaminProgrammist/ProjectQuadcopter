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
        private CinemachineBrain _cameraBrain;

        private void Awake()
        {
            _cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
            GetComponent<Button>().onClick.AddListener(RestartLevel);
            _defeatPanel = FindObjectOfType<DefeatPanel>();
            _entitySpawner = FindObjectOfType<EntitySpawner>();
            _chunkGenerator = FindObjectOfType<ChunkGenerator>();
        }

        public void RestartLevel()
        {
            _cameraBrain.enabled = false;
            GlobalSpeed.Instance.enabled = false;
            ScoreCounter.Instance.CheckRecord();
            Money.Instance.SetInitialAmount();
            Distance.Instance.ResetValue();
            _defeatPanel.gameObject.SetActive(false);
            _entitySpawner.ResetEntities();
            _chunkGenerator.ResetChunks();
            _cameraBrain.enabled = true;
            LevelRestarted?.Invoke();
        }
    }
}

