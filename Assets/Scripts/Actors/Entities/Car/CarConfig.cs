﻿using UnityEngine;
using NaughtyAttributes;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Config/Car", fileName = "New Car Config")]
    public class CarConfig : Config, ICanMove, ICanDetect
    {
        [SerializeField] private Car[] _prefabs;
        [SerializeField] [Range(1, 100)] private float _selfSpeed;
        [SerializeField, MinMaxSlider(-100, 100), BoxGroup("Detection")] private Vector2 _xDetectionRange;
        [SerializeField, MinMaxSlider(-100, 100), BoxGroup("Detection")] private Vector2 _zDetectionRange;
        [SerializeField, MinMaxSlider(-100, 100), BoxGroup("Detection")] private Vector2 _yDetectionRange;

        public Car Prefab => _prefabGetter.Get(_prefabs);
        public float SelfSpeed => _selfSpeed;
        public float XDetectionDistanceLeft => _xDetectionRange.x;
        public float XDetectionDistanceRight => _xDetectionRange.y;
        public float ZDetectionDistanceForward => _zDetectionRange.x;
        public float ZDetectionDistanceBackward => _zDetectionRange.y;
        public float YDetectionDistanceUp => _yDetectionRange.x;
        public float YDetectionDistanceDown => _yDetectionRange.y;
    }
}
