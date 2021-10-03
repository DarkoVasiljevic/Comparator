using Comparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public interface IDiffRepo
    {
        Task<bool> InsertOne(Diff one);
        Task<bool> InsertMany(IList<Diff> many);
        int Save();
    }
}
