using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos.Student
{
    [DataContract]
    public class SinhVienRequest
    {
        [DataMember(Order = 1)]
        public int PageNumber { get; set; }

        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        [DataMember(Order = 3)]
        public string? searchBySomething { get; set; }

        [DataMember(Order = 4)]
        public bool? sort { get; set; }
        [DataMember(Order = 5)]
        public int? MaLop { get; set; }  // Thêm biến chọn lớp (có thể null)

    }
}
