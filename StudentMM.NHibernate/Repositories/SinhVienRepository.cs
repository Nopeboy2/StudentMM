using NHibernate;
using StudentMM.Common.Dtos;
using StudentMM.Common.Models;
using StudentMM.NHibernate.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;


namespace StudentMM.NHibernate.Repositories
{
    public class SinhVienRepository : GenericRepository<SinhVien>, ISinhVienRepository
    {
        private readonly ISession _session;
        public SinhVienRepository(ISession session) : base(session)
        {
            _session = session;
        }
        public async Task<GetSearchObjectDto<SinhVien>> GetStudentListWithClassAsync(int pageNumber,int pageSize,string searchBySomething,bool? sort,int? maLop = null)
        {
            try
            {
                var query = _session.Query<SinhVien>()
                    .Fetch(s => s.LopHoc)
                    .AsQueryable();

                // Lọc theo mã lớp nếu có
                if (maLop.HasValue && maLop.Value > 0)
                {
                    query = query.Where(s => s.LopHoc.MaLop == maLop.Value);
                }

                // Lọc theo từ khóa tìm kiếm
                if (!string.IsNullOrEmpty(searchBySomething))
                {
                    query = query.Where(s =>
                        s.HoTen.Contains(searchBySomething) ||
                        s.MaSo.ToString().Contains(searchBySomething) ||
                        s.DiaChi.Contains(searchBySomething));
                }

                // Sắp xếp nếu có yêu cầu
                if (sort.HasValue)
                {
                    query = sort.Value
                        ? query.OrderBy(s => s.HoTen)
                        : query.OrderByDescending(s => s.HoTen);
                }

                // Lấy tổng số bản ghi (trước khi phân trang)
                int totalRecord = await query.CountAsync();

                // Lấy dữ liệu theo trang
                var sinhViens = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new GetSearchObjectDto<SinhVien>
                {
                    totalRecord = totalRecord,
                    Data = sinhViens
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new GetSearchObjectDto<SinhVien>(); // Trả về rỗng nếu có lỗi
            }
        }

    }
}
