using UnityEngine;
using NaughtyAttributes;
using General;

namespace Entities
{
    // [CreateAssetMenu(menuName = "Config/PizzaGuy", fileName = "New Pizza Guy Config")]
    [System.Obsolete]
    public class PizzaGuyConfig : Config, ICanDetect, ICanMove
    {
        [SerializeField] private PizzaGuy[] _prefabs;
        [SerializeField, MinMaxSlider(-100, 100), BoxGroup("Detection")] private Vector2 _xDetectionRange;
        [SerializeField, MinMaxSlider(-100, 100), BoxGroup("Detection")] private Vector2 _zDetectionRange;
        [SerializeField, MinMaxSlider(-100, 100), BoxGroup("Detection")] private Vector2 _yDetectionRange;

        public PizzaGuy Prefab => _prefabGetter.Get(_prefabs);
        public float SelfSpeed => 0;
        public float XDetectionDistanceLeft => _xDetectionRange.x;
        public float XDetectionDistanceRight => _xDetectionRange.y;
        public float ZDetectionDistanceForward => _zDetectionRange.y;
        public float ZDetectionDistanceBackward => _zDetectionRange.x;
        public float YDetectionDistanceUp => _yDetectionRange.y;
        public float YDetectionDistanceDown => _yDetectionRange.x;
    }
}


