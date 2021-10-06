using Online_Cinema_Core.Repository.Interface;
using System.Threading.Tasks;

namespace Online_Cinema_Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICinemaRoomRepository CinemaRoom { get; }
        ICommentRepository Comment { get; }
        IGenreRepository Genre { get; }
        IMovieRepository Movie { get; }
        IRoomRepository Room { get; }
        ISessionRepository Session { get; }
        ISubscriptionRepository Subscription { get; }
        Task SaveAsync();
    }
}
