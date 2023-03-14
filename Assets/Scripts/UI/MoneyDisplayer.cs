using Services;

namespace UI
{
    public class MoneyDisplayer : UIDisplayer 
    {
        private void OnEnable()
        {
            MoneyService.Instance.OnChanged += Display;
        }

        private void OnDisable()
        {
            MoneyService.Instance.OnChanged -= Display;
        }
    }
}
