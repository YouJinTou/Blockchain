﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Hashing;
using Models.Validation;
using Services.Generation;
using Services.Nodes;
using Services.Wallets;
using WebNode.Config;

namespace WebWallet
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
            services.AddTransient<IHasher, Sha256Hasher>();
            services.AddTransient<IBlockchainValidator, BlockchainValidator>();
            services.AddTransient<ITransactionValidator, TransactionValidator>();
            services.AddTransient<IBlockGenerator, GenesisBlockGenerator>();
            services.AddTransient<INodeService, NodeService>();
            services.AddTransient<IWalletService, WalletService>();

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
                routes.MapRoute("default", "{controller}/{action}/{id?}", 
                    new { controller = "Wallet", action = "Index" });
            });
        }
    }
}