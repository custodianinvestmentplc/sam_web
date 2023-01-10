using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.LocalServices;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class UserServiceHelper
    {
        public static UserRegisterDto GetUserByEmail(IDbConnection db, string email)
        {
            var sp = "dbo.GetUserByEmail";
            var prm = new DynamicParameters();

            prm.Add("@user_email", email);

            var lst = DbServer.LoadData<UserRegisterDto>(db, sp, prm)
                .SingleElseException(rowcount =>
                {
                    var msg = string.Empty;

                    if (rowcount == 0)
                    {
                        msg = $"No matching record found for the user {email}";
                    }
                    else
                    {
                        msg = $"{rowcount} matching records found for the user {email}";
                    }

                    throw new InvalidOperationException(msg);
                });

            return lst;
        }

        public static List<UserApplicationRegisterDto> GetUserApplications(IDbConnection db, string email)
        {
            var sp = "dbo.GetUserApplication";
            var prm = new DynamicParameters();

            prm.Add("@user_email", email);

            return DbServer.LoadData<UserApplicationRegisterDto>(db, sp, prm);
        }
    }
}
