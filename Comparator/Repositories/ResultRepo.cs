using Comparator.Base;
using Comparator.Database;
using Comparator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public class ResultRepo : IResultRepo
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Result> _dbTable;

        public ResultRepo(DBContext dbContext)
        {
            _dbContext = dbContext;
            _dbTable = _dbContext.Results;
        }

        public async Task<IList<Result>> GetAll() => await _dbTable.ToListAsync();

        public async Task<Result> GetLatestResultByLeftAndRightId(int id)
        {
		    return await _dbTable
                .AsQueryable()
                .Where(e => e.LeftId == id && e.RightId == id)
                .Include(d => d.Diffs)
                .OrderByDescending(d => d.ComparationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Result> GetById(int id) => await _dbTable.FindAsync(id);

        public async Task<(bool, int)> InsertOne(Result one)
        {
            var result = await _dbTable.AddAsync(one);
            if (0 == Save()) return (false, -1);

            return (true, result.Entity.Id);
        }

        public async Task<bool> InsertMany(IList<Result> many)
        {
            await _dbTable.AddRangeAsync(many);
            if (0 == Save()) return false;

            return true;
        }

        public int Save() => _dbContext.SaveChanges();
    }
}
