using Assets.Scripts.General;
using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Input
{
    public class PlayerInput : Singleton<PlayerInput>
    {
        public event Action<int> OnHorizontal;
        public event Action<int> OnVertical;
        public event Action OnTap;

        [SerializeField, Range(0, 1000)] private int _deadZone = 30;
        [SerializeField, Range(0, 1)] private float _threshold = 0.2f;
        
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerUp += Swipe;
        }

        private void Swipe(Finger finger)
        {
            Vector2 swipeDirection = (finger.screenPosition - finger.currentTouch.startScreenPosition);

            if (swipeDirection.magnitude >= _deadZone)
            {
                Vector2Int direction = CalculateDirection(swipeDirection.normalized);

                if (direction.x != 0)
                    OnHorizontal?.Invoke(direction.x);

                if (direction.y != 0)
                    OnVertical?.Invoke(direction.y);
            }

            if (finger.currentTouch.isTap)
            {
                OnTap?.Invoke();
            }
        }

        private Vector2Int CalculateDirection(Vector2 normalizedSwipeDirection)
        {
            int x = Mathf.RoundToInt(normalizedSwipeDirection.x - _threshold * Mathf.Sign(normalizedSwipeDirection.x));
            int y = Mathf.RoundToInt(normalizedSwipeDirection.y - _threshold * Mathf.Sign(normalizedSwipeDirection.y));
            return new Vector2Int(x, y);
        }

        private void OnDisable()
        {
            Touch.onFingerUp -= Swipe;
            EnhancedTouchSupport.Disable();
        }
    }
}