using WebNode.ApiModels.Users;

namespace WebNode.Code
{
    public interface INodeService
    {
        void RegisterAddress(RegisterUserModel model);
    }
}
