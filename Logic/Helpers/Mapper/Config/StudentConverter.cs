using AutoMapper;
using DAL.Entities;
using DAL.Interfaces;

namespace Logic.Helpers.Mapper.Config
{
    internal class StudentConverter : IValueConverter<Student, string>, IValueConverter<string, Student>
    {
        private IStudentRepository studentRepository;

        public StudentConverter(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public string Convert(Student sourceMember, ResolutionContext context)
        {
            return sourceMember.Login;
        }

        public Student? Convert(string sourceMember, ResolutionContext context)
        {
            return sourceMember == null ? null 
                : studentRepository.GetByLogin(sourceMember);
        }
    }
}
