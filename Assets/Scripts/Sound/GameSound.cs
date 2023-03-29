using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.General;
using UnityEngine;
using UnityEngine.Pool;
using NaughtyAttributes;

namespace Sound
{
    public class GameSound : Singleton<GameSound>
    {
        [SerializeField, BoxGroup("General")] private SoundAgent _agentPrefab;
        [SerializeField, BoxGroup("General"), Range(0, 1)] private float _globalVolume = 1f;

        private ObjectPool<SoundAgent> _agentsPool;
        private List<SoundAgent> _activeAgents = new();
        private bool _isEnabled = true;

        protected override void Init()
        {
            base.Init();

            _agentsPool = new ObjectPool<SoundAgent>(CreateAgent, OnGet, OnRelease, null, true);
        }

        private SoundAgent CreateAgent()
        {
            SoundAgent agent = Instantiate(_agentPrefab);
            return agent;
        }

        private void OnGet(SoundAgent agent)
        {
            _activeAgents.Add(agent);
        }

        private void OnRelease(SoundAgent agent)
        {
            _activeAgents.Remove(agent);
            agent.transform.SetParent(null);
            DontDestroyOnLoad(agent.gameObject);
        }

        public void ToggleSound(bool value)
        {
            _isEnabled = value;

            foreach (SoundAgent agent in _activeAgents)
            {
                agent?.ToggleSound(_isEnabled);
            }
        }

        public void SetGlobalVolume(float value)
        {
            _globalVolume = value;

            foreach (SoundAgent agent in _activeAgents)
            {
                agent?.SetGlobalVolume(_globalVolume);
            }
        }

        public void PlaySound(Transform target, AudioClip sound, float volume, float spatialBlend, bool isLooped, bool isStatic = true)
        {
            StartCoroutine(SoundPlaying(target, sound, volume, spatialBlend, isLooped, isStatic));
        }

        private IEnumerator SoundPlaying(Transform target, AudioClip sound, float volume, float spatialBlend, bool isLooped, bool isStatic)
        {
            SoundAgent agent = _agentsPool.Get();
            agent.Construct(sound, volume, spatialBlend, isLooped);
            agent.SetGlobalVolume(_globalVolume);
            agent.ToggleSound(_isEnabled);

            if (isStatic)
            {
                agent.transform.position = target.transform.position;
            }
            else
            {
                agent.transform.SetParent(target);
                agent.transform.localPosition = Vector3.zero;
            }

            yield return StartCoroutine(agent.Playing());
            _agentsPool.Release(agent);
        }
    }
}
