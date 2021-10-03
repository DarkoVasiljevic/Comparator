using Comparator.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Models
{
    public class Result
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public TypeOfResult ResultType { get; set; }

        public DateTime ComparationDate { get; set; }

        // Navigation properties

        public Left Left { get; set; }

        public int LeftId { get; set; }

        public Right Right { get; set; }

        public int RightId { get; set; }

        public ICollection<Diff> Diffs { get; set; }
    }
}