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
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(OnlineCinemaContext context) : base(context) { }

        public void CreateRoom(Room room)
        {
            Create(room);
        }

        public async Task<IEnumerable<Room>> GetAllRoomAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomByConditionAsync(Expression<Func<Room, bool>> predicate)
        {
            return await FindByCondition(predicate).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public void RemoveRoom(Room room)
        {
            Delete(room);
        }

        public void UpdateRoom(Room room)
        {
            Update(room);
        }
    }
}
