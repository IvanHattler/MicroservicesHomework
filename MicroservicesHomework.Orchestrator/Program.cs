using MicroservicesHomework.Orchestrator.Contracts;
using MicroservicesHomework.Orchestrator.Domain;

namespace MicroservicesHomework.Orchestrator
{
    public class Program
    {
        public static void Main(string[] args)
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

            app.MapGet("/get-queue", () => queue);
            app.MapPost("/move-to-end", async (MoveToEndDto dto) =>
            {
                queue.MoveToEnd(dto.ProductName, dto.ClientId);
                await SendNotification(dto, notificationsUrl);
                return queue;
            });

            app.Run();
        }

        private static async Task SendNotification(MoveToEndDto dto, string notificationsUrl)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(notificationsUrl),
            };

            var response = await client.PostAsync(
                "/send-attempt-to-order-notification",
                JsonContent.Create(dto));
        }
    }
}
