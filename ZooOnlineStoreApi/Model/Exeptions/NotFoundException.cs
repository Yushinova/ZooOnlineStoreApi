namespace ZooOnlineStoreApi.Model.Exeptions
{
    public class NotFoundException: ApplicationException
    {
        public NotFoundException(): base ("Not found exception") { }
    }
}
