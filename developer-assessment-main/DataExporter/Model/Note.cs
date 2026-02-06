namespace DataExporter.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PolicyId { get; set; }

        // NEW: Navigation to Policy
        public Policy Policy { get; set; }
    }
}
