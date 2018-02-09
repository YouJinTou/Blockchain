using AutoMapper;
using Models;
using WebNode.ApiModels.Users;

namespace WebNode.Config
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Address, string>().ConvertUsing(s => s.Id ?? string.Empty);
                cfg.CreateMap<string, Address>().ConstructUsing(s => new Address(s));
                cfg.CreateMap<TransactionModel, Transaction>();
            });
        }
    }
}
