using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos.Teacher
{
    [DataContract]
    public class ThongKeSinhVienDto
    {
        [DataMember(Order = 1)]
        public int MaGV { get; set; }

        [DataMember(Order = 2)]
        public string TenLop { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public int SoSinhVien { get; set; }
    }
}
