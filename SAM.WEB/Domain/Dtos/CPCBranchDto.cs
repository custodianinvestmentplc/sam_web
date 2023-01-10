namespace SAM.WEB.Domain.Dtos
{
    public class CPCBranchDto
    {
        public int RowId { get; set; }
        public string SourceSystemCode { get; set; }
        public string LocalSystemCode { get; set; }
        public string BranchName { get; set; }
        public string BranchEmailAddress { get; set; }
    }
}