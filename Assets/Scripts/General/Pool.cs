using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace General
{
    public class Pool<T> : IPool<T> where T : MonoBehaviour
    {
        private IFactory<T> _factory;
        private List<T> _elements;
        private Container _container;

        public bool IsInitialized { get; private set; }

        public Pool(IFactory<T> factory, Container container, int amount)
        {
            _factory = factory;
            _container = container;
            _elements = new List<T>();

            for (int i = 0; i < amount; i++)
                Create(false);

            IsInitialized = true;
        }

        public T Get(Vector3 spawnPosition)
        {
            if (HasAvailable(out T availableElement))
            {
                availableElement.transform.position = spawnPosition;
                return availableElement;
            }

            T createdElement = Create(true);
            createdElement.transform.position = spawnPosition;
            return createdElement;
        }

        public void ReleaseAll()
        {
            _elements
                .Where(element => element.gameObject.activeSelf == true).ToList()
                .ForEach(element => element.gameObject.SetActive(false));
        }

        private bool HasAvailable(out T availableElement)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                T element = _elements[Random.Range(0, _elements.Count)];

                if (element.gameObject.activeSelf == false)
                {
                    element.gameObject.SetActive(true);
                    availableElement = element;
                    return true;
                }
            }

            availableElement = null;
            return false;
        }

        protected T Create(bool isActive)
        {
            T element = _factory.GetCreated();
            _elements.Add(element);
            element.transform.SetParent(_container.transform);
            element.gameObject.SetActive(isActive);
            return element;
        }
    }
}
