using StudentMM.Common.Dtos;
using StudentMM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.IRepositories
{
    public interface ISinhVienRepository : IGenericRepository<SinhVien>
    {
        Task<GetSearchObjectDto<SinhVien>> GetStudentListWithClassAsync(int pageNumber, int pageSize, string searchBySomething, bool? sort, int? maLop = null);
    }
}
