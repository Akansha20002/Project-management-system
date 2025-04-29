namespace OrganizationManagement.DTO
{
    public class TestStepDTO
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Action { get; set; }
        public string ExpectedResult { get; set; }
        public string ActualResult { get; set; }

        public int TestCaseId { get; set; }
    }
}
