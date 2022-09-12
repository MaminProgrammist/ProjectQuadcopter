using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class LifeDisplayer : MonoBehaviour 
    {
        private Image[] _icons;

        private void Awake() => _icons = GetComponentsInChildren<Image>();

        public void Display(int lifes)
        {
            for (int i = 0; i < _icons.Length; i++)
            {
                Image icon = _icons[i];

                if (i < lifes)
                {
                    icon.gameObject.SetActive(true);
                    continue;
                }

                icon.rectTransform
                    .DOPunchScale(Vector3.one, 0.25f, 2, 0)
                    .OnComplete(() => icon.gameObject.SetActive(false));
            }
        }
    }
}
