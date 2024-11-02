using Newtonsoft.Json;
using StackExchange.Redis;

namespace StudentPortalServer.Services;

public interface IAsyncKeyValueStorage
{
    Task SetAsync<T>(string key, T value);
    Task<T?> GetAsync<T>(string key);
    Task<bool> DeleteAsync(string key);
    Task<bool> ExistsAsync(string key);
}

public class PrefixedAsyncKeyValueStorage(string prefix, IAsyncKeyValueStorage storage) : IAsyncKeyValueStorage
{
    private string Prefixed(string key)
    {
        return $"{prefix}-{key}";
    }

    public Task<bool> DeleteAsync(string key)
    {
        return storage.DeleteAsync(Prefixed(key));
    }

    public Task<bool> ExistsAsync(string key)
    {
        return storage.ExistsAsync(Prefixed(key));
    }

    public Task<T?> GetAsync<T>(string key)
    {
        return storage.GetAsync<T>(Prefixed(key));
    }

    public Task SetAsync<T>(string key, T value)
    {
        return storage.SetAsync(Prefixed(key), value);
    }
}


public class RedisKeyValueStorage(IDatabase db) : IAsyncKeyValueStorage
{
    public async Task SetAsync<T>(string key, T value)
    {
        var jsonData = JsonConvert.SerializeObject(value);
        await db.StringSetAsync(key, jsonData);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await db.StringGetAsync(key);
        if (jsonData.IsNullOrEmpty)
        {
            return default(T);
        }
        return JsonConvert.DeserializeObject<T>(jsonData!)!;
    }

    public async Task<bool> DeleteAsync(string key)
    {
        return await db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await db.KeyExistsAsync(key);
    }
}