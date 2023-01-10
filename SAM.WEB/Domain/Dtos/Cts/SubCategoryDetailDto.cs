namespace SAM.WEB.Domain.Dtos.Cts
{
    public class SubCategoryDetailDto
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int SubCategoryStatusCode { get; set; }
        public string SubCategoryStatusCodeName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryStatusCode { get; set; }
        public string CategoryStatusCodeName { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
    }
}
