using System.Linq;
using System.Text;
using System.Web.Mvc;
using MrRondon.Presentation.Mvc.Areas.Admin.Controllers;

namespace MrRondon.Presentation.Mvc.Extensions
{
    public class MessageActionResultExtension : ActionResult
    {
        private readonly ActionResult _actionResult;
        private readonly string _message;
        private readonly MessageType _type;

        public MessageActionResultExtension(ActionResult actionResult, string message, MessageType type)
        {
            _actionResult = actionResult;
            _message = message;
            _type = type;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.TempData[_type.ToString()] = _message;
            _actionResult.ExecuteResult(context);
        }

        public enum MessageType
        {
            Success = 1,
            Error = 2,
            Alert = 3,
            Information = 4,
        }
    }

    public static class ActionResultExtensions
    {
        public static ActionResult Success(this ActionResult actionResult, string message)
        {
            return new MessageActionResultExtension(actionResult, message, MessageActionResultExtension.MessageType.Success);
        }

        public static ActionResult Error(this ActionResult actionResult, string message)
        {
            return new MessageActionResultExtension(actionResult, message, MessageActionResultExtension.MessageType.Error);
        }

        public static ActionResult Error(this ActionResult actionResult, ModelStateDictionary modelState)
        {
            var message = new StringBuilder();
            foreach (var error in modelState.Values.Where(x => x.Errors.Any()).SelectMany(s => s.Errors))
                {
                 message.Append($"{error.ErrorMessage} ");
            }

            return new MessageActionResultExtension(actionResult, message.ToString(), MessageActionResultExtension.MessageType.Error);
        }

        public static ActionResult Alert(this ActionResult actionResult, string message)
        {
            return new MessageActionResultExtension(actionResult, message, MessageActionResultExtension.MessageType.Alert);
        }

        public static ActionResult Information(this ActionResult actionResult, string message)
        {
            return new MessageActionResultExtension(actionResult, message, MessageActionResultExtension.MessageType.Information);
        }
    }
}