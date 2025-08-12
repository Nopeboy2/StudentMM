using FluentNHibernate.Mapping;
using StudentMM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.NHibernate.Mappings
{
    public class GiaoVienMap : ClassMap<GiaoVien>
    {
        public GiaoVienMap()
        {
            Table("GiaoVien");

            Id(x => x.MaGV).GeneratedBy.Identity();
            Map(x => x.HoTen);
            Map(x => x.NgaySinh);
            HasMany(x => x.LopHocs).KeyColumn("MaGV").Inverse().Cascade.All();
        }
    }
}
