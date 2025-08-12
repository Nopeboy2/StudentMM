using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos.Class
{
    [DataContract]
    public class LopHocDto
    {
        [DataMember(Order = 1)]
        public int? MaLop { get; set; }

        [DataMember(Order = 2)]
        public string TenLop { get; set; } = null!;

        [DataMember(Order = 3)]
        public string MonHoc { get; set; } = null!;

        [DataMember(Order = 4)]
        public int MaGV { get; set; }

        [DataMember(Order = 5)]
        public string TenGV { get; set; } = null!;

        [DataMember(Order = 6)]
        public int SoLuongSinhVien { get; set; }
    }
}
