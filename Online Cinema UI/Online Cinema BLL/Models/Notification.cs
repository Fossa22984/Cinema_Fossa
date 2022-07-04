namespace Online_Cinema_BLL.Models
{
    public class Notification
    {
        public string IdUser { get; set; }
        public string IdFilm { get; set; }
        public string NameFilm { get; set; }
        public int Progress { get; set; }

        public Notification(string idUser, string idFilm, string nameFilm, int progress = 0)
        {
            IdUser = idUser;
            IdFilm = idFilm;
            NameFilm = nameFilm;
            Progress = progress;
        }
    }
}
