using EmployManager.Dal;
using EmployManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Service.impl
{
    public class EmployeeProfileService : IEmployeeProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EmployeeProfileService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<EmployeeProfile> GetProfileByEmployeeIdAsync(int employeeId)
        {
            var profile = await _context.EmployeeProfiles
                .FirstOrDefaultAsync(p => p.EmployeeId == employeeId);

            if (profile == null)
            {
                // Tạo profile mới nếu chưa có
                profile = new EmployeeProfile { EmployeeId = employeeId };
                _context.EmployeeProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }

            return profile;
        }

        public async Task<bool> CreateProfileAsync(EmployeeProfile profile)
        {
            try
            {
                // Kiểm tra xem đã có profile chưa
                var existingProfile = await _context.EmployeeProfiles
                    .FirstOrDefaultAsync(p => p.EmployeeId == profile.EmployeeId);

                if (existingProfile != null)
                {
                    return false;
                }

                _context.EmployeeProfiles.Add(profile);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProfileAsync(EmployeeProfile profile)
        {
            try
            {
                _context.Entry(profile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UploadProfileImageAsync(int employeeId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return false;

                var profile = await GetProfileByEmployeeIdAsync(employeeId);

                // Tạo thư mục uploads nếu chưa có
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file duy nhất
                var uniqueFileName = $"{employeeId}_{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn ảnh trong profile
                profile.ProfileImage = $"/uploads/profiles/{uniqueFileName}";
                await UpdateProfileAsync(profile);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetProfileImagePathAsync(int employeeId)
        {
            var profile = await GetProfileByEmployeeIdAsync(employeeId);
            return profile?.ProfileImage;
        }
    }
}