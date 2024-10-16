using System.Collections.Generic;
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
                app.Logger.LogInformation("ѕолучено сообщение о попытке заказа ({productName}, {clientId})", dto.ProductName, dto.ClientId);
                var list = queue.MoveToEnd(dto.ProductName, dto.ClientId);

                await SendNotification(notificationsUrl, dto);

                switch (dto.ProductName)
                {
                    case UsersQueue.ProductFromServiceAName:
                        await SendNextClient(serviceAUrl, list.First());
                        break;
                    case UsersQueue.ProductFromServiceBName:
                        await SendNextClient(serviceBUrl, list.First());
                        break;
                }

                return queue;
            });

            await Task.Delay(5000);
            await SendNextClient(serviceAUrl, queue[UsersQueue.ProductFromServiceAName].First());
            await SendNextClient(serviceBUrl, queue[UsersQueue.ProductFromServiceBName].First());

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
        
        private static async Task SendNextClient(string serviceUrl, long clientId)
        {
            var dto = new ClientIdDto
            {
                NextClientId = clientId,
            };

            using var client = new HttpClient()
            {
                BaseAddress = new Uri(serviceUrl),
            };

            await client.PostAsync(
                $"/post-queue",
                JsonContent.Create(dto));
        }

        private class ClientIdDto
        {
            public long NextClientId { get; set; }
        }
    }
}
