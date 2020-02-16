using System.Text.Json.Serialization;
using GoodNews.Models.Settings;
using GoodNews.Repositories;
using GoodNews.Repositories.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Good News!", Version = "v1"}));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.Configure<MySqlSettings>(
                Configuration.GetSection("Database").GetSection(nameof(MySqlSettings)));
            services.AddSingleton<IMySqlSettings>(sp =>
                sp.GetRequiredService<IOptions<MySqlSettings>>().Value);
            
            services.Configure<PostgresSettings>(
                Configuration.GetSection("Database").GetSection(nameof(PostgresSettings)));
            services.AddSingleton<IPostgresSettings>(sp =>
                sp.GetRequiredService<IOptions<PostgresSettings>>().Value);

            var conString = Configuration
                .GetSection("Database")
                .GetSection(nameof(PostgresSettings))
                .Get<PostgresSettings>().ConnectionString;
            services.AddDbContext<GoodNewsDBContext>(options =>
                options.UseNpgsql(conString));

            // Repositories
            services.AddSingleton<INewsHeadlineRepository, NewsHeadlineRepository>();
            services.AddSingleton<IAnnotationRepository, AnnotationRepository>();
            services.AddSingleton<ISessionRepository, SessionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var db = app.ApplicationServices.GetRequiredService<GoodNewsDBContext>();
            db.Database.EnsureCreated();

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