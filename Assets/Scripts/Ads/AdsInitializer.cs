using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] private bool _isTestMode;

        public const string GameID = "4865760";
        public const string RewardedVideo = "Rewarded_Android";
        public const string Banner = "Banner_Android";

        private void Awake() => Advertisement.Initialize(GameID, _isTestMode, this);

        public void OnInitializationComplete() => Debug.Log("AddsInitComplete");

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) => Debug.Log("AddsInitFaled : " + message);
    }
}
