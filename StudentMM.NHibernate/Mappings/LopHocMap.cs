using FluentNHibernate.Mapping;
using StudentMM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.Mappings
{
    public class LopHocMap : ClassMap<LopHoc>
    {
        public LopHocMap()
        {
            Table("LopHoc");

            Id(x => x.MaLop).GeneratedBy.Identity();
            Map(x => x.TenLop);
            Map(x => x.MonHoc);

            References(x => x.GiaoVien).Column("MaGV").Not.Nullable(); // FK đến GiaoVien

            HasMany(x => x.SinhViens)
                .KeyColumn("MaLop")
                .Inverse()
                .Cascade.All();
        }
    }
}
