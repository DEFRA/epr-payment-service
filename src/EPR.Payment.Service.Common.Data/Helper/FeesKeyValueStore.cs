namespace EPR.Payment.Service.Common.Data.Helper
{
    public class FeesKeyValueStore
    {
        public Dictionary<string, object> Data { get; } = new();

        public void Add(string key, object value) => Data[key] = value;
        public object? Get(string key) => Data.TryGetValue(key, out var value) ? value : null;
    }
}
