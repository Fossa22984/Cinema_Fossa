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
    public class CinemaRoomRepository : RepositoryBase<CinemaRoom>, ICinemaRoomRepository
    {
        public CinemaRoomRepository(OnlineCinemaContext context) : base(context) { }

        public async Task CreateCinemaRoom(CinemaRoom cinemaRoom)
        {
            await Create(cinemaRoom);
        }

        public async Task<IEnumerable<CinemaRoom>> GetAllCinemaRoomAsync()
        {
            return await FindAll().Include(x => x.Sessions).ThenInclude(x => x.Movie).ToListAsync();
        }

        public async Task<IEnumerable<CinemaRoom>> GetCinemaRoomByConditionAsync(Expression<Func<CinemaRoom, bool>> predicate)
        {
            return await FindByCondition(predicate).Include(x => x.Sessions).ThenInclude(x => x.Movie).ToListAsync();
        }

        public async Task<CinemaRoom> GetCinemaRoomByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).AsNoTracking().Include(x => x.Sessions).ThenInclude(x => x.Movie).FirstOrDefaultAsync();
        }

        public async Task RemoveCinemaRoom(CinemaRoom cinemaRoom)
        {
            await Delete(cinemaRoom);
        }

        public async Task UpdateCinemaRoom(CinemaRoom cinemaRoom)
        {
            await Update(cinemaRoom);
        }
    }
}