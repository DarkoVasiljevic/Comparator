using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Models
{
    public class Diff
    {
        [Required]
        public int Id { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }

        // Navigation properties

        public Result Result { get; set; }

        public int ResultId { get; set; }
    }
}
