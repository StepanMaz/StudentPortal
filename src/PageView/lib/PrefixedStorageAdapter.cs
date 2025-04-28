using StudentPortal.PageView.Services;

namespace StudentPortal.PageView;

public class PrefixedStorageAdapter<T>(string prefix, IAsyncKeyValueStorage<string, T> @base) : IAsyncKeyValueStorage<string, T>
{
    public Task<bool> Delete(string key)
    {
        return @base.Delete(GetPrefixedKey(key));
    }

    public Task<T?> Get(string key)
    {
        return @base.Get(GetPrefixedKey(key));
    }

    public Task<bool> Has(string key)
    {
        return @base.Has(GetPrefixedKey(key));
    }

    public Task Set(string key, T value)
    {
        return @base.Set(GetPrefixedKey(key), value);
    }

    private string GetPrefixedKey(string key) => $"{prefix}-{key}";
}
