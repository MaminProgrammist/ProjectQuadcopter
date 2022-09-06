using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class AdsBanner : MonoBehaviour
    {
        [SerializeField] private BannerPosition _position;

        private WaitForSeconds _waitingToShow = new(0.5f);

        private void Start()
        {
            Advertisement.Banner.SetPosition(_position);
            StartCoroutine(Showing());
        }

        private IEnumerator Showing()
        {
            while (Advertisement.isInitialized == false)
                yield return _waitingToShow;

            Advertisement.Banner.Show(AdsInitializer.Banner);
            yield break;
        }
    }
}