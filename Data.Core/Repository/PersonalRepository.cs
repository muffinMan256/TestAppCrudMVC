using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Core.Infrastructure;
using Data.Core.IRepository;
using Data.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Core.Repository
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly TestAppDbContext _db;
        public PersonalRepository(TestAppDbContext db)
        {
            _db = db;
        }

       public IQueryable<Personal> FindAllAsync()
        {

            return  _db.Personals.Select(a=>a);
        }

        public async Task<Personal?> FindByIdAsync(long id)
        {
            return await _db.Personals.FindAsync(id);
        }

        public async Task<Personal?> FindByIdAsync(Guid id)
        {
            return await _db.Personals.FirstOrDefaultAsync(a => a != null && a.UserId == id);
        }

        public async Task<Personal> AddAsync(Personal personal)
        {
           await _db.Personals.AddAsync(personal);
           await _db.SaveChangesAsync();
           return personal;

        }

        public async Task<Personal> UpdateAsync(Personal personal)
        {
            var entity = _db.Personals.Attach(personal);
            entity.State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return personal;

        }

        public async Task<Personal> DeleteAsync(Personal personal)
        {
            var entity = _db.Personals.Attach(personal);
            entity.State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return personal;
        }

        public async Task<IEnumerable<Personal>> GetManyAsync<TOrder>(Expression<Func<Personal, bool>> where, Expression<Func<Personal, TOrder>> order, SortType type)
        {
            if (type == SortType.Asc)
            {
                return await _db.Personals.Where(where).OrderBy(order).ToListAsync();
            }

            return await _db.Personals.Where(where).OrderByDescending(order).ToListAsync();

        }
    }
}
