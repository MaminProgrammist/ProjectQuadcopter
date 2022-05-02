﻿using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Config/Net", fileName = "New Net Config")]
    public class NetConfig : MultiplePrefabActorConfig<Net>
    {
        [SerializeField] [Range(1, 10)] private float _moveSpeed;
        [SerializeField] [Range(1, 10)] private float _rangeDetector;

        public float MoveSpeed { get => _moveSpeed; }
        public float RangeDetector { get => _rangeDetector; }
        
    }
}
