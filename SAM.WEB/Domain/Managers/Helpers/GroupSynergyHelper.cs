using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.LocalServices;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class GroupSynergyHelper
    {
        public static List<GroupCustomerSearchResultDto> SearchForCustomer(IDbConnection db, string searchTerm)
        {
            var sp = "grp.searcg_group_customer_db";
            var prm = new DynamicParameters();

            prm.Add("@search_term", searchTerm);
            var lst = DbServer.LoadData<GroupCustomerSearchResultDto>(db, sp, prm);

            return lst;
        }

        public static List<GroupCustomerSearchDetailsDto> GetCustomerByFuzzyKey(IDbConnection db, int fuzzykey)
        {
            var sp = "grp.get_customer_by_fuzzy_key";
            var prm = new DynamicParameters();

            prm.Add("@key_out", fuzzykey);
            var lst = DbServer.LoadData<GroupCustomerSearchDetailsDto>(db, sp, prm);

            return lst;
        }

        public static List<GroupCustomerItemDto> FetchAllCustomers(IDbConnection db)
        {
            var sp = "grp.get_all_customers";
            var lst = DbServer.LoadData<GroupCustomerItemDto>(db, sp, null);

            return lst;
        }
    }
}
