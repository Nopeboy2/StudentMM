using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos
{
    [DataContract]
    public class PagedResult<T>
    {
        [DataMember(Order = 1)]
        public List<T> Items { get; set; } = new List<T>();

        [DataMember(Order = 2)]
        public int TotalPages { get; set; }

        [DataMember(Order = 3)]
        public int CurrentPage { get; set; }

        [DataMember(Order = 4)]
        public int PageSize { get; set; }

        [DataMember(Order = 5)]
        public int TotalCount { get; set; }
    }
}
