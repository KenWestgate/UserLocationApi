using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Security.Authentication;
using System.Text;
using TechnicalTest.Api.Services;

namespace TechnicalTest.Api
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
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization();

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });

            services.AddDistributedMemoryCache();
            // TODO: refine options after performance testing
            var options = ConfigurationOptions.Parse(Configuration["ConnectionStrings:RedisCache"]);
            options.AbortOnConnectFail = false;
            options.AllowAdmin = false;
            options.ConnectRetry = 5;
            options.ConnectTimeout = 30000;
            options.ReconnectRetryPolicy = new ExponentialRetry(1000, 10000);
            options.SyncTimeout = 3000;
            options.SslProtocols = SslProtocols.Tls12;
            services.AddStackExchangeRedisCache(o => o.ConfigurationOptions = options);

            services.AddScoped<IUserLocationService, UserLocationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IDistributedCache cache)
        {
            app.UseAuthentication();

            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTimeUTC = DateTime.UtcNow.ToString();
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                // set expiration to a year in lieu of using real persistent storage
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(new DateTimeOffset(DateTime.Now.AddYears(1)));
                cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech Test");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
