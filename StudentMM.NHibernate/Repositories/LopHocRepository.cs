using NHibernate;
using StudentMM.Common.Models;
using StudentMM.NHibernate.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.Repositories
{
    public class LopHocRepository : GenericRepository<LopHoc>, ILopHocRepository
    {
        private readonly ISession _session;
        public LopHocRepository(ISession session) : base(session)
        {
            _session = session;
        }
    }
}
