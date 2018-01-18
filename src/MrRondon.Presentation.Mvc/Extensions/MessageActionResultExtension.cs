using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Extensions
{
    public class MessageActionResultExtension : ActionResult
    {
        private readonly ActionResult _actionResult;
        private readonly string _message;
        private readonly MessageType _type;

        public MessageActionResultExtension(ActionResult actionResult, string mensagem, MessageType type)
        {
            _actionResult = actionResult;
            _message = mensagem;
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
        public static ActionResult Success(this ActionResult actionResult, string mensagem)
        {
            return new MessageActionResultExtension(actionResult, mensagem, MessageActionResultExtension.MessageType.Success);
        }

        public static ActionResult Error(this ActionResult actionResult, string mensagem)
        {
            return new MessageActionResultExtension(actionResult, mensagem, MessageActionResultExtension.MessageType.Error);
        }

        public static ActionResult Alert(this ActionResult actionResult, string mensagem)
        {
            return new MessageActionResultExtension(actionResult, mensagem, MessageActionResultExtension.MessageType.Alert);
        }

        public static ActionResult Information(this ActionResult actionResult, string mensagem)
        {
            return new MessageActionResultExtension(actionResult, mensagem, MessageActionResultExtension.MessageType.Information);
        }
    }
}