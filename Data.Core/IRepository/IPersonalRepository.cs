using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Core.Infrastructure;
using Data.Core.Model;

namespace Data.Core.IRepository
{
    public interface IPersonalRepository
    {
        Task<IEnumerable<Personal?>> FindAllAsync();

        Task<Personal?> FindByIdAsync(long id);

        Task<Personal> FindByIdAsync(Guid id);

        Task<Personal> AddAsync(Personal personal);

        Task<Personal> UpdateAsync(Personal personal);

        Task<Personal> DeleteAsync(Personal personal);

        Task<IEnumerable<Personal>> GetManyAsync<TOrder>(Expression<Func<Personal, bool>> where, Expression<Func<Personal, TOrder>> order, SortType type);

    }
}
