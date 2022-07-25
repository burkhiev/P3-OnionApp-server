namespace OnionApp.Utilities.ResponseTypes
{
    public class ClientErrorResponse
    {
        public int Status { get; set; }
        public List<KeyValuePair<string, string>>? Errors { get; set; }
    }
}
