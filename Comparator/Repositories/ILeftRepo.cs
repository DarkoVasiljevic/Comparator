using Comparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public interface ILeftRepo
    {
        Task<IList<Left>> GetAll();
        Task<Left> GetById(int id);
        Task<bool> InsertOneWithCustomId(int id, Left one);
        Task<bool> InsertOne(Left one);
        Task<bool> InsertMany(IList<Left> many);
        Task<bool> UpdateOne(int id, Left one);
        int Save();
    }
}
