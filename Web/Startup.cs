using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Hashing;
using Models.Mining;
using Models.Validation;
using Models.Web.Settings;
using Services.Cryptography;
using Services.Faucet;
using Services.Generation;
using Services.Nodes;
using Services.Wallets;
using Web.Config;

namespace Web
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
            services.AddTransient<IConsensusStrategy, ProofOfWork>();
            services.AddTransient<IBlockGenerator, GenesisBlockGenerator>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ITransactionSecurityService, TransactionSecurityService>();
            services.AddTransient<IMessageSignerVerifier, MessageSignerVerifier>();
            services.AddTransient<IFaucetService, FaucetService>();

            services.Configure<NodeSettings>(this.Configuration.GetSection("Node"));
            services.Configure<FaucetSettings>(this.Configuration.GetSection("Faucet"));

            MapperConfig.RegisterMappings();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "walletsRoute",
                    template: "Wallets/{controller=Wallets}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "nodeRoute",
                    template: "Node/{controller=Base}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "faucetRoute",
                    template: "Faucet/{controller=Faucet}/{action=Index}/{id?}");
            });
        }
    }
}
