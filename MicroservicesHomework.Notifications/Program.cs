
using MicroservicesHomework.Orchestrator.Contracts;

namespace MicroservicesHomework.Notifications
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
            builder.Services.AddLogging();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapPost("/send-attempt-to-order-notification", (AttemptToOrderNotificationDto dto) =>
            {
                app.Logger.LogInformation("Была совершена попытка заказать продукт {productName} для клиента {clientId}",
                    dto.ProductName,
                    dto.ClientId);
            });

            app.Run();
        }
    }
}
