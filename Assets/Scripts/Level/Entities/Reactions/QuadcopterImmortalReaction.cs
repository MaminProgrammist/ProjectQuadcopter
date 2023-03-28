using System.Collections;
using UnityEngine;
using Entities;
using Components;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Reactions
{
    public class QuadcopterImmortalReaction : Reaction
    {
        private QuadcopterConfig _config;
        private Collider _collider;
        private SwipeController _swipeController;
        private SkinnedMeshRenderer _renderer;

        public QuadcopterImmortalReaction(Quadcopter quadcopter, QuadcopterConfig config)
        {
            _config = config;
            _collider = quadcopter.GetComponent<Collider>();
            _swipeController = quadcopter.GetComponent<SwipeController>();
            _renderer = quadcopter.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public override void React()
        {
            _swipeController.StartCoroutine(Immortaling());
            _swipeController.StartCoroutine(ControlDisabling());
        }

        private IEnumerator ControlDisabling()
        {
            _swipeController.enabled = false;
            yield return new WaitForSeconds(_config.ImmortalModeTime / 5);
            _swipeController.enabled = true;
            yield break;
        }

        private IEnumerator Immortaling()
        {
            _collider.enabled = false;
            _renderer.enabled = true;
            Color defaultColor = _renderer.material.color;
            TweenerCore<Color, Color, ColorOptions> tween = _renderer.material.DOColor(Color.green, 0.25f).SetLoops(-1, LoopType.Yoyo);
            yield return new WaitForSeconds(_config.ImmortalModeTime);
            tween.Kill();
            _renderer.material.color = defaultColor;
            _collider.enabled = true;
            yield break;
        }
    }
}
