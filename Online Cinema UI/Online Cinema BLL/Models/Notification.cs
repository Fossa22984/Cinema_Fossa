namespace Online_Cinema_BLL.Models
{
    public class Notification
    {
        public string IdUser { get; set; }
        public string IdFilm { get; set; }
        public string NameFilm { get; set; }
        public int Progress { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }

        public Notification(string idUser, string idFilm, string nameFilm, int progress = 0, NotificationTypeEnum notificationType = NotificationTypeEnum.None)
        {
            IdUser = idUser;
            IdFilm = idFilm;
            NameFilm = nameFilm;
            Progress = progress;
            NotificationType = notificationType;
        }

        public enum NotificationTypeEnum
        {
            None = 0,
            StartLoad = 1,
            SuccessfulLoad = 2
        }
    }
}
