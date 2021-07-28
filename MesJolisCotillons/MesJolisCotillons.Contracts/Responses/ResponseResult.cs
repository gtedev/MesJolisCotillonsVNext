namespace MesJolisCotillons.Contracts.Responses
{
    public class ResponseResult<T> : ResponseBase
    {
        public ResponseResult(bool success)
            : base(success)
        {
        }

        public T Result { get; }
    }
}
