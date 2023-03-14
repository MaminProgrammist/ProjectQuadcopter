using System.Linq;
using UnityEngine;

namespace General
{
    public class ConfigProvider<C> : MonoBehaviour where C : Config
    {
        private readonly C _config;

        private void Awake() => GetComponents<ConfigReceiver<C>>().ToList().ForEach(configReceiver => configReceiver.Receive(_config));
    }
}
