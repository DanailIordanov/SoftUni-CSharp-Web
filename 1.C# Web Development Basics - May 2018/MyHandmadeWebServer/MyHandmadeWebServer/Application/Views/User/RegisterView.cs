namespace MyHandmadeWebServer.Application.Views.User
{
    using Server.Contracts;

    public class RegisterView : IView
    {
        public string View()
        {
            return
                "<body>" +
                "   <form method=\"Post\">" +
                "       <span>Name: </span><input type=\"text\" name=\"name\" /><br/>" +
                "       <span>Password: </span><input type=\"password\" name=\"password\" /><br/>" +
                "       <input type=\"submit\" />" +
                "   </form>" +
                "</body>";
        }
    }
}
