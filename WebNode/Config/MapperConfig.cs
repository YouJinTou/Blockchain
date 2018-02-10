using AutoMapper;
using Models;
using Models.Hashing;
using Models.Validation;
using Models.Web.Nodes;
using Models.Web.Users;

namespace WebNode.Config
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            var hasher = new Sha256Hasher();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TransactionModel, Transaction>();
                cfg.CreateMap<AddPeerModel, Node>()
                    .ConstructUsing(apm => 
                        new Node(
                            apm.Name, 
                            apm.NetworkAddress, 
                            new BlockchainValidator(hasher), 
                            new TransactionValidator(hasher)));
            });
        }
    }
}
