using AutoMapper;
using Models;
using Models.Hashing;
using Models.Validation;
using Models.Web.Faucet;
using Models.Web.Nodes;
using Models.Web.Wallets;
using Services.Wallets;
using Web.Areas.Faucet.Models;
using Web.Areas.Wallets.Models;

namespace Web.Config
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            var hasher = new Sha256Hasher();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Node, Node>();
                cfg.CreateMap<SendTransactionViewModel, SendTransactionModel>();
                cfg.CreateMap<WalletCredentials, CreateWalletViewModel>();
                cfg.CreateMap<Transaction, TransactionViewModel>();
                cfg.CreateMap<AddressHistory, AddressHistoryViewModel>();
                cfg.CreateMap<FaucetSendViewModel, FaucetSendModel>();
                cfg.CreateMap<AddressHistory, SearchAddressViewModel>()
                    .ForMember(s => s.AddressHistory, d => d.MapFrom(model => model));
                cfg.CreateMap<SendTransactionModel, Transaction>()
                    .ConstructUsing(tm =>
                        new Transaction(tm.From, tm.To, tm.Amount, Wallet.GetPublicKey(tm.PrivateKey), null));
                cfg.CreateMap<FaucetSendModel, Transaction>()
                    .ConstructUsing(fm =>
                        new Transaction(fm.From, fm.To, fm.Amount, Wallet.GetPublicKey(fm.PrivateKey), null));
                cfg.CreateMap<AddPeerModel, Node>()
                    .ConstructUsing(apm =>
                        new Node(
                            apm.Name,
                            apm.NetworkAddress,
                            new BlockchainValidator(hasher),
                            new TransactionValidator(hasher, new MessageSignerVerifier())));
            });
        }
    }
}
