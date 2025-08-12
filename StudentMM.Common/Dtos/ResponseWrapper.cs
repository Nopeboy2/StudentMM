using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos
{
    [DataContract]
    public class ResponseWrapper<T> 
    {
        [DataMember(Order = 1)]
        public string Message { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public T? Data { get; set; }
        public ResponseWrapper() { }

        public ResponseWrapper(string message, T data)
        {
            Message = message;
            Data = data;
        }
    }
}
