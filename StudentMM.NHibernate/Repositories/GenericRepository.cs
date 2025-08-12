using NHibernate;
using NHibernate.Linq;
using StudentMM.NHibernate.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ISession _session;

        public GenericRepository(ISession session)
        {
            _session = session;
        }

        public async Task<int> CountAsync()
        {
            try
            {
                return await _session.Query<T>().CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CountAsync: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> CreateAsync(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    await _session.SaveAsync(entity);
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<T> CreateAndReturnAsync(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                    {
                        Console.WriteLine("🔴 Entity null, không thể lưu vào DB!");
                        return null;
                    }

                    Console.WriteLine($"🟢 Đang lưu entity vào DB");
                    await _session.SaveAsync(entity);
                    await transaction.CommitAsync();

                    Console.WriteLine("🟢 Lưu thành công!");
                    return entity;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"🔴 Lỗi khi lưu: {ex.Message}");
                    return null;
                }
            }
        }


        public async Task<T> GetByIdAsync(int? id)
        {
            return await _session.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.Run(() => _session.Query<T>().ToListAsync());
        }

        public IQueryable<T> Queryable()
        {
            return _session.Query<T>();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    await _session.UpdateAsync(entity);
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var entity = await _session.GetAsync<T>(id);
                    if (entity == null) return false;

                    await _session.DeleteAsync(entity);
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DeleteRangeAsync(List<T> entities)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        await _session.DeleteAsync(entity);
                    }
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> CreateRangeAsync(List<T> entities)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        await _session.SaveAsync(entity);
                    }
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        public async Task<IQueryable<T>> QueryAbleAsync()
        {
            return await Task.Run(() => _session.Query<T>());
        }
    }
}
