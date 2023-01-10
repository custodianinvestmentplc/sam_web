using SAM.WEB.Domain.Dtos.Cts;
using System.Collections.Generic;
using Dapper;
using SAM.WEB.Domain.Managers.LocalServices;
using System.Data;
using System;
using SAM.WEB.Domain.RequestModels;

namespace SAM.WEB.Domain.Managers.Helpers
{
    public static class CtsHelper
    {
        public static List<NewCtsTicket> FetchNewTickets(IDbConnection db)
        {
            var sp = "dbo.Rds_get_new_tickets";
            var lst = DbServer.LoadData<NewCtsTicket>(db, sp, null);

            return lst;
        }

        public static List<ServiceType> FetchServiceTypes(IDbConnection db)
        {
            var sp = "dbo.Rds_get_services";
            var lst = DbServer.LoadData<ServiceType>(db, sp, null);

            return lst;
        }

        public static List<ServiceTypeCategory> FetchCategoryByServiceTypeId(IDbConnection db, int serviceId)
        {
            var sp = "dbo.Rds_get_category_by_service_type_id";
            var prm = new DynamicParameters();

            prm.Add("@service_id", serviceId);

            var lst = DbServer.LoadData<ServiceTypeCategory>(db, sp, prm);

            return lst;
        }

        public static List<ServiceTypeSubCategory> FetchSubCategoryByCategoryId(IDbConnection db, int categoryId)
        {
            var sp = "dbo.Rds_get_sub_category_by_category_id";
            var prm = new DynamicParameters();

            prm.Add("@category_id", categoryId);

            var lst = DbServer.LoadData<ServiceTypeSubCategory>(db, sp, prm);

            return lst;
        }

        public static List<ServiceTypeSubCategoryItem> FetchSubCategoryItemBySubCategoryId(IDbConnection db, int subcategoryId)
        {
            var sp = "dbo.Rds_get_sub_category_item_by_sub_category_id";
            var prm = new DynamicParameters();

            prm.Add("@sub_category_id", subcategoryId);

            var lst = DbServer.LoadData<ServiceTypeSubCategoryItem>(db, sp, prm);

            return lst;
        }

        public static List<TechnicianUser> FetchAllTechnicians(IDbConnection db)
        {
            var sp = "dbo.Rds_get_technicians";

            var lst = DbServer.LoadData<TechnicianUser>(db, sp, null);

            return lst;
        }

        public static List<Companies> FetchAllCompanies(IDbConnection db)
        {
            var sp = "dbo.Rds_get_companies";

            var lst = DbServer.LoadData<Companies>(db, sp, null);

            return lst;
        }

        public static TicketDetail GetTicketByTicketNumber(IDbConnection db, string ticketNumber)
        {
            var sp = "dbo.Rds_get_ticket_by_ticket_number";
            var prm = new DynamicParameters();

            prm.Add("@ticket_number", ticketNumber);

            var lst = DbServer.LoadData<TicketDetail>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            throw new InvalidOperationException($"Cannot find Ticket with Number { ticketNumber }");
        }

        public static void CategoriseTicket(IDbConnection db, AssignTicketRequest request)
        {
            var sp = "dbo.ADD_UPDATE_REQUEST_CATEGORY";
            var param = new DynamicParameters();

            param.Add("@SUB_CATEGORY_ID", request.SubcategoryId);
            param.Add("@CATEGORY_ID", request.CategoryId);
            param.Add("@SUB_CATEGORY_ITEM_ID", request.SubcategoryItemId);
            param.Add("@SEVERITY_ID", request.SeverityId);
            param.Add("@REQUEST_ID", request.TicketRequestId);
            param.Add("@COMPANY_ID", request.CompanyId);
            param.Add("@USER_ID", request.UserId);

            DbServer.SaveData(db, sp, param);
        }

        public static void AllocateTicket(IDbConnection db, AssignTicketRequest request)
        {
            var sp = "dbo.ASSIGN_USER_TO_TICKET";
            var param = new DynamicParameters();

            param.Add("@REQUEST_ID", request.TicketRequestId);
            param.Add("@USER_ID", request.TechnicianId);
            param.Add("@STATUS", "ALLOCATED");

            DbServer.SaveData(db, sp, param);
        }
    
        public static TicketCategorization GetTicketCategorization(IDbConnection db, string ticketNumber)
        {
            var sp = "dbo.Rds_get_ticket_categorization";
            var prm = new DynamicParameters();

            prm.Add("@ticket_number", ticketNumber);

            var lst = DbServer.LoadData<TicketCategorization>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static List<NewCtsTicket> FindTicketByTitle(IDbConnection db, string searchTerm)
        {
            var sp = "dbo.Rds_find_ticket_by_title";
            var prm = new DynamicParameters();

            prm.Add("@search_term", searchTerm);

            return DbServer.LoadData<NewCtsTicket>(db, sp, prm);
        }

        public static List<ActivityLog> GetTicketActivityLog(IDbConnection db, string ticketNumber)
        {
            var sp = "dbo.Rds_get_ticket_activity_log";
            var prm = new DynamicParameters();

            prm.Add("@ticket_number", ticketNumber);

            return DbServer.LoadData<ActivityLog>(db, sp, prm);
        }

        public static CategoryDetailDto GetCategoryDetails(IDbConnection db, int categoryId)
        {
            var sp = "dbo.Rds_get_category_detail";
            var prm = new DynamicParameters();

            prm.Add("@category_id", categoryId);

            var lst = DbServer.LoadData<CategoryDetailDto>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }

        public static SubCategoryDetailDto GetSubCategoryDetails(IDbConnection db, int subcategoryId)
        {
            var sp = "dbo.Rds_get_subcategory_detail";
            var prm = new DynamicParameters();

            prm.Add("@subcategory_id", subcategoryId);

            var lst = DbServer.LoadData<SubCategoryDetailDto>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }
        
        public static ServiceTypeDetailDto GetServiceTypeDetails(IDbConnection db, int servicetypeId)
        {
            var sp = "dbo.Rds_get_service_type_detail";
            var prm = new DynamicParameters();

            prm.Add("@service_type_id", servicetypeId);

            var lst = DbServer.LoadData<ServiceTypeDetailDto>(db, sp, prm);

            if (lst != null && lst.Count == 1)
            {
                return lst[0];
            }

            return null;
        }
    }
}
