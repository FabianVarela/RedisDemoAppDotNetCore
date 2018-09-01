namespace RedisManager.Interface
{
    public interface ICacheManager<T>
    {
        bool Exists(string key);
        T GetValue(string key);
        void SetValue(string key, T value);
    }
}
