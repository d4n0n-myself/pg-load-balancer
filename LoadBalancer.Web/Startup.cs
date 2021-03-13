using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Database.Statistics;
using LoadBalancer.Domain.Services;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Domain.Tasks;
using LoadBalancer.Models.System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LoadBalancer.Web
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LoadBalancer", Version = "v1"});
            });

            services.Configure<BalancerConfiguration>(Configuration);

            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
            services.AddScoped<IQueryDistributionService, QueryDistributionService>();
            
            services.AddSingleton<IStatisticsStorage, StatisticsStorage>();
            services.AddSingleton<IRequestQueue, RequestQueue>();
            services.AddSingleton<IResponseStorage, ResponseStorage>();

            services.AddTransient<RetrieveOlapStatisticsTask>(); // todo scoped ?
            services.AddTransient<RetrieveOltpStatisticsTask>(); // todo scoped ?
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var container = app.ApplicationServices;

            Task.Run(async () => await RegisterTasks(container)).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoadBalancer.Web v1");
            });

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            ApplicationContext.Current = new ApplicationContext {Container = app.ApplicationServices};
        }
    }
}