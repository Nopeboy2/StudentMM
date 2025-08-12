using NHibernate;
using StudentMM.Common.IServices;
using StudentMM.Common.Models;
using StudentMM.NHibernate.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.Repositories
{
    public class GiaoVienRepository : GenericRepository<GiaoVien>, IGiaoVienRepository
    {
        private readonly ISession _session;
        public GiaoVienRepository(ISession session) : base(session)
        {
            _session = session;
        }
    }
}
