using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLoader : MonoBehaviour
{
    private int _loadedPercentage = 0;
    private LoadingProgressBar _progressBar;

    private void Awake() => _progressBar = FindObjectOfType<LoadingProgressBar>();

    private void Start() => StartCoroutine(SceneLoadingProgress());

    private IEnumerator SceneLoadingProgress()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        while (_loadedPercentage <= 100)
        {
            _loadedPercentage++;
            _progressBar.SetProgress(_loadedPercentage);
            yield return new WaitForFixedUpdate();
        }
        SceneManager.UnloadSceneAsync(1);
    }
}
