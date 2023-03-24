using Services;

namespace UI
{
    public class DistanceDisplayer : UIDisplayer 
    {
        private void OnEnable()
        {
            Distance.Instance.OnChanged += Display;
        }

        private void OnDisable()
        {
            Distance.Instance.OnChanged -= Display;
        }
    }
}
