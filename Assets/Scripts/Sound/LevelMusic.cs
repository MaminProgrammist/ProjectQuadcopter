using UnityEngine;

namespace Sound
{
    public class LevelMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField, Range(0, 1)] private float _volume = 1f;

        private void Start()
        {
            GameSound.Instance.PlaySound(transform, _clip, _volume, 0f, true);
        }
    }
}