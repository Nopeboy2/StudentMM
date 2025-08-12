using NHibernate;
using StudentMM.NHibernate.IRepositories;
using StudentMM.NHibernate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        private IGiaoVienRepository _giaoVienRepository;
        private ILopHocRepository _lopHocRepository;
        private ISinhVienRepository _sinhVienRepository;

        public UnitOfWork(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public IGiaoVienRepository GiaoVien => _giaoVienRepository ??= new GiaoVienRepository(_session);
        public ILopHocRepository LopHoc => _lopHocRepository ??= new LopHocRepository(_session);
        public ISinhVienRepository SinhVien => _sinhVienRepository ??= new SinhVienRepository(_session);

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null && _transaction.IsActive)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null && _transaction.IsActive)
                await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                await _session.FlushAsync();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _session.Dispose();
        }
    }
}
