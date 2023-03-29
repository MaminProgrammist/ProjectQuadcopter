using UnityEngine;

namespace Sound
{
    public abstract class BaseAgent : MonoBehaviour
    {
        protected AudioSource _audioSource;
        protected float _globalVolume;
        protected float _volume;
        protected bool _isEnabled;

        public virtual void Construct(AudioClip sound, float volume, float spatialBlend, bool isLooped)
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = sound;
            _audioSource.loop = isLooped;
            _audioSource.spatialBlend = spatialBlend;
            SetVolume(volume);
            _audioSource.Play();
        }

        public void SetVolume(float value)
        {
            _volume = value;
            UpdateVolume();
        }

        public void SetGlobalVolume(float value)
        {
            _globalVolume = value;
            UpdateVolume();
        }

        public void ToggleSound(bool value)
        {
            _isEnabled = value;

            if (value)
            {
                UpdateVolume();
            }
            else
            {
                _audioSource.volume = 0;
            }
        }

        protected void UpdateVolume()
        {
            if (_isEnabled)
            {
                _audioSource.volume = _volume * _globalVolume;
            }
        }

        public void ChangeClip(AudioClip clip)
        {
            _audioSource.clip = clip;
        }
    }
}