using Assets.Scripts.General;
using System;

namespace Services
{
    public sealed class UpdateService : Singleton<UpdateService>
    {
        public Action OnLateUpdate;
        public Action OnUpdate;
        public Action OnFixedUpdate;

        private void LateUpdate() => OnLateUpdate?.Invoke();

        private void Update() => OnUpdate?.Invoke();

        private void FixedUpdate() => OnFixedUpdate?.Invoke();
    }
}