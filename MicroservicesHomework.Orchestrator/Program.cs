using MicroservicesHomework.Orchestrator.Contracts;
using MicroservicesHomework.Orchestrator.Domain;

namespace MicroservicesHomework.Orchestrator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            var queue = new UsersQueue();

            var environmentVariables = Environment.GetEnvironmentVariables();
            var notificationsUrl = environmentVariables["notifications_url"] as string;
            var serviceAUrl = environmentVariables["servicea_url"] as string;
            var serviceBUrl = environmentVariables["serviceb_url"] as string;

            app.MapGet("/get-queue", () => queue);
            app.MapPost("/move-to-end", async (MoveToEndDto dto) =>
            {
                queue.MoveToEnd(dto.ProductName, dto.ClientId);

                await SendQueueToServiceA(serviceAUrl, queue);
                await SendQueueToServiceB(serviceBUrl, queue);
                await SendNotification(notificationsUrl, dto);

                return queue;
            });

            await Task.Delay(10000);
            await SendQueueToServiceA(serviceAUrl, queue);
            await SendQueueToServiceB(serviceBUrl, queue);

            app.Run();
        }

        private static async Task SendNotification(string url, MoveToEndDto dto)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(url),
            };

            await client.PostAsync(
                "/send-attempt-to-order-notification",
                JsonContent.Create(dto));
        }       
        
        private static async Task SendQueueToServiceA(string serviceAUrl, UsersQueue queue)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(serviceAUrl),
            };

            await client.PostAsync(
                $"/post-queue",
                JsonContent.Create(queue[UsersQueue.ProductFromServiceAName]));
        } 
        
        private static async Task SendQueueToServiceB(string serviceBUrl, UsersQueue queue)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(serviceBUrl),
            };

            await client.PostAsync(
                $"/post-queue",
                JsonContent.Create(queue[UsersQueue.ProductFromServiceBName]));
        }
    }
}
