using Assets.Scripts.General;
using System;

namespace Services
{
    public sealed class Updater : Singleton<Updater>
    {
        public event Action OnLateUpdate;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;

        private void LateUpdate() => OnLateUpdate?.Invoke();

        private void Update() => OnUpdate?.Invoke();

        private void FixedUpdate() => OnFixedUpdate?.Invoke();
    }
}