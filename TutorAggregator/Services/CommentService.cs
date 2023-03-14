using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Services
{
    public class CommentService : ICommentService
    {
        private DataContext _database;
        private IMapper _mapper;
        public CommentService(DataContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }
        public async Task<CommentDTO[]>? GetStudentComments(string studentLogin)
        {
            var student = await _database.Students.FirstOrDefaultAsync(e => e.Login == studentLogin);

            if (student == null)
                return null!;

            var comments = await _database.Comments
                .Include(e => e.Tutor)
                .Where(e => e.Student == student)
                .Select(e => _mapper.Map<CommentDTO>(e))
                .ToArrayAsync();

            return comments;

        }

        public async Task<CommentDTO[]>? GetTutorComments(string tutorLogin)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            if (tutor == null)
                return null!;

            var comments = await _database.Comments
                .Where(e => e.Tutor == tutor)
                .Include(e => e.Student)
                .Select(e => _mapper.Map<CommentDTO>(e))
                .ToArrayAsync();

            return comments;

        }

        public async Task<bool> TryCreateComment(string studentLogin, CommentDTO commentDto)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == commentDto.TutorLogin);
            var student = await _database.Students.FirstOrDefaultAsync(e => e.Login == studentLogin);
            if (tutor == null || student == null || commentDto.Score < 0
                || _database.Comments.Any(e => e.Tutor == tutor && e.Student == e.Student))
                return false;
            commentDto.StudentLogin = studentLogin;
            var comment = _mapper.Map<Comment>(commentDto);
            comment.Tutor = tutor;
            comment.Student = student;
            await _database.Comments.AddAsync(comment);
            await _database.SaveChangesAsync();

            return true;
        }
        public async Task<bool> TryChangeComment(string studentLogin, CommentDTO commentDto)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == commentDto.TutorLogin);
            var student = await _database.Students.FirstOrDefaultAsync(e => e.Login == studentLogin);
            if (tutor == null || student == null || commentDto.Score < 0)
                return false;
            var newComment = _mapper.Map<Comment>(commentDto);
            newComment.Tutor = tutor;
            newComment.Student = student;
            var comment = await _database.Comments
                .FirstOrDefaultAsync(e => e.Id == commentDto.Id);
            if (comment == null)
                return false;

            _database.Entry(comment)
                .CurrentValues.SetValues(newComment);
            await _database.SaveChangesAsync();

            return true;

        }


        public async Task<bool> TryDeleteComment(string tutorLogin, string studentLogin, int commentId)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            var student = await _database.Students.FirstOrDefaultAsync(e => e.Login == studentLogin);
            var comment = await _database.Comments
                .FirstOrDefaultAsync(e => e.Id == commentId && e.Tutor == tutor && e.Student == student);
            if (comment == null)
                return false;

            _database.Comments.Remove(comment);
            await _database.SaveChangesAsync();

            return true;

        }

        public async Task<double?>? GetTutorRating(string tutorLogin)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            if (tutor == null)
                return null!;

            return await _database.Comments
                .Where(e => e.Tutor == tutor)
                .AverageAsync(e => e.Score);
        }
    }
}
