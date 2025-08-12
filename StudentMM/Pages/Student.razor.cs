using AntDesign;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using StudentMM.Common.Dtos.Class;
using StudentMM.Common.Dtos.Student;
using StudentMM.Common.IServices;

namespace StudentMM.Pages
{
    public partial class Student : ComponentBase
    {
        [Inject] ISinhVienGrpcService sinhVienService { get; set; }
        [Inject] ILopHocGrpcService lophocService { get; set; }
        [Inject] ModalService Modal { get; set; }
        [Inject] ConfirmService ComfirmService { get; set; }
        [Inject] NotificationService _notice { get; set; }

        private int[] pageSizeOptions = { 3, 5, 10, 20, 50 };
        private string searchTerm = string.Empty;
        private List<SinhVienDto> sinhViens = new();
        private List<SinhVienDto> filteredSinhViens = new();
        private bool isSortedAscending = true;
        private int _total;

        // DRAWER
        private string drawerTitle = string.Empty;
        private bool visibleCreateDrawer = false;
        private bool visibleUpdateDrawer = false;
        private CreateSinhVienRequest creatingStudent = new();
        private UpdateSinhVienRequest updatingStudent = new();
        private List<LopHocDto> _lopHocSelect = new();

        // SELECT
        private List<LopHocDto> _lopHocDtos = new();
        private LopHocDto _selectedItem = new();
        private int? _selectedValue;


        private int pageNumber = 1;
        private int pageSize = 3;
        private int totalStudent = 0;


        protected override async Task OnInitializedAsync()
        {
            await LoadStudentsAsync();
            await LoadClass();
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                var request = new SinhVienRequest { PageNumber = pageNumber, PageSize = pageSize, searchBySomething = searchTerm, sort = isSortedAscending , MaLop = _selectedValue };

                var response = await sinhVienService.GetAllSinhVienAsync(request);

                if (response == null || response.Data == null)
                {
                    sinhViens = new List<SinhVienDto>();
                    totalStudent = 0;
                }
                else
                {
                    //sinhViens = response.Data.Items.ToList();
                    //int startIndex = (pageNumber - 1) * pageSize + 1;
                    //sinhViens = response.Data.Items
                    //    .Select((sinhVien, index) =>
                    //    {
                    //        sinhVien.STT = startIndex + index;
                    //        return sinhVien;
                    //    })
                    //    .ToList();
                    //totalStudent = response.Data.TotalCount;
                    var items = response.Data.Items.ToList();

                    int startIndex = (pageNumber - 1) * pageSize + 1;
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].STT = startIndex + i;
                    }

