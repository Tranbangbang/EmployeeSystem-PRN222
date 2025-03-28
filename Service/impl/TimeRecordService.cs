using EmployManager.Dal;
using EmployManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Service.impl
{
    public class TimeRecordService : ITimeRecordService
    {
        private readonly ApplicationDbContext _context;

        public TimeRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TimeRecord>> GetTimeRecordsByEmployeeIdAsync(int employeeId)
        {
            return await _context.TimeRecords
                .Where(tr => tr.EmployeeId == employeeId)
                .OrderByDescending(tr => tr.CheckInTime)
                .ToListAsync();
        }

        public async Task<List<TimeRecord>> GetTimeRecordsByMonthAsync(int employeeId, int month, int year)
        {
            return await _context.TimeRecords
                .Where(tr => tr.EmployeeId == employeeId &&
                       tr.CheckInTime.Month == month &&
                       tr.CheckInTime.Year == year)
                .OrderByDescending(tr => tr.CheckInTime)
                .ToListAsync();
        }

        public async Task<TimeRecord> GetOpenTimeRecordAsync(int employeeId)
        {
            return await _context.TimeRecords
                .Where(tr => tr.EmployeeId == employeeId && tr.CheckOutTime == null)
                .OrderByDescending(tr => tr.CheckInTime)
                .FirstOrDefaultAsync();
        }


        //-----tim ban ghi cham cong hom nay 
        public async Task<TimeRecord> GetAttendanceToday(int employeeId)
        {
            var today = DateTime.Today; // Ngày hôm nay

            // Tìm bản ghi có CheckInTime 
            TimeRecord result = await _context.TimeRecords
                .Where(tr => tr.EmployeeId == employeeId 
                    && tr.CheckInTime.Date == today) 
                .OrderByDescending(tr => tr.CheckInTime)
                .FirstOrDefaultAsync(); 

            return result;
        }


        public async Task<bool> CheckInAsync(int employeeId, string notes = null)
        {

            TimeRecord timeToday = await this.GetAttendanceToday(employeeId);

            if (timeToday != null)
            {
                timeToday.CheckOutTime = DateTime.Now;

                _context.TimeRecords.Update(timeToday);
                await _context.SaveChangesAsync();
                return true;
            }
                
            else
            {
                var timeRecord = new TimeRecord
                {
                    EmployeeId = employeeId,
                    CheckInTime = DateTime.Now,
                    Notes = notes
                };
                _context.TimeRecords.Add(timeRecord);
                await _context.SaveChangesAsync();
                return true;
            }

            //try
            //{
            //    // Check if there's already an open time record
            //    var openRecord = await GetOpenTimeRecordAsync(employeeId);
            //    if (openRecord != null)
            //        return false;

            //    var timeRecord = new TimeRecord
            //    {
            //        EmployeeId = employeeId,
            //        CheckInTime = DateTime.Now,
            //        Notes = notes
            //    };

            //    _context.TimeRecords.Add(timeRecord);
            //    await _context.SaveChangesAsync();
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public async Task<bool> CheckOutAsync(int employeeId)
        {
            try
            {
                var openRecord = await GetOpenTimeRecordAsync(employeeId);
                if (openRecord == null)
                    return false;

                openRecord.CheckOutTime = DateTime.Now;
                _context.TimeRecords.Update(openRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TimeRecord> GetTimeRecordByIdAsync(int id)
        {
            return await _context.TimeRecords
                .Include(tr => tr.Employee)
                .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task<bool> UpdateTimeRecordAsync(TimeRecord timeRecord)
        {
            try
            {
                _context.Entry(timeRecord).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTimeRecordAsync(int id)
        {
            try
            {
                var timeRecord = await _context.TimeRecords.FindAsync(id);
                if (timeRecord == null)
                    return false;

                _context.TimeRecords.Remove(timeRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
