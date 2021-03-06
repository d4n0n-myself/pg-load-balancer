using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Database.Statistics;
using LoadBalancer.Domain.Decision;
using LoadBalancer.Domain.Distribution;
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
    /// <summary>
    /// Default web startup class.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Configure services.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LoadBalancer", Version = "v1"});
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<BalancerConfiguration>(Configuration);

            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();

            services.AddScoped<IServerDecider, ServerDecider>();
            services.AddScoped<IQueryDistributionService, QueryDistributionService>();
            
            services.AddSingleton<IStatisticsStorage, StatisticsStorage>();
            services.AddSingleton<IRequestQueue, RequestQueue>();
            services.AddSingleton<IResponseStorage, ResponseStorage>();

            services.AddTransient<RetrieveOlapStatisticsTask>();
            services.AddTransient<RetrieveOltpStatisticsTask>();
        }
        
        /// <summary>
        /// Configure.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
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

            ApplicationContext.Container = app.ApplicationServices;
        }
    }
}