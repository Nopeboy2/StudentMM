using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Models
{
    public class GiaoVien
    {
        public virtual int MaGV { get; set; }
        public virtual string HoTen { get; set; } = null!;
        public virtual DateTime NgaySinh { get; set; }
        public virtual ICollection<LopHoc> LopHocs { get; set; } = null!;
    }
}
