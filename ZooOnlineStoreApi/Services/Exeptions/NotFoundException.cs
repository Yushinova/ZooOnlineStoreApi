namespace ZooOnlineStoreApi.Services.Exeptions
{
    public class NotFoundException: ApplicationException
    {
        public NotFoundException(): base ("Not found exception") { }
        public NotFoundException(string message) : base(message) { }
    }
}
