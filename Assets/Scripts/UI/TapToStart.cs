using UnityEngine;
using Services;

namespace UI
{
    public class TapToStart : MonoBehaviour
    {
        private void OnEnable() => GlobalSpeedService.Instance.OnStartup += () => gameObject.SetActive(false);

        private void OnDisable() => GlobalSpeedService.Instance.OnStartup -= () => gameObject?.SetActive(false);
    }
}
