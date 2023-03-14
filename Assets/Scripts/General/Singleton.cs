using UnityEngine;

namespace Assets.Scripts.General
{
    public class Singleton<T> : MonoBehaviour  where T : Component
    {
        public static T Instance { get; private set; }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Instance.transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

            Init();
        }

        protected virtual void Init() { }
    }
}
