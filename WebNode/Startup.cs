using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Hashing;
using Models.Validation;
using WebNode.Code;
using WebNode.Config;

namespace WebNode
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<INodeService, NodeService>();
            services.AddTransient<IHasher, Sha256Hasher>();
            services.AddTransient<IBlockchainValidator, BlockchainValidator>();
            services.AddTransient<ITransactionValidator, TransactionValidator>();

            MapperConfig.RegisterMappings();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}/{id?}");
            });
        }
    }
}
