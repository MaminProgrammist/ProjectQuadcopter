using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace UI
{
    public class Logo : MonoBehaviour
    {
        private VideoPlayer _player;

        private void Awake() => _player = GetComponent<VideoPlayer>();

        private void Start() => StartCoroutine(Waiting());

        private IEnumerator Waiting()
        {
            yield return new WaitForSeconds((float)_player.length);
            SceneManager.LoadScene(1);
            yield break;
        }
    }
}