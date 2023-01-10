using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAM.WEB.Domain.Managers.LocalServices
{
    public static class DbServer
    {
        private static readonly int _timeoutPeriod = 60 * 5;

        public static List<T> LoadData<T>(IDbConnection db, string proc, DynamicParameters prm)
        {
            try
            {
                var data = db.Query<T>(proc, param: prm ?? null, commandType: CommandType.StoredProcedure, commandTimeout: _timeoutPeriod).ToList();

                return data;
            }
            catch
            {
                throw;
            }
        }

        public static void SaveData(IDbConnection db, string proc, DynamicParameters prm)
        {
            try
            {
                db.Execute(proc, param: prm ?? null, commandType: CommandType.StoredProcedure, commandTimeout: _timeoutPeriod);
            }
            catch
            {
                throw;
            }
        }
    }
}
