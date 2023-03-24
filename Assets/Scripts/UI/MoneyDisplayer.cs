using Services;

namespace UI
{
    public class MoneyDisplayer : UIDisplayer 
    {
        private void OnEnable()
        {
            Money.Instance.OnChanged += Display;
        }

        private void OnDisable()
        {
            Money.Instance.OnChanged -= Display;
        }
    }
}
