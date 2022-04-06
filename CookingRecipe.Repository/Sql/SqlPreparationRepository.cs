using CookingRecipe.Core;
using CookingRecipe.Repository;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookingRecipe.Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the Preparations backend using 
    /// SQL via Entity Framework Core 2.0.
    /// </summary>
    public class SqlPreparationRepository : IPreparationRepository
    {
        private readonly CookingRecipeContext _db;

        public SqlPreparationRepository(CookingRecipeContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Preparation>> GetAsync()
        {
            return await _db.Preparations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Preparation> GetAsync(Guid id)
        {
            return await _db.Preparations
                .AsNoTracking()
                .FirstOrDefaultAsync(Preparation => Preparation.Id == id);
        }

        public async Task<Preparation> UpsertAsync(Preparation Preparation)
        {
            var current = await _db.Preparations.FirstOrDefaultAsync(_Preparation => _Preparation.Id == Preparation.Id);
            if (null == current)
            {
                _db.Preparations.Add(Preparation);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(Preparation);
            }
            await _db.SaveChangesAsync();
            return Preparation;
        }

        public async Task DeleteAsync(Guid id)
        {
            var Preparation = await _db.Preparations.FirstOrDefaultAsync(_Preparation => _Preparation.Id == id);
            if (null != Preparation)
            {
                _db.Preparations.Remove(Preparation);
                await _db.SaveChangesAsync();
            }
        }
    }
}
