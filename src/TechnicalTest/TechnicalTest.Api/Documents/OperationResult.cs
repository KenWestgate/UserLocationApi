namespace TechnicalTest.Api.Documents
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public T Model { get; set;}
    }
}
