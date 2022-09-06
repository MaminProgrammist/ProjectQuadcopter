using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;

public class LoadingProgressBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake() => _slider = GetComponent<Slider>();
    

    public void AddProgress(float percentage)
    {
        if (_slider.value >= 1) return;

        _slider.value += Mathf.Abs(percentage/100);
        Mathf.Clamp01(_slider.value);
    }

    public void SetProgress(float percentage) => _slider.value = Mathf.Clamp01((float)(percentage/100));

}
