using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

namespace Company.Function;

public class GetResumeCounter
{
    private readonly ILogger<GetResumeCounter> _logger;

    // Reuse CosmosClient (thread-safe)
    private static readonly CosmosClient _cosmosClient = new CosmosClient(
        Environment.GetEnvironmentVariable("AzureResumeConnectionString"));

    private static readonly Container _container =
        _cosmosClient.GetContainer("AzureResume", "Counter");

    public GetResumeCounter(ILogger<GetResumeCounter> logger) => _logger = logger;

    [Function("GetResumeCounter")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetResumeCounter")]
        HttpRequestData req)
    {
        // Read current document (id = "1", partition key = "1")
        Counter? current = null;
        try
        {
            var read = await _container.ReadItemAsync<Counter>("1", new PartitionKey("1"));
            current = read.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            current = null;
        }

        // Compute next count and upsert
        var next = current is null
            ? new Counter { Id = "1", Count = 1 }
            : new Counter { Id = current.Id, Count = current.Count + 1 };

        await _container.UpsertItemAsync(next, new PartitionKey("1"));

        // Return just the count
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync(next.Count.ToString());
        return res;
    }
}
