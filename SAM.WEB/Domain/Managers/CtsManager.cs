using SAM.WEB.Domain.Dtos.Cts;
using SAM.WEB.Domain.Managers.Helpers;
using SAM.WEB.Domain.RequestModels;
using System.Collections.Generic;

namespace SAM.WEB.Domain.Managers
{
    public class CtsManager
    {
        private readonly string _connectionString;

        public CtsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<NewCtsTicket> FetchNew()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchNewTickets(db);

            return lst;
        }

        public List<ServiceType> FetchServiceTypes()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchServiceTypes(db);

            return lst;
        }

        public List<ServiceTypeCategory> FetchCategoryByServiceTypeId(int serviceId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchCategoryByServiceTypeId(db, serviceId);

            return lst;
        }

        public List<ServiceTypeSubCategory> FetchSubCategoryByCategoryId(int categoryId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchSubCategoryByCategoryId(db, categoryId);

            return lst;
        }

        public List<ServiceTypeSubCategoryItem> FetchSubCategoryItemBySubCategoryId(int subcategoryId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchSubCategoryItemBySubCategoryId(db, subcategoryId);

            return lst;
        }

        public List<TechnicianUser> FetchAllTechnicians()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchAllTechnicians(db);

            return lst;
        }

        public List<Companies> FetchAllCompanies()
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.FetchAllCompanies(db);

            return lst;
        }

        public TicketDetail GetTicketByTicketNumber(string ticketNumber)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            var lst = CtsHelper.GetTicketByTicketNumber(db, ticketNumber);

            return lst;
        }

        public void AssignNewTicket(AssignTicketRequest request)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            CtsHelper.CategoriseTicket(db, request);
            CtsHelper.AllocateTicket(db, request);
        }

        public TicketCategorization GetTicketCategorization(string ticketNumber)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var data = CtsHelper.GetTicketCategorization(db, ticketNumber);

            return data;
        }

        public List<NewCtsTicket> FindTicketByTitle(string searchTerm)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var data = CtsHelper.FindTicketByTitle(db, searchTerm);

            return data;
        }

        public List<ActivityLog> GetTicketActivityLog(string ticketNumber)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);
            var data = CtsHelper.GetTicketActivityLog(db, ticketNumber);

            return data;
        }

        public CategoryDetailDto GetCategoryDetails(int categoryId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CtsHelper.GetCategoryDetails(db, categoryId);
        }

        public SubCategoryDetailDto GetSubCategoryDetails(int subcategoryId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CtsHelper.GetSubCategoryDetails(db, subcategoryId);
        }

        public ServiceTypeDetailDto GetServiceTypeDetails(int servicetypeId)
        {
            using var db = DatabaseHelper.OpenDatabase(_connectionString);

            return CtsHelper.GetServiceTypeDetails(db, servicetypeId);
        }
    }
}
