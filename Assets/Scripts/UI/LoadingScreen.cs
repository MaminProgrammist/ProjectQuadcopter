using Assets.Scripts.General;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen>
{
    private CanvasGroup _canvasGroup;
    private Slider _slider;

    protected override void Init()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _slider = GetComponentInChildren<Slider>();
    }

    public void SetProgress(float progress) => _slider.value = progress;

    public async UniTask Show()
    {
        gameObject.SetActive(true);
        await DOTween.To(alpha => _canvasGroup.alpha = alpha, 0, 1, 0.1f);
    }

    public async UniTask Hide()
    {
        await DOTween.To(alpha => _canvasGroup.alpha = alpha, 1, 0, 0.1f);
        gameObject.SetActive(false);
    }
}
