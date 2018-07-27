namespace MyHandmadeWebServer.ByTheCake.Services.Contracts
{
    using ViewModels.Account;

    public interface IUserService
    {
        bool Create(string username, string password);

        bool Find(string username, string password);

        int? GetUserId(string username);

        ProfileViewModel Profile(string username);
    }
}
