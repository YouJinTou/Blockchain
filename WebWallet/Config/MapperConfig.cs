using AutoMapper;
using Services.Wallets;
using WebWallet.Models;

namespace WebNode.Config
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<WalletCredentials, CreateWalletViewModel>();
            });
        }
    }
}
