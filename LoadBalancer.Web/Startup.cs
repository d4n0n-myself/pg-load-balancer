using System.Threading.Tasks;
using LoadBalancer.Database;
using LoadBalancer.Domain;
using LoadBalancer.Models;
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
            services.AddSingleton<IStatisticsStorage, StatisticsStorage>();

            services.AddTransient<RetrieveOlapStatisticsTask>();
            services.AddTransient<RetrieveOltpStatisticsTask>();
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            ApplicationContext.Current = new ApplicationContext {Container = app.ApplicationServices};
        }
    }
}