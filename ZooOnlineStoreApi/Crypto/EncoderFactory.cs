using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Crypto
{
    public class EncoderFactory
    {
        public static IEncoder CreateEncoderFactory()
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string serviceType = config.GetSection("ServiceType").Value ?? "default";
            switch (serviceType)
            {
                case "MD5":
                    return new MD5Encoder();
                case "SHA256":
                    return new SHA256Encoder();
                case "Bcrypt":
                    return new BCryptEncoder();
                default:
                    throw new InvalidOperationException($"unknown service: {serviceType}");
            }
        }
    }
}
