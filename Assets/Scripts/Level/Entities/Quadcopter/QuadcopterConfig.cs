using UnityEngine;
using General;

namespace Entities
{
    [CreateAssetMenu(menuName = "Config/Quadcopter", fileName = "New Quadcopter Config")]
    public class QuadcopterConfig : Config
    {
        [SerializeField] private Quadcopter _prefab;
        [SerializeField, Range(0, 10)] private float _motionDuration;
        [SerializeField, Range(1, 5)] private int _lives;
        [SerializeField, Range(0, 10)] private float _immortalModeTime;
        [SerializeField, Range(1, 5)] private int _charge;
        [SerializeField, Range(1, 15)] private int _chargeDecreaseTime;
        [SerializeField, Range(0, 15)] private int _money;
        [SerializeField, Range(1, 100)] private int _successfulDeliveryReward;
        [SerializeField, Range(1, 100)] private int _fineForFailedDelivery;
        [SerializeField] private Vector3 _pizzaConnectionPoint;
        [SerializeField] private ParticleSystem _poofParticle;
        [SerializeField] private ParticleSystem _destroyingParticle;
        [SerializeField] private AudioClip _poofSound;
        [SerializeField] private AudioClip _destroyingSound;

        public Quadcopter Prefab => _prefab;
        public int MaxLives => _lives;
        public float ImmortalModeTime => _immortalModeTime;
        public int ChargeLimit => _charge;
        public int ChargeDecreaseTime => _chargeDecreaseTime;
        public float MotionDuration => _motionDuration / 10;
        public int Money => _money;
        public int SuccessfulDeliveryReward => _successfulDeliveryReward;
        public int FineForFailedDelivery => _fineForFailedDelivery;
        public Vector3 PizzaConnectionPoint => _pizzaConnectionPoint;
        public ParticleSystem PoofParticle => _poofParticle;
        public ParticleSystem DestroyingParticle => _destroyingParticle;
        public AudioClip PoofSound => _poofSound;
        public AudioClip DestroyingSound => _destroyingSound;
    }
}
