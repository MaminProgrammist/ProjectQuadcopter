using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Services;
using UI;

namespace Ads
{
    public class AdsRewardedButton : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
    {
        public event Action OnShowCompleted;

        private Button _button;
        private DefeatPanel _defeatPanel;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _defeatPanel = FindObjectOfType<DefeatPanel>();
        }

        private void Start() => Advertisement.Load(AdsInitializer.RewardedVideo, this);

        private void OnEnable() => _button.onClick.AddListener(ShowVideo);

        public void OnUnityAdsAdLoaded(string placementId) => Debug.Log("Ads was loaded");

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) => Debug.Log("Faled to load Ads");

        public void OnUnityAdsShowClick(string placementId) { }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Advertisement.Load(AdsInitializer.RewardedVideo, this);
                    SceneManager.LoadScene(0);
                    break;

                case UnityAdsShowCompletionState.COMPLETED:
                    Advertisement.Load(AdsInitializer.RewardedVideo, this);
                    GlobalSpeed.Instance.enabled = true;
                    OnShowCompleted?.Invoke();
                    break;

                case UnityAdsShowCompletionState.UNKNOWN:
                    SceneManager.LoadScene(0);
                    break;
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

        public void OnUnityAdsShowStart(string placementId) => _defeatPanel.gameObject.SetActive(false);

        private void ShowVideo() => Advertisement.Show(AdsInitializer.RewardedVideo, this);

        private void OnDisable() => _button.onClick.RemoveListener(ShowVideo);
    }
}
