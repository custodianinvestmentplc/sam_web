using SAM.WEB.Domain.Dtos;
using System.Collections.Generic;

namespace SAM.WEB.Services
{
    public interface IGroupSynergyServices
    {
        List<GroupCustomerSearchResultDto> SearchGroupCustomerDatabase(string searchTerm);
        List<GroupCustomerSearchDetailsDto> GetCustomerSearchDetailsDtos(int fuzzyKey);
        List<GroupCustomerItemDto> FetchAllCustomers();
    }
}
