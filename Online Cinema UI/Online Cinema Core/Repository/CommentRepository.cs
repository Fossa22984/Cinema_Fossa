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

        public async Task CreateCommentAsync(Comment comment)
        {
            await CreateAsync(comment);
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

        public async Task RemoveCommentAsync(Comment comment)
        {
            await DeleteAsync(comment);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            await UpdateAsync(comment);
        }
    }
}
