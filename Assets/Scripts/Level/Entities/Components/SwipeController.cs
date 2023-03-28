using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Input;
using General;
using Services;
using Entities;
using System;
using System.Collections;

namespace Components
{
    public class SwipeController : ConfigReceiver<QuadcopterConfig>
    {
        public event Action<Vector3> OnMove;

        private WayMatrix _wayMatrix = new();
        private Vector2Int _currentPosition;
        private Animator _animator;
        private Dictionary<Vector2Int, string> _animations = new();
        private bool _canMove = true;

        public Vector2Int CurrentPosition
        {
            get => _currentPosition;

            private set
            {
                _currentPosition = new Vector2Int
                (
                    Mathf.Clamp(value.x, 0, WayMatrix.Width - 1),
                    Mathf.Clamp(value.y, 0, WayMatrix.Height - 1)
                );
            }
        }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _animations.Add(Vector2Int.up, AnimationService.States.UpStrafe);
            _animations.Add(Vector2Int.down, AnimationService.States.DownStrafe);
            _animations.Add(Vector2Int.left, AnimationService.States.LeftStrafe);
            _animations.Add(Vector2Int.right, AnimationService.States.RightStrafe);
        }

        private void OnEnable()
        {
            PlayerInput.Instance.OnHorizontal += MoveHorizontal;
            PlayerInput.Instance.OnVertical += MoveVertical;
        }

        public SwipeController SetStartablePosition(MatrixPosition position)
        {
            transform.position = _wayMatrix.GetPosition(position, out _currentPosition);
            return this;
        }

        public void SetPosition(MatrixPosition position) => _wayMatrix.GetPosition(position, out _currentPosition);

        private void MoveHorizontal(int direction)
        {
            if (_canMove == false)
                return;

            StartCoroutine(UpdatePosition(new(direction, 0)));
        }

        private void MoveVertical(int direction) => StartCoroutine(VerticalMotion(direction));

        private IEnumerator UpdatePosition(Vector2Int positionShift)
        {
            if (positionShift != Vector2Int.zero)
            {
                CurrentPosition = new Vector2Int(CurrentPosition.x + positionShift.x, CurrentPosition.y - positionShift.y);
                Vector3 updatedPosition = _wayMatrix.GetPositionByArrayCoordinates(CurrentPosition);
                _animator.Play(_animations[positionShift]);
                yield return transform.DOMove(updatedPosition, _config.MotionDuration).WaitForCompletion();
                OnMove?.Invoke(updatedPosition);
            }
        }

        private IEnumerator VerticalMotion(int direction)
        {
            if (_canMove == false)
                yield break;

            _canMove = false;
            yield return UpdatePosition(new(0, direction));
            yield return new WaitForSeconds(_config.VerticalSideHoldingDuration);
            yield return UpdatePosition(new(0, -direction));
            _canMove = true;
        }

        private void OnDisable()
        {
            PlayerInput.Instance.OnHorizontal -= MoveHorizontal;
            PlayerInput.Instance.OnVertical -= MoveVertical;
        }
    }
}