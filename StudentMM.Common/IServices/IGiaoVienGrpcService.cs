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
    public interface IGiaoVienGrpcService
    {
        [OperationContract]
        Task<List<GiaoVienDto>> GetAllGiaoVienAsync();

        [OperationContract]
        Task<List<ThongKeSinhVienDto>> GetChartGiaoVienAsync(string teacherId);
    }
}
