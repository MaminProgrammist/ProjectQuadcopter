using Services;

namespace UI
{
    public class DistanceDisplayer : UIDisplayer 
    {
        private void OnEnable()
        {
            DistanceService.Instance.OnChanged += Display;
        }

        private void OnDisable()
        {
            DistanceService.Instance.OnChanged -= Display;
        }
    }
}
