using System.Diagnostics;
using System.Text.Json;
using StackExchange.Redis;

namespace StudentPortal.PageView.Services;

public interface IReadOnlyAsyncKeyValueStorage<TKey, TValue>
{
    Task<TValue?> Get(TKey key);
    Task<bool> Has(TKey key);
}

public interface IAsyncKeyValueStorage<TKey, TValue> : IReadOnlyAsyncKeyValueStorage<TKey, TValue>
{
    Task Set(TKey key, TValue value);
    Task<bool> Delete(TKey key);
}

public class RedisTemporaryStorage(IDatabase database) : IAsyncKeyValueStorage<string, string>
{
    public Task<bool> Delete(string key)
    {
        return database.KeyDeleteAsync(key);
    }

    public async Task<string?> Get(string key)
    {
        var value = await database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            return default;
        }

        return value;
    }

    public Task<bool> Has(string key)
    {
        return database.KeyExistsAsync(key);
    }

    public async Task Set(string key, string value)
    {
        await database.StringSetAsync(key, value);
    }
}