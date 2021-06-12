using System.Text.Json.Serialization;
using GoodNews.Models.Settings;
using GoodNews.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

namespace GoodNews
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers().AddJsonOptions(opt =>
      {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      });

      services.AddSwaggerGen(c =>
          c.SwaggerDoc("v1", new OpenApiInfo { Title = "Good News!", Version = "v1" }));

      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
                  builder => builder
                      .AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
      });

      // requires using Microsoft.Extensions.Options
      services.Configure<MongoSettings>(
          Configuration.GetSection("Database").GetSection(nameof(MongoSettings)));

      services.AddSingleton<IMongoSettings>(sp =>
          sp.GetRequiredService<IOptions<MongoSettings>>().Value);

      // Repositories
      services.AddSingleton<INewsHeadlineRepository, Repositories.Mongo.NewsHeadlineRepository>();
      services.AddSingleton<ISessionRepository, Repositories.Mongo.SessionRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

      app.UseSerilogRequestLogging();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Good News! V1");
        c.RoutePrefix = string.Empty;
      });

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors("CorsPolicy");

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}