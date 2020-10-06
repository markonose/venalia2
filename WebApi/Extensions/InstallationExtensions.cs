using WebApi.Entities;
using WebApi.Responses.Installation;

namespace WebApi.Extensions
{
    public static class InstallationExtensions
    {
        public static void RemovePersonalData(this Installation installation)
        {
            installation.CustomerFirstName = "";
            installation.CustomerLastName = "";
            installation.CustomerEmail = "";
            installation.CustomerPhoneNumber = "";
            installation.StreetNumber = "";
        }
    }
}
