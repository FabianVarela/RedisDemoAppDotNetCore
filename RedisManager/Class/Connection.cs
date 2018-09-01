namespace RedisManager.Class
{
    using StackExchange.Redis;
    using System;

    public class Connection
    {
        private Lazy<ConnectionMultiplexer> lazyConnection = null;

        public Connection(string connectionString)
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = connectionString;
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        public ConnectionMultiplexer RedisConnection
        {
            get => lazyConnection.Value;
        }

        public string TestConnection()
        {
            string value = String.Empty;
            IDatabase cache = RedisConnection.GetDatabase();

            string cacheCommand = "PING";
            value += $"\nCache command  : {cacheCommand}";
            value += $"\nCache response : {cache.Execute(cacheCommand).ToString()} \n";

            cacheCommand = "GET Message";
            value += $"\nCache command  : {cacheCommand} or StringGet()";
            value += $"\nCache response : {cache.StringGet("Message").ToString()} \n";

            cacheCommand = "SET Message \"Hello! The cache is working from a .NET Core console app!\"";
            value += $"\nCache command  : {cacheCommand} or StringSet()";
            value += $"\nCache response : {cache.StringSet("Message", "Hello! The cache is working from a .NET Core console app!").ToString()} \n";

            cacheCommand = "GET Message";
            value += $"\nCache command  : {cacheCommand} or StringGet()";
            value += $"\nCache response : {cache.StringGet("Message").ToString()} \n";

            cacheCommand = "CLIENT LIST";
            value += $"\nCache command  : {cacheCommand}";
            value += $"\nCache response : \n {cache.Execute("CLIENT", "LIST").ToString().Replace("id=", "id=")}";

            lazyConnection.Value.Dispose();

            return value;
        }
    }
}
