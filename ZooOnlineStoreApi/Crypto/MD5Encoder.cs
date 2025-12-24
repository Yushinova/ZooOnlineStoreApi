using System.Security.Cryptography;
using System.Text;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Crypto
{
    public class MD5Encoder: IEncoder
    {
        public string Encode(string data)
        {
            using (MD5 md5Hash = MD5.Create())
            {
               //преобразуем строку в массив байтов
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
               //создаем строку из массива байтов
                StringBuilder sBuilder = new StringBuilder();
                //каждый байт в шестнадцатеричное представление
                for (int i = 0; i < bytes.Length; i++)
                {
                    sBuilder.Append(bytes[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public bool Verify(string plainData, string hashedData)
        {
            string encodedPlainData = Encode(plainData);
            return string.Equals(encodedPlainData, hashedData, StringComparison.OrdinalIgnoreCase);
        }
    }
}
