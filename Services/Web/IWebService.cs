namespace Services.Web
{
    public interface IWebService
    {
        T GetDeserialized<T>(string endpoint) where T : class;

        string SendSerialized<T>(string endpoint, T obj) where T : class;
    }
}
