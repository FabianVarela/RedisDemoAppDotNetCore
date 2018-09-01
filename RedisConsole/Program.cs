namespace RedisConsole
{
    using System;
    using Microsoft.Extensions.Configuration;
    using RedisManager.Class;

    class Program
    {
        const string SecretName = "CacheConnection";

        private static IConfigurationRoot ConfigRedis { get; set; }

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            ConfigRedis = builder.Build();
        }

        static void Main(string[] args)
        {
            InitializeConfiguration();

            var conn = new Connection(ConfigRedis[SecretName]);
            Console.WriteLine(conn.TestConnection());
        }
    }
}
