using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using StudentMM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.Mappings
{
    public class SinhVienMap : ClassMap<SinhVien>
    {
        public SinhVienMap()
        {
            Table("SinhVien");

            Id(x => x.MaSo).GeneratedBy.Identity();
            Map(x => x.HoTen);
            Map(x => x.NgaySinh);
            Map(x => x.DiaChi);

            References(x => x.LopHoc).Column("MaLop").Not.Nullable(); // FK đến LopHoc
        }
    }
}
