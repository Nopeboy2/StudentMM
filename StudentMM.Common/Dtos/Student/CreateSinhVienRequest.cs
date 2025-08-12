using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentMM.Common.Dtos.Student
{
    [DataContract]
    public class CreateSinhVienRequest
    {
        [DataMember(Order = 1)]
        [Required(ErrorMessage = "Tên sinh viên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sinh viên không được vượt quá 100 ký tự.")]
        public string HoTen { get; set; } = null!;

        [DataMember(Order = 2)]
        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự.")]
        public string DiaChi { get; set; } = null!;

        [DataMember(Order = 3)]
        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ.")]
        public DateTime NgaySinh { get; set; }

        [DataMember(Order = 4)]
        [Required(ErrorMessage = "Mã lớp học là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mã lớp học không quá 100 ký tự.")]
        public int? MaLop { get; set; }
    }
}
