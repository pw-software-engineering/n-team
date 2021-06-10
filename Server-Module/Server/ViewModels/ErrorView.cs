
namespace Server.ViewModels
{
    public class ErrorView
    {
        public string Error { get; }
        public ErrorView(string error)
        {
            Error = error;
        }
    }
}
