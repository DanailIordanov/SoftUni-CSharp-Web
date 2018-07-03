namespace MyHandmadeWebServer.Application.Views.User
{
    using Server.Contracts;

    class DetailsView : IView
    {
        private readonly Model model;

        public DetailsView(Model model)
        {
            this.model = model;
        }

        public string View()
        {
            return 
                "<body>" +
                $"   <div>Hello, {model["name"]}!</div>" +
                "</body>";
        }
    }
}
