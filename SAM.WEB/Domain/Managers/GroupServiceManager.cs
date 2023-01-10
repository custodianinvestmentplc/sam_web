using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers.Helpers;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Managers
{
    public class GroupServiceManager
    {
        private readonly string _connectionString;
        
        public GroupServiceManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<GroupCustomerSearchResultDto> SearchCustomerDatabase(string searchTerm)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return GroupSynergyHelper.SearchForCustomer(db, searchTerm);
        }

        public List<GroupCustomerSearchDetailsDto> GetCustomerByFuzzyKey(int fuzzyKey)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return GroupSynergyHelper.GetCustomerByFuzzyKey(db, fuzzyKey);
        }

        public List<GroupCustomerItemDto> FetchAllCustomers()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            return GroupSynergyHelper.FetchAllCustomers(db);
        }
    }
}
