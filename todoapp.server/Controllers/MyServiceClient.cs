namespace todoapp.server.Controllers
{
    public class MyServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MyServiceClient> _logger;

        public MyServiceClient(
            HttpClient httpClient,
            ILogger<MyServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Service discovery integration point
        public async Task<List<Object>> GetDiscoveryDataAsync(string serviceName)
        {
            try
            {
                // Service name resolution happens automatically
                var response = await _httpClient.GetAsync($"/api/{serviceName}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Object>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Service discovery failed for {Service}", serviceName);
                throw new Exception("Service unavailable", ex);
            }
        }
    }
}
