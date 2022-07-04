﻿using Microsoft.EntityFrameworkCore;
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

        public void CreateCinemaRoom(CinemaRoom cinemaRoom)
        {
            Create(cinemaRoom);
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
            return await FindByCondition(x => x.Id == Id).Include(x => x.Sessions).ThenInclude(x => x.Movie).FirstOrDefaultAsync();
        }

        public void RemoveCinemaRoom(CinemaRoom cinemaRoom)
        {
            Delete(cinemaRoom);
        }

        public void UpdateCinemaRoom(CinemaRoom cinemaRoom)
        {
            Update(cinemaRoom);
        }
    }
}