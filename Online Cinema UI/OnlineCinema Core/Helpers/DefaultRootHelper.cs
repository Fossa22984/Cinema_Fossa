namespace OnlineCinema_Core.Helpers
{
    public class DefaultRootHelper
    {
        private static readonly DefaultRootHelper _instance = new DefaultRootHelper();
        public static DefaultRootHelper Current => _instance;

        public string DefaultIconPath { get; private set; } = @".\wwwroot\Images\background-fon.jpg";
    }
}
