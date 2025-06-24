using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Enum;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using System.Net;

namespace Smmsbe.Services
{
    public class StudentService : IStudentService
    {
        public readonly IStudentRepository _studentRepository;
        public readonly IParentRepository _parentRepository;
        public readonly IHashHelper _hashHelper;

        public StudentService(IStudentRepository studentRepository, IParentRepository parentRepository, IHashHelper hashHelper)
        {
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
            _hashHelper = hashHelper;
        }

        public async Task<Student> GetById(int id)
        {
            var student = await _studentRepository.GetById(id);
            return student;
        }

        public async Task<StudentResponse> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetAll()
                                                    .Include(x => x.Parent)
                                                    .Where(x => x.StudentId == id)
                                                    .Select(x => new StudentResponse
                                                    {
                                                        StudentId = x.StudentId,
                                                        FullName = x.FullName,
                                                        DateOfBirth = x.DateOfBirth,
                                                        ClassName = x.ClassName,
                                                        Gender = x.Gender,
                                                        StudentNumber = x.StudentNumber,
                                                        Parent = new ParentResponse
                                                        {
                                                            ParentId = x.Parent.ParentId,
                                                            FullName = x.Parent.FullName,
                                                            PhoneNumber = x.Parent.PhoneNumber,
                                                            Email = x.Parent.Email,
                                                            Address = x.Parent.Address
                                                        }
                                                    }).FirstOrDefaultAsync();

            if (student == null) throw AppExceptions.NotFoundId();

            return student;
        }

        public async Task<List<StudentResponse>> GetStudentByParent(int parentId)
        {
            return await _studentRepository.GetAll()
                                        .Where(x => x.Parent.ParentId == parentId)
                                        .Select(x => new StudentResponse
                                        {
                                            StudentId = x.StudentId,
                                            FullName = x.FullName,
                                            DateOfBirth = x.DateOfBirth,
                                            ClassName = x.ClassName,
                                            Gender = x.Gender,
                                            StudentNumber = x.StudentNumber,
                                            Parent = new ParentResponse
                                            {
                                                ParentId = x.Parent.ParentId,
                                                FullName = x.Parent.FullName,
                                                PhoneNumber = x.Parent.PhoneNumber,
                                                Email = x.Parent.Email,
                                                Address = x.Parent.Address
                                            }
                                        }).ToListAsync();
        }

        public async Task<Student> AddStudentAsync(AddStudentRequest request)
        {

            // Check if StudentNumber already exists
            var exstingAcc = await _studentRepository.GetAll().FirstOrDefaultAsync(x => x.StudentNumber == request.StudentNumber);
            if (exstingAcc != null) throw AppExceptions.BadRequestStudentNumberExists();

            var newStudentAcc = new Student
            {
                ParentId = request.ParentId,
                PasswordHash = _hashHelper.HashPassword(request.PasswordHash),
                FullName = request.FullName,
                DateOfBirth= request.DateOfBirth,
                ClassName = request.ClassName,
                Gender = request.Gender,
                StudentNumber= request.StudentNumber
            };

            return await _studentRepository.Insert(newStudentAcc);
        }

        public async Task<Student> AuthorizeAsync(string studentNumber, string password)
        {
            if (string.IsNullOrEmpty(studentNumber) || string.IsNullOrEmpty(password))
                throw AppExceptions.NotFoundAccount();

            var accNur = await _studentRepository.GetAll().SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);

            if (accNur == null) throw AppExceptions.NotFoundAccount();

            var passwordHash = _hashHelper.HashPassword(password);

            if (accNur.PasswordHash != passwordHash) throw AppExceptions.NotFoundAccount();

            return accNur;
        }

        public async Task<Student> UpdateStudentAsync(UpdateStudentRequest request)
        {
            var updateStudent = await _studentRepository.GetById(request.StudentId);
            if (updateStudent == null) throw AppExceptions.NotFoundAccount();

            updateStudent.StudentId = request.StudentId;
            updateStudent.ParentId = request.ParentId;
            updateStudent.FullName = request.FullName;
            updateStudent.ClassName = request.ClassName;
            updateStudent.DateOfBirth = request.DateOfBirth;
            updateStudent.Gender = request.Gender;
            updateStudent.StudentNumber = request.StudentNumber;

            await _studentRepository.Update(updateStudent);
            return updateStudent;
        }

        public async Task<List<StudentResponse>> SearchStudentAsync(SearchStudentRequest request)
        {
            var query = _studentRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.StudentId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.FullName) && s.FullName.Contains(request.Keyword)) ||
                            (!string.IsNullOrEmpty(s.StudentNumber) && s.StudentNumber.Contains(request.Keyword)) ||
                            (!string.IsNullOrEmpty(s.ClassName) && s.ClassName.Contains(request.Keyword)));

            var Students = await query.Select(n => new StudentResponse
            {
                StudentId = n.StudentId,
                FullName = n.FullName,
                ClassName = n.ClassName,
                DateOfBirth = n.DateOfBirth,
                Gender = n.Gender,  
                StudentNumber = n.StudentNumber,
                Parent = new ParentResponse
                {
                    ParentId = n.Parent.ParentId,
                    FullName = n.Parent.FullName,
                    PhoneNumber = n.Parent.PhoneNumber,
                    Email = n.Parent.Email,
                    Address = n.Parent.Address
                }
            }).ToListAsync();

            return Students;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                var acc = await _studentRepository.GetById(id);
                if (acc == null) throw AppExceptions.NotFoundAccount();

                await _studentRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
