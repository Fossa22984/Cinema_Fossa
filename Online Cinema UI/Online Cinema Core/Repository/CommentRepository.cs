using Microsoft.EntityFrameworkCore;
using Online_Cinema_Core.Context;
using Online_Cinema_Core.Repository.Interface;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(OnlineCinemaContext context) : base(context) { }

        public async Task CreateComment(Comment comment)
        {
            await Create(comment);
        }

        public async Task<IEnumerable<Comment>> GetAllCommentAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentByConditionAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await FindByCondition(predicate).ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task RemoveComment(Comment comment)
        {
            await Delete(comment);
        }

        public async Task UpdateComment(Comment comment)
        {
            await Update(comment);
        }
    }
}
