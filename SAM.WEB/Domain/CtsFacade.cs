using SAM.WEB.Domain.Dtos.Cts;
using SAM.WEB.Domain.Managers;
using SAM.WEB.Domain.RequestModels;
using SAM.WEB.Services;
using System.Collections.Generic;

namespace SAM.WEB.Domain
{
    public class CtsFacade : ICtsService
    {
        private readonly CtsManager _ctsManager;

        public CtsFacade(string connstring)
        {
            _ctsManager = new CtsManager(connstring);
        }

        public List<NewCtsTicket> FetchNewCtsTickets()
        {
            return _ctsManager.FetchNew();
        }

        public List<ServiceType> FetchServiceTypes()
        {
            return _ctsManager.FetchServiceTypes();
        }

        public List<ServiceTypeCategory> FetchCategoryByServiceTypeId(int serviceId)
        {
            return _ctsManager.FetchCategoryByServiceTypeId(serviceId);
        }

        public List<ServiceTypeSubCategory> FetchSubCategoryByCategoryId(int categoryId)
        {
            return _ctsManager.FetchSubCategoryByCategoryId(categoryId);
        }

        public List<ServiceTypeSubCategoryItem> FetchSubCategoryItemBySubCategoryId(int subcategoryId)
        {
            return _ctsManager.FetchSubCategoryItemBySubCategoryId(subcategoryId);
        }

        public List<TechnicianUser> FetchAllTechnicians()
        {
            return _ctsManager.FetchAllTechnicians();
        }

        public List<Companies> FetchAllCompanies()
        {
            return _ctsManager.FetchAllCompanies();
        }

        public TicketDetail GetTicketByTicketNumber(string ticketNumber)
        {
            return _ctsManager.GetTicketByTicketNumber(ticketNumber);
        }

        public void AssignNewTicket(AssignTicketRequest request)
        {
            _ctsManager.AssignNewTicket(request);
        }

        public TicketCategorization GetTicketCategorization(string ticketNumber)
        {
            return _ctsManager.GetTicketCategorization(ticketNumber);
        }

        public List<NewCtsTicket> FindTicketByTitle(string searchTerm)
        {
            return _ctsManager.FindTicketByTitle(searchTerm);
        }

        public List<ActivityLog> GetTicketActivityLog(string ticketNumber)
        {
            return _ctsManager.GetTicketActivityLog(ticketNumber);
        }

        public CategoryDetailDto GetCategoryDetails(int categoryId)
        {
            return _ctsManager.GetCategoryDetails(categoryId);
        }

        public SubCategoryDetailDto GetSubCategoryDetails(int subcategoryId)
        {
            return _ctsManager.GetSubCategoryDetails(subcategoryId);
        }

        public ServiceTypeDetailDto GetServiceTypeDetails(int servicetypeId)
        {
            return _ctsManager.GetServiceTypeDetails(servicetypeId);
        }
    }
}
