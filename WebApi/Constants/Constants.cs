using System;

namespace WebApi.Constants
{
    public static class Administration
    {
        public static Guid Id = new Guid("00000000-0000-0000-0000-000000000001");
    }

    public static class Database
    {
        public static string InstallationsTableName = "[dbo].[Installations]";
        public static string Name = "Venalia";
        public static string UsersTableName = "[dbo].[Users]";
    }
}
