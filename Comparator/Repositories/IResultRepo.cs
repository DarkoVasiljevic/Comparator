using Comparator.Base;
using Comparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public interface IResultRepo
    {
        Task<IList<Result>> GetAll();
        Task<Result> GetLatestResultByLeftAndRightId(int id);
        Task<Result> GetById(int id);
        Task<(bool, int)> InsertOne(Result one);
        Task<bool> InsertMany(IList<Result> many);
        int Save();
    }
}
