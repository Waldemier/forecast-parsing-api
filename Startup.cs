using System.Reflection;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Settings;
using ForecastAPI.Data.Entities;
using ForecastAPI.Emailing.Extensions;
using ForecastAPI.Emailing.Services.Implementations;
using ForecastAPI.Emailing.Services.Interfaces;
using ForecastAPI.Emailing.Settings;
using ForecastAPI.Handlers;
using ForecastAPI.Repositories.Implementations;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Security.Extensions;
using ForecastAPI.Security.Services.Implementations;
using ForecastAPI.Security.Services.Interfaces;
using ForecastAPI.Security.Settings;
using ForecastAPI.Services;
using ForecastAPI.Services.Implementations;
using ForecastAPI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RazorLight.Extensions;

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
            var emailingSettings = new FluentEmailSettings();
            Configuration.GetSection("FluentEmailSettings").Bind(emailingSettings);
            services.AddSingleton(emailingSettings);
            
            var forecastSettings = new ForecastSettings();
            Configuration.GetSection("ForecastSettings").Bind(forecastSettings);
            services.AddSingleton(forecastSettings);

            var jwtSettings = new JwtSettings();
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            // Configure options for api controllers
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ValidationUserExistingFilter>();
            services.AddScoped<ValidationRequestFilter>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITemporaryTokensService, TemporaryTokenService>();

            services.AddScoped<IVerifyPasswordRepository, VerifyPasswordRepository>();
            services.AddScoped<IRegistrationConfirmRepository, RegistrationConfirmRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IForecastApiService, ForecastApiService>();
            services.AddScoped<IForecastDbService, ForecastDbService>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddSecurityServices();
            services.AddHttpContextAccessor();

            services.AddControllers(opt =>
            {
                // registered the filter globally
                opt.Filters.Add(new CustomExceptionFilter());
            }).AddNewtonsoftJson();

            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
            {
                // allow cookies, etc.
                policy.AllowCredentials() 
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination", "X-History-Pagination") // Allows to send the some custom headers like a "X-Pagination", that declared in AdminController GetAllUsers action
                    .WithOrigins(Configuration["Frontend:Url"]);
            }));

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("ForecastDbConnectionString")));
            
            services.ConfigureJsonWebToken(jwtSettings);
            services.ConfigureEmailing(emailingSettings);
            
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

            app.UseStaticFiles();
            
            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}