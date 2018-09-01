namespace RedisManager.Class
{
    using System;
    using Interface;
    using Newtonsoft.Json;

    public class CacheManager<T> : ICacheManager<T> where T : class
    {
        private Connection connection;
        private int timeExipred;

        public CacheManager(Connection connection, int timeExipred)
        {
            this.connection = connection;
            this.timeExipred = timeExipred;
        }

        public bool Exists(string key) => connection.RedisConnection.GetDatabase().KeyExists(key);

        public T GetValue(string key)
        {
            var cache = connection.RedisConnection.GetDatabase();
            var members = cache.SetMembers(key);

            if (members.Length == 0) return default(T);

            var memberResult = JsonConvert.DeserializeObject<T>(members[0].ToString());
            return memberResult;
        }

        public void SetValue(string key, T value)
        {
            if (value == null) return;

            var cache = connection.RedisConnection.GetDatabase();
            var jsonValue = JsonConvert.SerializeObject(value);

            cache.SetAdd(key, jsonValue);
            cache.KeyExpire(key, TimeSpan.FromMinutes(timeExipred));
        }
    }
}
