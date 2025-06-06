using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace Forum.Application.Monitoring;

public class DomainMetrics
{
    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new(); 
    private readonly Meter _meter = new("FRM.Application");


    public void ForumFetched(bool successful)
    {
        IncrementCounter("forum_fetched", 1, 
            new Dictionary<string, object?> { ["successful"] = successful });
    }

    private void IncrementCounter(string key, int value, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = _counters.GetOrAdd(key, _ => _meter.CreateCounter<int>(key));
        counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }
}