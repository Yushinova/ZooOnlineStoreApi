namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IEncoder
    {
        string Encode(string data);
        bool Verify(string plainData, string hashedData);
    }
}
