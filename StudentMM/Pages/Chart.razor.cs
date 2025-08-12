using AntDesign.Charts;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StudentMM.Common.Dtos.Teacher;
using StudentMM.Common.IServices;

namespace StudentMM.Pages
{
    public partial class Chart : ComponentBase
    {
        [Inject] private IGiaoVienGrpcService _giaoVienService { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; }

        private int? _selectedTeacherId;
        private List<GiaoVienDto> giaoViens = new();
        private List<GiaoVienDto> selectionGiaoViens = new();
        private string errorMessage = string.Empty;
        private List<object> chartData = new();

        ColumnConfig config1 = new()
        {
            IsGroup = true,
            XField = "tenlop",
            YField = "sosinhvien",
            YAxis = new AntDesign.Charts.ValueAxis()
            {
                Min = 0,
            },
            Label = new ColumnViewConfigLabel()
            {
                Visible = true,
            },
            Color = new string[] { "#1ca9e6" },
        };


        protected override async Task OnInitializedAsync()
        {
            await LoadTeachers();
        }


        private async Task LoadTeachers()
        {
            var response = await _giaoVienService.GetAllGiaoVienAsync();

            if (response == null)
            {
                giaoViens = new List<GiaoVienDto>();
            }
            else
            {
                giaoViens = response.ToList();
            }

            selectionGiaoViens = new List<GiaoVienDto>(giaoViens);
            StateHasChanged();
        }

        private async Task OnSelectedItemChangedHandler(GiaoVienDto e)
        {
            if (e != null)
            {
                _selectedTeacherId = e.MaGV;
                if (_selectedTeacherId.HasValue)
                {
                    await LoadChartData(_selectedTeacherId.Value);
                }
            }
        }

        private async Task LoadChartData(int teacherId)
        {
            var chartResponse = await _giaoVienService.GetChartGiaoVienAsync(teacherId.ToString());
            if (chartResponse != null)
            {
                chartData = chartResponse.Select(c => new
                {
                    giaovien = c.MaGV,
                    tenlop = c.TenLop,
                    sosinhvien = c.SoSinhVien
                }).Cast<object>().ToList();
            }
            else
            {
                chartData = new List<object>();
            }

            await InvokeAsync(StateHasChanged);
        }


        private async Task ExportToExcel()
        {
            Console.WriteLine("📌 ExportToExcel() started!");

            try
            {
                using var workbook = new XLWorkbook();
                var dataTable = new System.Data.DataTable();

                dataTable.Columns.Add("Mã Giáo Viên", typeof(int));
                dataTable.Columns.Add("Tên Lớp", typeof(string));
                dataTable.Columns.Add("Số Sinh Viên", typeof(int));

                var chartResponse = await _giaoVienService.GetChartGiaoVienAsync(_selectedTeacherId.ToString());

                foreach (var item in chartResponse)
                {
                    dynamic chartItem = item;
                    dataTable.Rows.Add(_selectedTeacherId, chartItem.TenLop, chartItem.SoSinhVien);
                }

                var worksheet = workbook.Worksheets.Add("Danh Sách");
                worksheet.Cell(1, 1).InsertTable(dataTable);

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var bytes = stream.ToArray();
                var base64 = Convert.ToBase64String(bytes);

                Console.WriteLine($"📌 Base64 Length: {base64.Length}");

                await JS.InvokeVoidAsync("downloadFile", "ThongKe.xlsx", base64);

                Console.WriteLine("✅ JS.InvokeVoidAsync executed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
