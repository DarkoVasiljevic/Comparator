using Comparator.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Dtos
{
    public class ResultResponse
    {
        public string ResultType { get; set; }
        public IList<DiffResponse> Diffs { get; set; }
    }
}
