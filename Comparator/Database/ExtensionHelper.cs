using Comparator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Database
{
    public static class ExtensionHelper
    {  
        public static async Task<bool> EnableIdentityInsertAsync<T>(this DbContext context) => await SetIdentityInsertAsync<T>(context, true);
        public static async Task<bool> DisableIdentityInsertAsync<T>(this DbContext context) => await SetIdentityInsertAsync<T>(context, false);

        private static async Task<bool> SetIdentityInsertAsync<T>([NotNull] DbContext context, bool enable)
        {
            if (context == null) return false;

            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");

            return true;
        }

        public static async Task<bool> SaveChangesWithIdentityInsertAsync<T>([NotNull] this DbContext context)
        {
            if (context == null) return false;

            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.EnableIdentityInsertAsync<T>();
            var success = 0 != await context.SaveChangesAsync();
            await context.EnableIdentityInsertAsync<T>();
            await transaction.CommitAsync();

            return success;
        }
    }
}
