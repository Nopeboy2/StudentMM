using StudentMM.Common.Dtos;
using StudentMM.Common.Dtos.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.IServices
{
    [ServiceContract]
    public interface ILopHocGrpcService
    {
        [OperationContract]
        Task<ResponseWrapper<List<LopHocDto>>> GetAllLopHocAsync();
    }
}
