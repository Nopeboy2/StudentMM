using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos
{
    public class GetSearchObjectDto<T>
    {
        public int totalRecord { get; set; }
        public List<T> Data { get; set; }
    }
}
