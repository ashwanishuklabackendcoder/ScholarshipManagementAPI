using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.Data.Contexts;
using ScholarshipManagementAPI.Data.DbModels;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.School.StudentRegistration;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.School;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScholarshipManagementAPI.Services.Implementation.School
{
    public class StudentRegistrationService : IStudentRegistrationService
    {
        private readonly AppDbContext _context;

        public StudentRegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(StudentRegistrationRequestDto dto)
        {
            var photoPath = await UploadFileAsync(dto.Photo, "photos", new[] { ".jpg", ".jpeg", ".png" });
            var recPath = await UploadFileAsync(dto.Recommendation, "recommendations", new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" });

            var entity = new StudentRegistration
            {
                PhotoPath = photoPath,
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                ThirdName = dto.ThirdName,
                LastName = dto.LastName,
                MotherName = dto.MotherName,
                Dob = dto.Dob.HasValue ? DateOnly.FromDateTime(dto.Dob.Value) : null,
                Nationality = dto.Nationality,
                ResidenceCountry = dto.ResidenceCountryId,
              
                Tribe = dto.Tribe,
                Religion = dto.Religion,
                Gender = dto.Gender,
                IsOrphan = dto.IsOrphan,
                OrphanNumber = dto.OrphanNumber,

                City = dto.City,
                Village = dto.Village,
                Block = dto.Block,
                Street = dto.Street,
                House = dto.House,
                Phone = dto.Phone,
                Email = dto.Email,

                FromDaSchool = dto.FromDaSchool == true ? 1 : 0,
                DaStudentCode = dto.DaStudentCode,
                SchoolName = dto.SchoolName,
                HsSpecialization = dto.HsSpecialization,
                CombinedSpec = dto.CombinedSpec,
                TotalScore = dto.TotalScore,
                MaxScore = dto.MaxScore,
                RelativeGrade = dto.RelativeGrade,
                EnglishScore = dto.EnglishScore,

                TransferInstitution = dto.TransferInstitution,
                TransferProgram = dto.TransferProgram,
                TransferInstitutionType = dto.TransferInstitutionType,
                TransferCredits = dto.TransferCredits,
                TransferLastSemEnd = dto.TransferLastSemEnd.HasValue ? DateOnly.FromDateTime(dto.TransferLastSemEnd.Value) : null,
                TransferGpa = dto.TransferGpa,

                FinancialNeed = dto.FinancialNeed,
                SelfReliance = dto.SelfReliance,
                Motivation = dto.Motivation,
                FutureGoals = dto.FutureGoals,
                RecommendationLetterPath = recPath,
                RecommendationLetterNotes = dto.RecommendationLetterNotes,

                IsDraft = dto.IsDraft,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.StudentRegistrations.Add(entity);
            await _context.SaveChangesAsync();

            return entity.StudentId;
        }

        public async Task<bool> UpdateAsync(StudentRegistrationRequestDto dto)
        {
            if (dto.StudentId == null || dto.StudentId == 0)
                return false;

            var entity = await _context.StudentRegistrations
                .FirstOrDefaultAsync(x => x.StudentId == dto.StudentId);

            if (entity == null)
                return false;

            if (dto.Photo != null)
            {
                entity.PhotoPath = await UploadFileAsync(dto.Photo, "photos", new[] { ".jpg", ".jpeg", ".png" });
            }
            if (dto.Recommendation != null)
            {
                entity.RecommendationLetterPath = await UploadFileAsync(dto.Recommendation, "recommendations", new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" });
            }

            entity.FirstName = dto.FirstName;
            entity.SecondName = dto.SecondName;
            entity.ThirdName = dto.ThirdName;
            entity.LastName = dto.LastName;
            entity.MotherName = dto.MotherName;
            entity.Dob = dto.Dob.HasValue ? DateOnly.FromDateTime(dto.Dob.Value) : null;
            entity.Nationality = dto.Nationality;
            entity.ResidenceCountry = dto.ResidenceCountryId;
            entity.Tribe = dto.Tribe;
            entity.Religion = dto.Religion;
            entity.Gender = dto.Gender;
            entity.IsOrphan = dto.IsOrphan;
            entity.OrphanNumber = dto.OrphanNumber;

            entity.City = dto.City;
            entity.Village = dto.Village;
            entity.Block = dto.Block;
            entity.Street = dto.Street;
            entity.House = dto.House;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;

            entity.FromDaSchool = dto.FromDaSchool == true ? 1 : 0;
            entity.DaStudentCode = dto.DaStudentCode;
            entity.SchoolName = dto.SchoolName;
            entity.HsSpecialization = dto.HsSpecialization;
            entity.CombinedSpec = dto.CombinedSpec;
            entity.TotalScore = dto.TotalScore;
            entity.MaxScore = dto.MaxScore;
            entity.RelativeGrade = dto.RelativeGrade;
            entity.EnglishScore = dto.EnglishScore;

            entity.TransferInstitution = dto.TransferInstitution;
            entity.TransferProgram = dto.TransferProgram;
            entity.TransferInstitutionType = dto.TransferInstitutionType;
            entity.TransferCredits = dto.TransferCredits;
            entity.TransferLastSemEnd = dto.TransferLastSemEnd.HasValue ? DateOnly.FromDateTime(dto.TransferLastSemEnd.Value) : null;
            entity.TransferGpa = dto.TransferGpa;

            entity.FinancialNeed = dto.FinancialNeed;
            entity.SelfReliance = dto.SelfReliance;
            entity.Motivation = dto.Motivation;
            entity.FutureGoals = dto.FutureGoals;
            entity.RecommendationLetterNotes = dto.RecommendationLetterNotes;

            entity.IsDraft = dto.IsDraft;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.StudentRegistrations
                .FirstOrDefaultAsync(x => x.StudentId == id);

            if (entity == null)
                return false;

            entity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentRegistrationResponseDto?> GetByIdAsync(long id)
        {
            return await _context.StudentRegistrations
                .AsNoTracking()
                .Where(x => x.StudentId == id && x.IsActive)
                .Select(x => new StudentRegistrationResponseDto
                {
                    StudentId = x.StudentId,
                    PhotoPath = x.PhotoPath,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    ThirdName = x.ThirdName,
                    LastName = x.LastName,
                    MotherName = x.MotherName,
                    Dob = x.Dob.HasValue ? x.Dob.Value.ToDateTime(TimeOnly.MinValue) : null,
                    Nationality = x.Nationality,
                    ResidenceCountry = x.ResidenceCountry,
                 
                    Tribe = x.Tribe,
                    Religion = x.Religion,
                    Gender = x.Gender,
                    IsOrphan = x.IsOrphan,
                    OrphanNumber = x.OrphanNumber,
                    City = x.City,
                    Village = x.Village,
                    Block = x.Block,
                    Street = x.Street,
                    House = x.House,
                    Phone = x.Phone,
                    Email = x.Email,
                    FromDaSchool = x.FromDaSchool == 1,
                    DaStudentCode = x.DaStudentCode,
                    SchoolName = x.SchoolName,
                    HsSpecialization = x.HsSpecialization,
                    CombinedSpec = x.CombinedSpec,
                    TotalScore = x.TotalScore,
                    MaxScore = x.MaxScore,
                    RelativeGrade = x.RelativeGrade,
                    EnglishScore = x.EnglishScore,
                    TransferInstitution = x.TransferInstitution,
                    TransferProgram = x.TransferProgram,
                    TransferInstitutionType = x.TransferInstitutionType,
                    TransferCredits = x.TransferCredits,
                    TransferLastSemEnd = x.TransferLastSemEnd.HasValue ? x.TransferLastSemEnd.Value.ToDateTime(TimeOnly.MinValue) : null,
                    TransferGpa = x.TransferGpa,
                    FinancialNeed = x.FinancialNeed,
                    SelfReliance = x.SelfReliance,
                    Motivation = x.Motivation,
                    FutureGoals = x.FutureGoals,
                    RecommendationLetterPath = x.RecommendationLetterPath,
                    RecommendationLetterNotes = x.RecommendationLetterNotes,
                    IsDraft = x.IsDraft,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResultDto<StudentRegistrationResponseDto>> GetByFilterAsync(StudentRegistrationFilterDto filter)
        {
            var query = _context.StudentRegistrations
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (filter.IsDraft.HasValue)
                query = query.Where(x => x.IsDraft == filter.IsDraft.Value);

            if (!string.IsNullOrWhiteSpace(filter.Gender))
                query = query.Where(x => x.Gender == filter.Gender);

            if (!string.IsNullOrWhiteSpace(filter.SchoolName))
                query = query.Where(x => x.SchoolName != null && x.SchoolName.Contains(filter.SchoolName));

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLower();
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(search) ||
                    x.LastName.ToLower().Contains(search) ||
                    (x.Email != null && x.Email.ToLower().Contains(search)) ||
                    (x.Phone != null && x.Phone.Contains(search))
                );
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(x => x.CreatedDate);

            if (filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var items = await query
                .Select(x => new StudentRegistrationResponseDto
                {
                    StudentId = x.StudentId,
                    PhotoPath = x.PhotoPath,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    ThirdName = x.ThirdName,
                    LastName = x.LastName,
                    MotherName = x.MotherName,
                    Dob = x.Dob.HasValue ? x.Dob.Value.ToDateTime(TimeOnly.MinValue) : null,
                    Nationality = x.Nationality,
                    ResidenceCountry = x.ResidenceCountry,
                    Tribe = x.Tribe,
                    Religion = x.Religion,
                    Gender = x.Gender,
                    IsOrphan = x.IsOrphan,
                    OrphanNumber = x.OrphanNumber,
                    City = x.City,
                    Village = x.Village,
                    Block = x.Block,
                    Street = x.Street,
                    House = x.House,
                    Phone = x.Phone,
                    Email = x.Email,
                    FromDaSchool = x.FromDaSchool == 1,
                    DaStudentCode = x.DaStudentCode,
                    SchoolName = x.SchoolName,
                    HsSpecialization = x.HsSpecialization,
                    CombinedSpec = x.CombinedSpec,
                    TotalScore = x.TotalScore,
                    MaxScore = x.MaxScore,
                    RelativeGrade = x.RelativeGrade,
                    EnglishScore = x.EnglishScore,
                    TransferInstitution = x.TransferInstitution,
                    TransferProgram = x.TransferProgram,
                    TransferInstitutionType = x.TransferInstitutionType,
                    TransferCredits = x.TransferCredits,
                    TransferLastSemEnd = x.TransferLastSemEnd.HasValue ? x.TransferLastSemEnd.Value.ToDateTime(TimeOnly.MinValue) : null,
                    TransferGpa = x.TransferGpa,
                    FinancialNeed = x.FinancialNeed,
                    SelfReliance = x.SelfReliance,
                    Motivation = x.Motivation,
                    FutureGoals = x.FutureGoals,
                    RecommendationLetterPath = x.RecommendationLetterPath,
                    RecommendationLetterNotes = x.RecommendationLetterNotes,
                    IsDraft = x.IsDraft,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<StudentRegistrationResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        private async Task<string?> UploadFileAsync(IFormFile? file, string subFolder, string[] allowedExtensions)
        {
            if (file == null) return null;
            if (file.Length == 0) throw new CustomException("Uploaded file is empty.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
            {
                throw new CustomException($"Invalid file type. Only {string.Join("/", allowedExtensions)} are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                throw new CustomException("File size must be less than 5MB.");
            }

            var cleanName = Path.GetFileName(file.FileName).Replace(" ", "_");
            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}_{cleanName}";
            var directory = Path.Combine("wwwroot", "uploads", "student-registrations", subFolder);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fullPath = Path.Combine(directory, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/student-registrations/{subFolder}/{fileName}";
        }
    }
}
