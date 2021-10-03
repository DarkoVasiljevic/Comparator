using Comparator.Database;
using Comparator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public class DiffRepo : IDiffRepo
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Diff> _dbTable;

        public DiffRepo(DBContext dbContext)
        {
            _dbContext = dbContext;
            _dbTable = _dbContext.Diffs;
        }

        public async Task<bool> InsertMany(IList<Diff> many)
        {
            await _dbTable.AddRangeAsync(many);
            if (0 == Save()) return false;

            return true;
        }

        public async Task<bool> InsertOne(Diff one)
        {
            await _dbTable.AddAsync(one);
            if (0 == Save()) return false;

            return true;
        }

        public int Save() => _dbContext.SaveChanges();
    }
}
