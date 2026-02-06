using System.ComponentModel.DataAnnotations.Schema;

namespace DataExporter.Model
{
    public class Policy
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }

        // NEW: One Policy has many Notes
        public List<Note> Notes { get; set; } = new();
    }
}
