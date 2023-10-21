namespace HomeworkManager.Model.ErrorEntities
{
    public class BusinessError
    {
        public string Message { get; }
        public string[] Messages { get; }

        public BusinessError()
        {
            Message = "Something went wrong!";
            Messages = new[] { "Something went wrong!" };
        }

        public BusinessError(string message)
        {
            Message = message;
            Messages = new[] { message };
        }

        public BusinessError(params string[] messages)
        {
            Message = messages[0];
            Messages = messages;
        }
    }
}