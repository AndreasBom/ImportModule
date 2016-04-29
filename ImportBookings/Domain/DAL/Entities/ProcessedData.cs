using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportBookings.Domain.DAL.Entities

{
    [Table("ProcessedData")]
    public class ProcessedData
    {
        [Key]
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
