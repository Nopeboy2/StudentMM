using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Models
{
    public class LopHoc
    {
        public virtual int MaLop { get; set; }
        public virtual string TenLop { get; set; } = null!;
        public virtual string MonHoc { get; set; } = null!;
        public virtual int MaGV { get; set; }
        public virtual GiaoVien GiaoVien { get; set; } = null!;

        public virtual ICollection<SinhVien> SinhViens { get; set; } = null!;
    }
}
