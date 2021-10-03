using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Models
{
    public class Right
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Data { get; set; }

        public DateTime ModifiedDate { get; set; }

        // Navigation properties

        public ICollection<Result> Results { get; set; }
    }
}
