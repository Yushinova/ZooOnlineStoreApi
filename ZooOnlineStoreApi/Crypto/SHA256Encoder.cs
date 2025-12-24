using System.Security.Cryptography;
using System.Text;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Crypto
{
    public class SHA256Encoder : IEncoder
    {
        public string Encode(string data)
        {
            var inputBytes = Encoding.UTF8.GetBytes(data);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash).ToLower(); ;
        }

        public bool Verify(string plainData, string hashedData)
        {
            string encodedPlainData = Encode(plainData);
            return encodedPlainData == hashedData;
        }
    }
}
