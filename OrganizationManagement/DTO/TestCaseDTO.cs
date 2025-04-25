namespace OrganizationManagement.DTO
{
    public class TestCaseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Steps { get; set; }
        public bool IsAutomated { get; set; }

        public int TestSuiteId { get; set; }
    }

}
