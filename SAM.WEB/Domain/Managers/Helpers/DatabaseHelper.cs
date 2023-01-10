using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class DatabaseHelper
    {
        public static IDbConnection OpenDatabase(string constring)
        {
            try
            {
                var db = new SqlConnection(constring);
                db.Open();

                return db;
            }
            catch
            {
                throw;
            }
        }

        public static T SingleElseException<T>(this IEnumerable<T> source, Action<int> rowcountDelegate)
        {
            T result = default(T);
            var count = 0;
            bool firstElement = true;

            foreach (T element in source)
            {
                if (firstElement)
                {
                    result = element;
                    firstElement = false;
                }

                count++;
            }

            if (count != 1)
            {
                rowcountDelegate(count);
            }

            return result;
        }
    }
}
