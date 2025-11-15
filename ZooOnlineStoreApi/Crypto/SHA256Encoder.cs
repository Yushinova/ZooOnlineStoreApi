using System.Security.Cryptography;
using System.Text;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Crypto
{
    public class SHA256Encoder : IEncoder
    {
        public string Encode(string data)
        {
            var inputBytes = Encoding.UTF8.GetBytes(data);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }
    }
}
