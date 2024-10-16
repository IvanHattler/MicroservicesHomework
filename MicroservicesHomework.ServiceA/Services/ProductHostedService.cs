

namespace MicroservicesHomework.ServiceA.Services
{
    public class ProductHostedService : BackgroundService
    {
        private readonly string _orchestratorUrl;

        public ProductHostedService(string orchestratorUrl)
        {
            _orchestratorUrl = orchestratorUrl;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (UsersQueue.Instance != null && UsersQueue.Instance.Count > 0)
                {
                    using var client = new HttpClient()
                    {
                        BaseAddress = new Uri(_orchestratorUrl),
                    };

                    var clientId = UsersQueue.Instance.First();
                    UsersQueue.Instance.Remove(clientId);

                    var resp = await client.PostAsync("/move-to-end", JsonContent.Create(new
                        {
                            UsersQueue.ProductName,
                            ClientId = clientId,
                        }), stoppingToken);
                }

                await Task.Delay(1000 + new Random().Next(5000), stoppingToken);
            }
        }
    }
}
