using System;
using System.Globalization;
using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Helpers
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var modelState = new ModelState
            {
                Value = valueResult
            };

            object actualValue = null;

            var valueResultAttemptedValue = valueResult.AttemptedValue;

            var wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            var alternateSeperator = wantedSeperator == "," ? "." : ",";

            if (valueResultAttemptedValue.IndexOf(wantedSeperator, StringComparison.Ordinal) == -1 && valueResultAttemptedValue.IndexOf(alternateSeperator, StringComparison.Ordinal) != -1)
            {
                valueResultAttemptedValue = valueResult.AttemptedValue.Replace(alternateSeperator, wantedSeperator);
            }

            try
            {
                actualValue = decimal.Parse(valueResultAttemptedValue, NumberStyles.Any);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}