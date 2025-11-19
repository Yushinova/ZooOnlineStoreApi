namespace ZooOnlineStoreApi.Api.Jwt
{
    public class InvalidApiKeyException : ApplicationException
    {
        public InvalidApiKeyException() : base("api key is invalid") { }
    }
}
