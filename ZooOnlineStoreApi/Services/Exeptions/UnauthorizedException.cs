namespace ZooOnlineStoreApi.Services.Exeptions
{
    public class UnauthorizedException: ApplicationException
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
