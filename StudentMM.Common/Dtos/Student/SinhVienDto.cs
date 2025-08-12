using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos.Student
{
    [DataContract]
    public class SinhVienDto
    {
        [DataMember(Order = 1)]
        public int STT { get; set; }
        [DataMember(Order = 2)] 
        public int MaSo { get; set; }
        [DataMember(Order = 3)] 
        public string HoTen { get; set; } = null!;
        [DataMember(Order = 4)] 
        public DateTime NgaySinh { get; set; }
        [DataMember(Order = 5)] 
        public string DiaChi { get; set; } = null!;
        [DataMember(Order = 6)]
        public int MaLop { get; set; }
        [DataMember(Order = 7)]
        public string TenLop { get; set; } = null!;
        [DataMember(Order = 8)] 
        public string MonHoc { get; set; } = null!;
    }
}
