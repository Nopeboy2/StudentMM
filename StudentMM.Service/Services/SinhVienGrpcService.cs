using AutoMapper;
using StudentMM.Common.Dtos;
using StudentMM.Common.Dtos.Student;
using StudentMM.Common.IServices;
using StudentMM.Common.Models;
using StudentMM.NHibernate.IRepositories;
using StudentMM.NHibernate.Repositories;
using StudentMM.NHibernate.UnitOfWork;

namespace StudentMM.Service.Services
{
    public class SinhVienGrpcService : ISinhVienGrpcService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SinhVienGrpcService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<SinhVienDto>> AddSinhVienAsync(CreateSinhVienRequest request)
        {

            var lopHoc = await _unitOfWork.LopHoc.GetByIdAsync(request.MaLop);
            if (lopHoc == null)
            {
                return new ResponseWrapper<SinhVienDto>("Lớp học không tồn tại!", null);
            }
            SinhVien sinhVien = new SinhVien()
            {
                DiaChi = request.DiaChi,
                NgaySinh = request.NgaySinh,
                HoTen = request.HoTen,
                MaLop = request.MaLop,
                LopHoc = lopHoc
            };

            try
            {
                var create = await _unitOfWork.SinhVien.CreateAndReturnAsync(sinhVien);
                await _unitOfWork.SaveChangesAsync();
                var createdSinhVien = create;
                if (createdSinhVien == null)
                {
                    return new ResponseWrapper<SinhVienDto>("Thêm sinh viên thất bại", null);
                }

                return new ResponseWrapper<SinhVienDto>("Thêm sinh viên thành công", _mapper.Map<SinhVienDto>(createdSinhVien));
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<SinhVienDto>("Thêm sinh viên thất bại", null);
            }
        }

        public async Task<ResponseWrapper<bool>> DeleteSinhVienAsync(string maSinhVien)
        {
            var sinhVien = await _unitOfWork.SinhVien.GetByIdAsync(int.Parse(maSinhVien));
            if (sinhVien == null)
            {
                return new ResponseWrapper<bool>("Sinh viên không tồn tại", false);
            }

            await _unitOfWork.SinhVien.DeleteAsync(sinhVien.MaSo);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseWrapper<bool>("Xóa sinh viên thành công", true);
        }

