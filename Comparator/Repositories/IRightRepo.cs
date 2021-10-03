using Comparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public interface IRightRepo
    {
        Task<IList<Right>> GetAll();
        Task<Right> GetById(int id);
        Task<bool> InsertOne(Right one);
        Task<bool> InsertOneWithCustomId(int id, Right one);
        Task<bool> InsertMany(IList<Right> many);
        Task<bool> UpdateOne(int id, Right one);
        int Save();
    }
}
