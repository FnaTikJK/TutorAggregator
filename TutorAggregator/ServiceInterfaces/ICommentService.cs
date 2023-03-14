using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface ICommentService
    {
        public Task<CommentDTO[]>? GetTutorComments(string tutorLogin);
        public Task<CommentDTO[]>? GetStudentComments(string studentLogin);
        public Task<double?>? GetTutorRating(string tutorLogin);
        public Task<bool> TryCreateComment(string studentLogin, CommentDTO commentDto);
        public Task<bool> TryChangeComment(string studentLogin, CommentDTO commentDto);
        public Task<bool> TryDeleteComment(string tutorLogin, string studentLogin, int commentId);

    }
}
