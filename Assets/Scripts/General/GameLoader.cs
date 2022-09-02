using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace General
{
    public class GameLoader : MonoBehaviour
    {
        public const int GameSceneIndex = 1;

        private Slider _progressSlider;

        private void Awake() => _progressSlider = GetComponentInChildren<Slider>();

        private void Start() => StartCoroutine(Loading());

        private IEnumerator Loading()
        {
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(GameSceneIndex);

            while (loadingOperation.isDone == false)
            {
                _progressSlider.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                yield return null;
            }

            yield break;
        }
    }
}