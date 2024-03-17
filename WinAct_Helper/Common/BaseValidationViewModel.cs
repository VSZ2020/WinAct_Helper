using System.Collections.ObjectModel;

namespace WinAct_Helper.Common
{
    public abstract class BaseValidationViewModel : BaseViewModel
    {
        public BaseValidationViewModel()
        {
            ValidationMessages = new();
        }

        #region ValidationMessage
        public ObservableCollection<ValidationMessage> ValidationMessages { get; }

        public bool IsValid => ValidationMessages?.Count == 0;

        public void AddErrorMessage(string text, string? title = null)
        {
            ValidationMessages.Add(new ValidationMessage() { Content = text, Title = title != null ? title : "" });
            OnChanged(nameof(IsValid));
        }

        public void ClearValidationMessages()
        {
            ValidationMessages.Clear();
            OnChanged(nameof(IsValid));
        } 
        #endregion
    }
}
