using UnityEngine;
using System.Collections;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundAgent : BaseAgent
    {
        // public void Construct(AudioClip sound, float volume, float spatialBlend, bool isLooped)
        // {
        //     base.Construct(sound, volume, spatialBlend, isLooped);
            
        //     //_gameEvents.Subscribe(GameEventType.Paused, _audioSource.Pause);
        //     // _gameEvents.Subscribe(GameEventType.Resumed, _audioSource.UnPause);
        //     // _gameEvents.Subscribe(GameEventType.Restarted, _audioSource.Stop);
        // }

        public IEnumerator Playing()
        {
            _audioSource.Play();

            while (_audioSource.isPlaying)
            {
                yield return null;
            }

            yield break;
        }

        // protected override void OnDestroy()
        // {
        //     // _gameEvents.UnSubscribe(GameEventType.Paused, _audioSource.Pause);
        //     // _gameEvents.UnSubscribe(GameEventType.Resumed, _audioSource.UnPause);
        //     // _gameEvents.UnSubscribe(GameEventType.Restarted, _audioSource.Stop);
        // }
    }
}
