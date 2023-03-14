using UnityEngine;
using General;
using Assets.Scripts.General;

namespace Services
{
    public class ContainerService : Singleton<ContainerService>
    {
        public Container GetCreatedContainer(string title, Transform parent)
        {
            GameObject container = new GameObject(title);
            container.transform.SetParent(parent);
            container.transform.position = Vector3.zero;
            return container.AddComponent<Container>();
        }

        public Container GetCreatedContainer(string title, Transform parent, Vector3 position)
        {
            GameObject container = new GameObject(title);
            container.transform.SetParent(parent);
            container.transform.position = position;
            return container.AddComponent<Container>();
        }
    }
}