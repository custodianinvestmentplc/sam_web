using SAM.WEB.Domain.Dtos;
using SAM.WEB.Domain.Managers;
using SAM.WEB.Services;
using System.Collections.Generic;

namespace SAM.WEB.Domain
{
    public class GroupSynergyServiceFacade : IGroupSynergyServices
    {
        private readonly GroupServiceManager _groupManager;
        public GroupSynergyServiceFacade(string connectionstring)
        {
            _groupManager = new GroupServiceManager(connectionstring);
        }

        public List<GroupCustomerSearchDetailsDto> GetCustomerSearchDetailsDtos(int fuzzyKey)
        {
            return _groupManager.GetCustomerByFuzzyKey(fuzzyKey);
        }

        public List<GroupCustomerSearchResultDto> SearchGroupCustomerDatabase(string searchTerm)
        {
            return _groupManager.SearchCustomerDatabase(searchTerm);
        }

        public List<GroupCustomerItemDto> FetchAllCustomers()
        {
            return _groupManager.FetchAllCustomers();
        }
    }
}
