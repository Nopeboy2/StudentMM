using AutoMapper;
using StudentMM.Common.Dtos.Class;
using StudentMM.Common.Dtos.Student;
using StudentMM.Common.Dtos.Teacher;
using StudentMM.Common.Models;

namespace StudentMM.Common.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SinhVien, SinhVienDto>()
                                .ForMember(dest => dest.MaLop, opt => opt.MapFrom(src => src.LopHoc.MaLop))
                                .ForMember(dest => dest.TenLop, opt => opt.MapFrom(src => src.LopHoc.TenLop))
                                .ForMember(dest => dest.MonHoc, opt => opt.MapFrom(src => src.LopHoc.MonHoc));

            CreateMap<SinhVienDto, SinhVien>()
                    .ForMember(dest => dest.LopHoc, opt => opt.Ignore());

            CreateMap<LopHoc, LopHocDto>();

            CreateMap<GiaoVien, GiaoVienDto>().ReverseMap();
        }
    }
}
