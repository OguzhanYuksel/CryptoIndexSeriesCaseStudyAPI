using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CryptoIndexSeriesCaseStudyAPI.Clients;
using CryptoIndexSeriesCaseStudyAPI.Services.Concrete;
using CryptoIndexSeriesCaseStudyAPI.Services.Interfaces;
using AutoMapper;
using CryptoIndexSeriesCaseStudyAPI.MappingProfiles;

namespace CryptoIndexSeriesCaseStudyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoIndexSeriesCaseStudyAPI", Version = "v1" });
            });
            services.AddHttpClient<BinanceClient>();
            services.AddHttpClient<CoinBaseProClient>();
            services.AddHttpClient<HuobiClient>();
            services.AddScoped<IExchangeDataUnifierService, ExchangeDataUnifierService>();
            services.AddAutoMapper(typeof(UnifiedTradeDataMappingProfile), typeof(UnifiedCandleStickDataMappingProfile), typeof(UnifiedOrderbookDataMappingProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoIndexSeriesCaseStudyAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
