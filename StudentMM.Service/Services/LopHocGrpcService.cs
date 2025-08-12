using AutoMapper;
using StudentMM.Common.Dtos;
using StudentMM.Common.Dtos.Class;
using StudentMM.Common.IServices;
using StudentMM.NHibernate.UnitOfWork;

namespace StudentMM.Service.Services
{
    public class LopHocGrpcService : ILopHocGrpcService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LopHocGrpcService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<List<LopHocDto>>> GetAllLopHocAsync()
        {
            var lopHocs = await _unitOfWork.LopHoc.GetAllAsync();

            var lopHocDtos = _mapper.Map<List<LopHocDto>>(lopHocs);

            return new ResponseWrapper<List<LopHocDto>>("Lấy danh sách lớp học thành công", lopHocDtos);
        }
    }
}
