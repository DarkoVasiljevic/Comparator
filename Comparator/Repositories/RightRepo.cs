using Comparator.Database;
using Comparator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Comparator.Repositories
{
    public class RightRepo : IRightRepo
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Right> _dbTable;

        public RightRepo(DBContext dbContext)
        {
            _dbContext = dbContext;
            _dbTable = _dbContext.Rights;
        }

        public async Task<IList<Right>> GetAll() => await _dbTable.ToListAsync();

        public async Task<Right> GetById(int id) => await _dbTable.FindAsync(id);

        public async Task<bool> InsertOneWithCustomId(int id, Right one)
        {
            one.Id = id;

            var success = await ExtensionHelper.EnableIdentityInsertAsync<Right>(_dbContext);
            if (success)
            {
                await _dbTable.AddAsync(one);
                var result = await ExtensionHelper.SaveChangesWithIdentityInsertAsync<Right>(_dbContext) &&
                             await ExtensionHelper.DisableIdentityInsertAsync<Right>(_dbContext);

                return result;
            }

            return false;
        }

        public async Task<bool> InsertOne(Right one)
        {
            await _dbTable.AddAsync(one);
            if (0 == Save()) return false;

            return true;
        }

        public async Task<bool> InsertMany(IList<Right> many)
        {
            await _dbTable.AddRangeAsync(many);
            if (0 == Save()) return false;

            return true;
        }

        public async Task<bool> UpdateOne(int id, Right one)
        {
            Right exist = await _dbTable.FindAsync(id);
            if (null == exist) return false;

            _ = UpdateObjectProperties(exist, one);
            if (0 == Save()) return false;

            return true;
        }

        public int Save() => _dbContext.SaveChanges();

        private U UpdateObjectProperties<U>(U dest, U src)
        {
            PropertyInfo[] properties = src.GetType().GetProperties();

            var notNullProperties = properties.ToList().FindAll(p => p.GetValue(src) != null);

            List<string> propertyNames = notNullProperties.Select(p => p.Name).Skip(1).ToList();

            propertyNames.ToList().ForEach(p => Debug.WriteLine($"property: {p}" + "\n"));

            properties.Where(p => propertyNames.Contains(p.Name)).ToList()
                .ForEach(p => p.SetValue(dest, p.GetValue(src, null)));

            return dest;
        }
    }
}
