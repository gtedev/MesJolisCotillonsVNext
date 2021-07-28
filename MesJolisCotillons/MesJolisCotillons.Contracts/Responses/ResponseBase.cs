namespace MesJolisCotillons.Contracts.Responses
{
    using System.Collections.Generic;

    public abstract class ResponseBase : IResponse
    {
        public ResponseBase(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; }

        public List<string> Messages { get; } = new List<string>();

        public void AddMessages(params string[] messages)
        {
            this.Messages.AddRange(messages);
        }
    }
}
