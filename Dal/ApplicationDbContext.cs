using EmployManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Dal
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conStr = "server=localhost;database=EmployManager;Integrated Security=True;encrypt=true;TrustServerCertificate=true";
                optionsBuilder.UseSqlServer(conStr);
            }
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }
        public DbSet<OvertimeRequest> OvertimeRequests { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<TimeRecord>().HasKey(tr => tr.Id);
            modelBuilder.Entity<OvertimeRequest>().HasKey(or => or.Id);
            modelBuilder.Entity<Department>().HasKey(d => d.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Salary>().HasKey(s => s.Id);
            modelBuilder.Entity<EmployeeProfile>().HasKey(ep => ep.Id);
            modelBuilder.Entity<Leave>().HasKey(l => l.Id);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeRecord>()
                .HasOne(tr => tr.Employee)
                .WithMany(e => e.TimeRecords)
                .HasForeignKey(tr => tr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OvertimeRequest>()
                .HasOne(or => or.Employee)
                .WithMany(e => e.OvertimeRequests)
                .HasForeignKey(or => or.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeProfile>()
                .HasOne(ep => ep.Employee)
                .WithOne()
                .HasForeignKey<EmployeeProfile>(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Leave>()
                .HasOne(l => l.Employee)
                .WithMany()
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>().Property(e => e.EmployeeCode).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.FirstName).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.LastName).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.Email).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.PasswordHash).IsRequired();

            modelBuilder.Entity<Department>().Property(d => d.Name).IsRequired();
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired();

            modelBuilder.Entity<TimeRecord>().Property(tr => tr.CheckInTime).IsRequired();
            modelBuilder.Entity<OvertimeRequest>().Property(or => or.StartTime).IsRequired();
            modelBuilder.Entity<OvertimeRequest>().Property(or => or.EndTime).IsRequired();
            modelBuilder.Entity<OvertimeRequest>().Property(or => or.Reason).IsRequired();

            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeCode).IsUnique();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Employee" }
            );
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "IT", Description = "Công nghệ thông tin" },
                new Department { Id = 2, Name = "HR", Description = "Nhân sự" },
                new Department { Id = 3, Name = "Finance", Description = "Tài chính" },
                new Department { Id = 4, Name = "Marketing", Description = "Marketing" }
            );
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    EmployeeCode = "ADMIN001",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@company.com",
                    PhoneNumber = "0392234",
                    DepartmentId = 1,
                    RoleId = 1,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    HireDate = new System.DateTime(2023, 1, 1),
                    IsActive = true
                }
            );
        }
    }
}