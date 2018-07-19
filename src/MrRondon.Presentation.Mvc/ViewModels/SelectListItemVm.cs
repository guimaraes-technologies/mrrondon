namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class SelectListItemVm
    {
        public SelectListItemVm(object value, string text, bool isSelected = false)
        {
            Value = value;
            Text = text;
            Selected = isSelected;
        }

        public object Value { get; private set; }
        public string Text { get; private set; }
        public bool Selected { get; private set; }
    }
}