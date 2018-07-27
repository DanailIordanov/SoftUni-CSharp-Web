namespace MyHandmadeWebServer.ByTheCake.Controllers
{
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using Services.Contracts;
    using ViewModels.Shopping;
    using ViewModels.Account;

    using System;

    public class AccountController : Controller
    {
        private const string RegisterView = @"Account\register";
        private const string LoginView = @"Account\login";
        private const string ProfileView = @"Account\profile";

        private readonly IUserService users;

        public AccountController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(RegisterView);
        }

        public IHttpResponse Register(IHttpRequest request, RegisterViewModel viewModel)
        {
            this.SetDefaultViewData();

            if (string.IsNullOrWhiteSpace(viewModel.Username) || string.IsNullOrWhiteSpace(viewModel.Password))
            {
                this.AddError("Username and password fields cannot be empty.");

                return this.FileViewResponse(RegisterView);
            }

            if (viewModel.Username.Length < 4 || viewModel.Username.Length > 20)
            {
                this.AddError("Username should be between 3 and 20 symbols.");

                return this.FileViewResponse(RegisterView);
            }

            if (viewModel.Password.Length < 4)
            {
                this.AddError("Password should be at least 4 symbols.");

                return this.FileViewResponse(RegisterView);
            }
            else if (viewModel.Password.Length > 100)
            {
                this.AddError("Password is too long.");

                return this.FileViewResponse(RegisterView);
            }

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                this.AddError("Passwords don't match.");

                return this.FileViewResponse(RegisterView);
            }

            var success = this.users.Create(viewModel.Username, viewModel.Password);

            if (success)
            {
                this.LoginUser(request, viewModel.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.AddError("There is another user with this username");

                return this.FileViewResponse(RegisterView);
            }
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(LoginView);
        }

        public IHttpResponse Login(IHttpRequest request, LoginViewModel viewModel)
        {
            this.SetDefaultViewData();

            if (string.IsNullOrWhiteSpace(viewModel.Username) || string.IsNullOrWhiteSpace(viewModel.Password))
            {
                this.AddError("Username and password fields cannot be empty.");

                return this.FileViewResponse(LoginView);
            }

            var success = this.users.Find(viewModel.Username, viewModel.Password);

            if (success)
            {
                this.LoginUser(request, viewModel.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.AddError("Invalid username or password.");

                return this.FileViewResponse(LoginView);
            }
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var userModel = this.users.Profile(username);

            if (userModel == null)
            {
                throw new InvalidOperationException($"The user: {username} could not be found in the database.");
            }

            this.ViewData["username"] = userModel.Username;
            this.ViewData["registrationDate"] = userModel.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = userModel.TotalOrders.ToString();

            return this.FileViewResponse(ProfileView);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData() => this.ViewData["authDisplay"] = "none";

        private void LoginUser(IHttpRequest request, string username)
        {
            request.Session.Add(SessionStore.CurrentUserKey, username);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}
