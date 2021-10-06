using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Repository.Interface
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        Task<IEnumerable<Room>> GetAllRoomAsync();
        Task<Room> GetRoomByIdAsync(int Id);
        Task<IEnumerable<Room>> GetRoomByConditionAsync(Expression<Func<Room, bool>> predicate);
        void CreateRoom(Room room);
        void UpdateRoom(Room room);
        void RemoveRoom(Room room);
    }
}
