using StudentMM.Common.Dtos;
using StudentMM.Common.Dtos.Student;
using StudentMM.Common.Dtos.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.IServices
{
    [ServiceContract]
    public interface ISinhVienGrpcService
    {
        [OperationContract]
        Task<ResponseWrapper<PagedResult<SinhVienDto>>> GetAllSinhVienAsync(SinhVienRequest request);

        [OperationContract]
        Task<ResponseWrapper<SinhVienDto>> AddSinhVienAsync(CreateSinhVienRequest request);

        [OperationContract]
        Task<ResponseWrapper<SinhVienDto>> UpdateSinhVienAsync(UpdateSinhVienRequest request);

        [OperationContract]
        Task<ResponseWrapper<bool>> DeleteSinhVienAsync(string maSinhVien);

        [OperationContract]
        Task<ResponseWrapper<SinhVienDto?>> GetSinhVienByIdAsync(string maSinhVien);
    }
}
