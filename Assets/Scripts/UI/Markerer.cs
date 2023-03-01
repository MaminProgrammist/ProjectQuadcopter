using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Markerer : MonoBehaviour
    {
        private Image _marker;
        private Camera _camera;

        private void Awake() => _camera = Camera.main;

        public void SetImage(Image image) => _marker = image;

        private void OnEnable()
        {
            if (_marker != null)
            {
                UpdateService.Instance.OnUpdate += Mark;
                _marker.gameObject.SetActive(true);
            }
        }

        private void Mark() => _marker.rectTransform.position = _camera.WorldToScreenPoint(transform.position);

        private void OnDisable()
        {
            if (_marker != null)
            {
                UpdateService.Instance.OnUpdate -= Mark;
                _marker.gameObject.SetActive(false);
            }
        }
    }
}