using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System.Collections;

public class GameLoader : MonoBehaviour
{
    private int loadedPercentage = 0;
    private LoadingProgressBar _progressBar;

    private void Awake() => _progressBar = FindObjectOfType<LoadingProgressBar>();

    private void Start() => StartCoroutine(SceneLoadingProgress());

    private IEnumerator SceneLoadingProgress()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (loadedPercentage <= 100)
        {
            loadedPercentage++;
            _progressBar.SetProgress(loadedPercentage);
            yield return new WaitForFixedUpdate();
        }
        SceneManager.UnloadSceneAsync(0);
    }
}
