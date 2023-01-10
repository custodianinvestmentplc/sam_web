namespace SAM.WEB.Domain.Dtos
{
    public class GroupCustomerSearchDetailsDto
    {
        public int FuzzyKey { get; set; }
        public string CustomerKey { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSegment { get; set; } 
        public decimal IncomeBracket { get; set; }
        public decimal PremiumContribution { get; set; }
        public string RelationshipManagerName { get; set; }
        public string RelationshipManagerPhone { get; set; }
        public string IntermediaryType { get; set; }
        public string IntermediaryName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string SubsidiaryCode { get; set; }
        public string CustomerType { get; set; }
    }
}
