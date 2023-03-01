using Assets.Scripts.General;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen>
{
    private Slider _slider;

    protected override void Init()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetProgress(float progress) => _slider.value = progress;
}
