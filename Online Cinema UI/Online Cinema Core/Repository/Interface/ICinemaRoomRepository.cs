using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface ICinemaRoomRepository : IRepositoryBase<CinemaRoom>
    {
        Task<IEnumerable<CinemaRoom>> GetAllCinemaRoomAsync();
        Task<CinemaRoom> GetCinemaRoomByIdAsync(int Id);
        Task<IEnumerable<CinemaRoom>> GetCinemaRoomByConditionAsync(Expression<Func<CinemaRoom, bool>> predicate);
        void CreateCinemaRoom(CinemaRoom cinemaRoom);
        void UpdateCinemaRoom(CinemaRoom cinemaRoom);
        void RemoveCinemaRoom(CinemaRoom cinemaRoom);
    }
}
