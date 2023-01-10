using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.Helpers;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Managers
{
    public class UserServiceManager
    {
        private readonly string _connectionString;

        public UserServiceManager(string constring)
        {
            _connectionString = constring;
        }

        public List<UserApplicationRegisterDto> GetApplicationAssignedToUser(string userEmail)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var model = UserServiceHelper.GetUserApplications(db, userEmail);

            return model;
        }

        public UserRegisterDto GetUserByEmail(string userEmail)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var user = UserServiceHelper.GetUserByEmail(db, userEmail);

            return user;
        }
    }
}
