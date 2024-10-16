
using MicroservicesHomework.ServiceA.Services;

namespace MicroservicesHomework.ServiceB
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

            var environmentVariables = Environment.GetEnvironmentVariables();
            var orchestratorUrl = environmentVariables["orchestrator_url"] as string;

            builder.Services.AddHostedService(x => new ProductHostedService(orchestratorUrl));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapPost("/post-queue", (List<long> queue) =>
            {
                UsersQueue.Instance = queue;
            });

            app.Run();
        }
    }
}
