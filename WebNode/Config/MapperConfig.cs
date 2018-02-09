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
                cfg.CreateMap<TransactionModel, Transaction>();
            });
        }
    }
}
