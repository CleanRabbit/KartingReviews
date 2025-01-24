
using Summers.Wyvern.Server.MongoDb;
using Summers.Wyvern.Server.MongoDb.Database;

namespace Summers.Wyvern.DataAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Configuration.AddJsonFile($"appsettings.json", true, true);

			// Add services to the container.
			builder.Services.AddControllers();
			//builder.Services.AddScoped<Server.Host, Server.Host>();
			builder.Services.AddSingleton<IDataAccessController, WyvernDataAccessController>();

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

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.Services.GetService<IDataAccessController>()?.Connect(app.Configuration);

			app.MapControllers();

			app.Run();
		}
	}
}
