namespace SAM.WEB.Domain.Dtos.Cts
{
    public class TicketCategorization
    {
        public int RequestId { get; set; }
        public string TicketNumber { get; set; }
        public int ServiceTypeId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubCategoryItemId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ServiceTypeName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryItemName { get; set; }
    }
}
