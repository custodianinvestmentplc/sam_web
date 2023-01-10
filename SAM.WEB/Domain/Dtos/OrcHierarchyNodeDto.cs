namespace SAM.WEB.Domain.Dtos
{
    public class OrcHierarchyNodeDto
    {
        public string AgentBusinessCode { get; set; }
        public string OrcNodeBusinessCode { get; set; }
        public string OrcNodeFullName { get; set; }
        public string OrcNodeType { get; set; }
        public string ApplyCommission { get; set; }
        public double ComRateTraditional { get; set; }
        public double ComRateAnnuity { get; set; }
    }
}
