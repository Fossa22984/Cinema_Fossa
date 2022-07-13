using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task<IEnumerable<Comment>> GetAllCommentAsync();
        Task<Comment> GetCommentByIdAsync(int Id);
        Task<IEnumerable<Comment>> GetCommentByConditionAsync(Expression<Func<Comment, bool>> predicate);
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task RemoveCommentAsync(Comment comment);
    }
}
