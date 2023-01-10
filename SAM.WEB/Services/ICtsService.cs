using SAM.WEB.Domain.Dtos.Cts;
using SAM.WEB.Domain.RequestModels;
using System.Collections.Generic;

namespace SAM.WEB.Services
{
    public interface ICtsService
    {
        List<NewCtsTicket> FetchNewCtsTickets();
        List<ServiceType> FetchServiceTypes();
        List<ServiceTypeCategory> FetchCategoryByServiceTypeId(int serviceId);
        List<ServiceTypeSubCategory> FetchSubCategoryByCategoryId(int categoryId);
        List<ServiceTypeSubCategoryItem> FetchSubCategoryItemBySubCategoryId(int subcategoryId);
        List<TechnicianUser> FetchAllTechnicians();
        List<Companies> FetchAllCompanies();
        TicketDetail GetTicketByTicketNumber(string ticketNumber);
        void AssignNewTicket(AssignTicketRequest request);
        TicketCategorization GetTicketCategorization(string ticketNumber);
        List<NewCtsTicket> FindTicketByTitle(string searchTerm);
        List<ActivityLog> GetTicketActivityLog(string ticketNumber);
        CategoryDetailDto GetCategoryDetails(int categoryId);
        SubCategoryDetailDto GetSubCategoryDetails(int subcategoryId);
        ServiceTypeDetailDto GetServiceTypeDetails(int servicetypeId);
    }
}
