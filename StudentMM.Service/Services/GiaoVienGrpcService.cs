using AutoMapper;
using StudentMM.Common.Dtos.Teacher;
using StudentMM.Common.IServices;
using StudentMM.NHibernate.UnitOfWork;

namespace StudentMM.Service.Services
{
   public class GiaoVienGrpcService : IGiaoVienGrpcService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GiaoVienGrpcService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GiaoVienDto>> GetAllGiaoVienAsync()
        {
            var giaoViens = await _unitOfWork.GiaoVien.GetAllAsync();

            var giaoVienDtos = _mapper.Map<List<GiaoVienDto>>(giaoViens);

            return new List<GiaoVienDto>(giaoVienDtos);
        }

        public async Task<List<ThongKeSinhVienDto>> GetChartGiaoVienAsync(string teacherId)
        {
            var query = await _unitOfWork.GiaoVien.GetByIdAsync(int.Parse(teacherId));
            var countClassOfTeacher = query.LopHocs.Count();
            var chartGiaoVien = new List<ThongKeSinhVienDto>();
            if (countClassOfTeacher > 0)
            {
                foreach (var item in query.LopHocs)
                {
                    ThongKeSinhVienDto chartDto = new ThongKeSinhVienDto()
                    {
                        MaGV = item.MaGV,
                        TenLop = item.TenLop,
                        SoSinhVien = item.SinhViens.Count(),
                    };
                    chartGiaoVien.Add(chartDto);
                }
            }
            return chartGiaoVien;
        }

    }
}
