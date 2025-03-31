namespace GameOfLifeApi.Exception
{
    public class ErrorResponse
    {
        public string? Endpoint { get; set; }
        public int StatusCode { get; set; }
        public string? Title { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
    }
}
