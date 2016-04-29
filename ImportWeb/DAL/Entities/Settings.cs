using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImportWeb.DAL.Entities
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}