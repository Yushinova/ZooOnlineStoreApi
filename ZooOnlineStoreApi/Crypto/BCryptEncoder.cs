using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Crypto
{
    public class BCryptEncoder : IEncoder
    {
        public string Encode(string data)
        {
            string key = "$2a$12$ncjskFMRG08WaoGrZkXhGe";
            string cryptdata = BCrypt.Net.BCrypt.HashPassword(data, key);
            //Console.WriteLine(BCrypt.Net.BCrypt.Verify(data,cryptdata));//сравнение исходных данных и полученных
            return cryptdata;
        }
        public bool Verify(string plainData, string hashedData)
        {

            return BCrypt.Net.BCrypt.Verify(plainData, hashedData);

        }
    }
}
