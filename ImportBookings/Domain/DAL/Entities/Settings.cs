using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ImportBookings.Domain.DAL.Entities
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