                    sinhViens = items;
                    totalStudent = response.Data.TotalCount;

                }
                filteredSinhViens = new List<SinhVienDto>(sinhViens);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadStudentsAsync: {ex.Message}");
            }
        }

        private async Task OnPageChanged(PaginationEventArgs args)
        {
            pageNumber = args.Page;
            await LoadStudentsAsync();
        }
        private async Task OnPageSizeChanged(PaginationEventArgs args)
        {
            pageSize = args.PageSize;  // Lấy kích thước trang mới
            pageNumber = 1;            // Reset trang về 1
            await LoadStudentsAsync();
        }

        private async Task LoadClass()
        {
            creatingStudent = new CreateSinhVienRequest();
            var lopHocs = await lophocService.GetAllLopHocAsync();
            if (lopHocs != null)
            {
                _lopHocDtos = lopHocs.Data.ToList();
            }
            else
            {
                _lopHocDtos = new List<LopHocDto>();
            }
            _lopHocDtos.Insert(0, new LopHocDto { MaLop = null, TenLop = "Tất cả" });

            // Chọn mặc định "Tất cả"
            _selectedItem = _lopHocDtos[0];
            _selectedValue = null;
        }

        private async void OnSelectedItemChangedHandler(LopHocDto lopHocDto)
        {
            //_selectedItem = lopHocDto;
            //_selectedValue = lopHocDto.MaLop;

            //if (_selectedValue != null)
            //{
            //    filteredSinhViens = sinhViens.Where(sv => sv.MaLop == _selectedValue).ToList();
            //}
            //else
            //{
            //    filteredSinhViens = new List<SinhVienDto>(sinhViens);
            //}
            _selectedItem = lopHocDto;
            _selectedValue = lopHocDto?.MaLop;

            pageNumber = 1; // reset về trang 1 khi đổi lớp
            await LoadStudentsAsync();
        }

        private async Task FilterStudents(ChangeEventArgs e)
        {
            searchTerm = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredSinhViens = new List<SinhVienDto>(sinhViens);
            }
            else
            {
                await LoadStudentsAsync();
            }
        }


        private async Task ShowMessageAsync(string title, string description)
        {
            await _notice.Open(new NotificationConfig()
            {
                Message = title,
                Description = description
            });
        }

        // SORT

        private void SortByName()
        {
            filteredSinhViens = isSortedAscending
                ? filteredSinhViens.OrderBy(sv => sv.HoTen).ToList()
                : filteredSinhViens.OrderByDescending(sv => sv.HoTen).ToList();
            isSortedAscending = !isSortedAscending;
        }


        private async Task Delete(SinhVienDto row)
        {
            await sinhVienService.DeleteSinhVienAsync(row.MaSo.ToString());

            var request = new SinhVienRequest { PageNumber = pageNumber, PageSize = pageSize };

            var response = await sinhVienService.GetAllSinhVienAsync(request);

            if (response == null || response.Data == null || response.Data.Items.Count == 0)
            {
                if (pageNumber > 1)
                {
                    pageNumber--;
                }
            }

            await LoadStudentsAsync();
        }


        private void Cancel()
        {
            return;
        }


        // DRAWER

        // CREATE
        private async void OnOpenCreateDrawer()
        {
            creatingStudent = new CreateSinhVienRequest();
            var lopHocs = await lophocService.GetAllLopHocAsync();
            if (lopHocs != null)
            {
                _lopHocSelect = lopHocs.Data.ToList();
            }
            else
            {
                _lopHocSelect = new List<LopHocDto>();
            }
            drawerTitle = "Thêm Sinh Viên";
            this.visibleCreateDrawer = true;
        }

        void OnCloseCreateDrawer()
        {
            visibleCreateDrawer = false;
        }

        // UPDATE 

        private async void OnOpenUpdateDrawer(SinhVienDto sv)
        {
            updatingStudent = new UpdateSinhVienRequest
            {
                MaSo = sv.MaSo,
                HoTen = sv.HoTen,
                NgaySinh = sv.NgaySinh,
                DiaChi = sv.DiaChi,
                MaLop = sv.MaLop,
            };

            drawerTitle = "Chỉnh Sửa Sinh Viên";
            visibleUpdateDrawer = true;
            StateHasChanged();
        }

        void OnCloseUpdateDrawer()
        {
            visibleUpdateDrawer = false;
        }


        // HANDLE ACTION
        private async Task HandleUpdate()
        {
            if (updatingStudent.MaSo != null || updatingStudent != null)
            {
                try
                {
                    var response = await sinhVienService.UpdateSinhVienAsync(updatingStudent);
                    if (response.Data != null)
                    {
                        await LoadStudentsAsync();
                        await ShowMessageAsync("Update", "Cập nhật thành viên thành công");
                        OnCloseUpdateDrawer();
                    }
                    else
                    {
                        await ShowMessageAsync("Update", "Cập nhật thành viên thất bại");
                    }
                }
                catch (Exception ex)
                {
                    await ShowMessageAsync("Lỗi hệ thống", ex.Message);
                }
            }
        }


        private async Task HandleCreate()
        {
            try
            {
                var response = await sinhVienService.AddSinhVienAsync(creatingStudent);
                if (response.Data != null)
                {
                    totalStudent += 1;
                    await LoadStudentsAsync();
                    creatingStudent = new CreateSinhVienRequest();
                    await ShowMessageAsync("Create", "Thêm thành viên thành công");
                    OnCloseCreateDrawer();
                }
                else
                {
                    await ShowMessageAsync("Create", "Thêm thành viên thất bại");
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Lỗi hệ thống", ex.Message);
            }
        }
    }
}
