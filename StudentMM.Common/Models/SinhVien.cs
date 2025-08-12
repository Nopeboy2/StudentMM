using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Models
{
    public class SinhVien
    {
        public virtual int MaSo { get; set; }
        public virtual string HoTen { get; set; } = null!;
        public virtual DateTime NgaySinh { get; set; }
        public virtual string DiaChi { get; set; } = null!;
        public virtual int? MaLop { get; set; }
        public virtual LopHoc LopHoc { get; set; } = null!;
    }
}