        public async Task<ResponseWrapper<PagedResult<SinhVienDto>>> GetAllSinhVienAsync(SinhVienRequest request)
        {
            // gọi lần đầu
            var response = await _unitOfWork.SinhVien
                .GetStudentListWithClassAsync(request.PageNumber, request.PageSize, request.searchBySomething, request.sort, request.MaLop);

            // lấy total (repository của bạn trả về totalRecord)
            int totalCount = response.totalRecord;

            // Nếu không có dữ liệu trả về nhưng có tổng bản ghi > 0 và page hiện tại > 1
            // => tính trang cuối cùng và gọi lại với page thích hợp
            if ((response.Data == null || (response.Data is System.Collections.ICollection coll && coll.Count == 0))
                && totalCount > 0
                && request.PageNumber > 1)
            {
                int lastPage = (int)Math.Ceiling((double)totalCount / Math.Max(1, request.PageSize));
                if (lastPage < 1) lastPage = 1;

                if (lastPage != request.PageNumber)
                {
                    request.PageNumber = lastPage;
                    response = await _unitOfWork.SinhVien
                        .GetStudentListWithClassAsync(request.PageNumber, request.PageSize, request.searchBySomething, request.sort, request.MaLop);
                }
            }

            // Nếu tổng bản ghi = 0 => trả về cấu trúc rỗng (không cần map)
            if (totalCount == 0)
            {
                var emptyPaged = new PagedResult<SinhVienDto>
                {
                    Items = new List<SinhVienDto>(),
                    TotalPages = 0,
                    CurrentPage = 1,
                    PageSize = request.PageSize,
                    TotalCount = 0
                };

                return new ResponseWrapper<PagedResult<SinhVienDto>>("Không tìm thấy sinh viên", emptyPaged);
            }

            // Map an toàn (nếu Data null thì trả list rỗng)
            var sinhVienDtos = response.Data != null
                ? _mapper.Map<List<SinhVienDto>>(response.Data)
                : new List<SinhVienDto>();

            int totalPages = (int)Math.Ceiling((double)totalCount / Math.Max(1, request.PageSize));

            var pagedSinhVienDtos = new PagedResult<SinhVienDto>
            {
                Items = sinhVienDtos,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return new ResponseWrapper<PagedResult<SinhVienDto>>("Lấy danh sách sinh viên thành công", pagedSinhVienDtos);
        }

        public async Task<ResponseWrapper<SinhVienDto?>> GetSinhVienByIdAsync(string maSinhVien)
        {
            var sinhVien = await _unitOfWork.SinhVien.GetByIdAsync(int.Parse(maSinhVien));
            if (sinhVien == null)
            {
                return new ResponseWrapper<SinhVienDto?>("Sinh viên không tồn tại", null);
            }

            var sinhVienDto = _mapper.Map<SinhVienDto>(sinhVien);
            return new ResponseWrapper<SinhVienDto?>("Lấy thông tin sinh viên thành công", sinhVienDto);
        }

        public async Task<ResponseWrapper<PagedResult<SinhVienDto>>> GetSinhVienByLopAsync(SinhVienRequest request)
        {
            var result = await _unitOfWork.SinhVien
                .GetStudentListWithClassAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.searchBySomething,
                    request.sort,
                    request.MaLop
                );

            int totalCount = result.totalRecord;

            if ((result.Data == null || (result.Data is System.Collections.ICollection coll && coll.Count == 0))
                && totalCount > 0
                && request.PageNumber > 1)
            {
                int lastPage = (int)Math.Ceiling((double)totalCount / Math.Max(1, request.PageSize));
                if (lastPage < 1) lastPage = 1;

                if (lastPage != request.PageNumber)
                {
                    request.PageNumber = lastPage;
                    result = await _unitOfWork.SinhVien
                        .GetStudentListWithClassAsync(
                            request.PageNumber,
                            request.PageSize,
                            request.searchBySomething,
                            request.sort,
                            request.MaLop
                        );
                }
            }

            if (totalCount == 0)
            {
                var emptyPaged = new PagedResult<SinhVienDto>
                {
                    Items = new List<SinhVienDto>(),
                    TotalPages = 0,
                    CurrentPage = 1,
                    PageSize = request.PageSize,
                    TotalCount = 0
                };

                return new ResponseWrapper<PagedResult<SinhVienDto>>("Không tìm thấy sinh viên", emptyPaged);
            }

            var sinhVienDtos = result.Data != null
                ? _mapper.Map<List<SinhVienDto>>(result.Data)
                : new List<SinhVienDto>();

            int totalPages = (int)Math.Ceiling((double)totalCount / Math.Max(1, request.PageSize));

            var pagedSinhVienDtos = new PagedResult<SinhVienDto>
            {
                Items = sinhVienDtos,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return new ResponseWrapper<PagedResult<SinhVienDto>>("Lấy danh sách sinh viên thành công", pagedSinhVienDtos);
        }

        public async Task<ResponseWrapper<SinhVienDto>> UpdateSinhVienAsync(UpdateSinhVienRequest request)
        {
            var sinhVien = await _unitOfWork.SinhVien.GetByIdAsync(request.MaSo);

            if (sinhVien == null)
            {
                return new ResponseWrapper<SinhVienDto>("Sinh viên không tồn tại", null);
            }

            var lopHoc = await _unitOfWork.LopHoc.GetByIdAsync(request.MaLop);
            if (lopHoc == null)
            {
                return new ResponseWrapper<SinhVienDto>("Lớp học không tồn tại!", null);
            }

            sinhVien.HoTen = request.HoTen;
            sinhVien.NgaySinh = request.NgaySinh;
            sinhVien.DiaChi = request.DiaChi;
            sinhVien.MaLop = request.MaLop;
            sinhVien.LopHoc = lopHoc;

            await _unitOfWork.SinhVien.UpdateAsync(sinhVien);
            await _unitOfWork.SaveChangesAsync();
            var sinhVienDto = _mapper.Map<SinhVienDto>(sinhVien);

            return new ResponseWrapper<SinhVienDto>("Cập nhật sinh viên thành công", sinhVienDto);
        }
    }

}
