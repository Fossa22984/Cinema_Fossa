using Online_Cinema_Core.Context;
using Online_Cinema_Core.Repository;
using Online_Cinema_Core.Repository.Interface;
using System.Threading.Tasks;

namespace Online_Cinema_Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private OnlineCinemaContext _context;
        private ICinemaRoomRepository _cinemaRoom;
        private ICommentRepository _comment;
        private IGenreRepository _genre;
        private IMovieRepository _movie;
        private IRoomRepository _room;
        private ISessionRepository _session;
        private ISubscriptionRepository _subscription;

        public UnitOfWork(OnlineCinemaContext context) { _context = context; }

        public ICinemaRoomRepository CinemaRoom
        {
            get
            {
                if (_cinemaRoom == null)
                {
                    _cinemaRoom = new CinemaRoomRepository(_context);
                }
                return _cinemaRoom;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null)
                {
                    _comment = new CommentRepository(_context);
                }
                return _comment;
            }
        }

        public IGenreRepository Genre
        {
            get
            {
                if (_genre == null)
                {
                    _genre = new GenreRepository(_context);
                }
                return _genre;
            }
        }

        public IMovieRepository Movie
        {
            get
            {
                if (_movie == null)
                {
                    _movie = new MovieRepository(_context);
                }
                return _movie;
            }
        }

        public IRoomRepository Room
        {
            get
            {
                if (_room == null)
                {
                    _room = new RoomRepository(_context);
                }
                return _room;
            }
        }

        public ISessionRepository Session
        {
            get
            {
                if (_session == null)
                {
                    _session = new SessionRepository(_context);
                }
                return _session;
            }
        }

        public ISubscriptionRepository Subscription
        {
            get
            {
                if (_subscription == null)
                {
                    _subscription = new SubscriptionRepository(_context);
                }
                return _subscription;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
