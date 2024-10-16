namespace MicroservicesHomework.ServiceB.Services
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
                if (UsersQueue.NextClientId != null)
                {
                    var clientId = UsersQueue.NextClientId.Value;
                    UsersQueue.NextClientId = null;
                    await SendSucessMessage(clientId, stoppingToken);
                }

                await Task.Delay(1000 + new Random().Next(5000), stoppingToken);
            }
        }

        private async Task SendSucessMessage(long clientId, CancellationToken stoppingToken)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(_orchestratorUrl),
            };

            var resp = await client.PostAsync("/move-to-end", JsonContent.Create(new
            {
                UsersQueue.ProductName,
                ClientId = clientId,
            }), stoppingToken);
        }
    }
}
