using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;

public class Bootstrap : MonoBehaviour
{
    [SerializeField, Scene] int _gameplayScene;

    private async void Start()
    {
        await LoadingScreen.Instance.Show();
        IProgress<float> progress = Progress.Create<float>(LoadingScreen.Instance.SetProgress);
        await SceneManager.LoadSceneAsync(_gameplayScene, LoadSceneMode.Additive).ToUniTask(progress);
    }
}
