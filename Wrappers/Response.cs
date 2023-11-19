namespace CoolMate.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            ErrorCode = 200;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
