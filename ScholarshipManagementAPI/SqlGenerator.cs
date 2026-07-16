using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.DTOs.School.Students;

namespace ScholarshipManagementAPI
{
    public class SqlGenerator {
        public static void Print() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=Dummy;Trusted_Connection=True;")
                .Options;
            using var ctx = new AppDbContext(options);
            var query = ctx.StudentRegistrations
                    .AsNoTracking()
                    .Where(x => x.IsActive);
            
            var selectQuery = query.Select(x => new StudentRequestDto {
                        StudentId = x.StudentId,
                        FullName = string.Join(" ",
                            new[] {
                        x.FirstName,
                        x.SecondName,
                        x.ThirdName,
                        x.LastName
                            }.Where(s => !string.IsNullOrWhiteSpace(s)))
            });
            var sql = selectQuery.ToQueryString();

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine(sql);
            Console.WriteLine("========================================");
            Console.WriteLine();
        }
    }
}
