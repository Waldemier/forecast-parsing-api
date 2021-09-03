using ForecastAPI.Data;
using ForecastAPI.Data.Common.Settings;
using ForecastAPI.Repositories.Implementations;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Services;
using ForecastAPI.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ForecastAPI
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
            var forecastSettings = new ForecastSettings();
            Configuration.GetSection("ForecastSettings").Bind(forecastSettings);
            services.AddSingleton(forecastSettings);

            services.AddScoped<IForecastApiService, ForecastApiService>();
            services.AddScoped<IForecastDbService, ForecastDbService>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddControllers(opt =>
            {
                //opt.Filters.Add(new );
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000");
            }));

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("ForecastDbConnectionString")));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForecastAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ForecastAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}