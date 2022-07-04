using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace OnlineCinema_Core.Config
{
    public class Log : IDisposable
    {
        public static readonly Log _instance = new Log();
        public static Log Current => _instance;

        public event Action<ItemMessage> OnNewMessage;
        private ConcurrentQueue<ItemMessage> _messages = new ConcurrentQueue<ItemMessage>();
        private readonly string _rootDirectory;
        private string _path => Path.Combine(_rootDirectory, $"{DateTime.UtcNow.ToString("dd.MM.yyyy_HH")}-00.log");

        public bool IsActive { get; private set; }

        private Thread _thread;

        public void Debug(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Debug, memberName);
        }

        public Log()
        {
            _rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Main");

            if (!Directory.Exists(_rootDirectory))
                Directory.CreateDirectory(_rootDirectory);

            Start();
        }


        public void Error(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Error, memberName);
        }

        public void Error(Exception e, [CallerMemberName] string memberName = "")
        {
            if (e == null) return;

            StringBuilder sb = new StringBuilder();
            var exception = e;
            int counter = 7;

            do
            {
                sb.Append(exception.ToString() + "\r\n");
                exception = exception.InnerException;

            } while (exception != null && exception.InnerException != exception && --counter > 0);

            ChangeProperties(sb.ToString(), TypeMessage.Error, memberName);
        }

        public void Message(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Message, memberName);
        }

        public void Warning(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Warning, memberName);
        }

        public void Success(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Success, memberName);
        }

        private void ChangeProperties(string text, TypeMessage typeMessage, string memberName)
        {
            var userName = Thread.CurrentPrincipal?.Identity?.Name;
            var message = new ItemMessage
            {
                Message = text,
                MemberName = memberName,
                Type = typeMessage,
                NameUser = userName,
                Time = DateTime.UtcNow
            };

            _messages.Enqueue(message);
        }


        private void Process()
        {
            while (IsActive)
            {
                try
                {

                    ItemMessage message;
                    if (_messages.TryDequeue(out message))
                    {
                        OnNewMessage?.Invoke(message);
                        var txt = message.ToString();

                        Trace.WriteLine(txt);
                        File.AppendAllText(_path, txt);

                        continue;
                    }
                    Thread.Sleep(500);
                }
                catch
                {
                }
            }
        }



        private void Start()
        {
            if (IsActive) return;
            IsActive = true;

            _thread = new Thread(Process);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void Stop()
        {
            IsActive = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }


    public struct ItemMessage
    {
        public DateTime Time;
        public TypeMessage Type;
        public string Message;
        public string NameUser;
        public string MemberName { get; set; }

        public override string ToString()
        {
            return $"{Time:HH:mm:ss.ffff} <{NameUser}> [{Type.ToString().ToUpperInvariant()}][{MemberName}] {Message}\r\n";
        }
    }

    public enum TypeMessage
    {
        Default = 0,
        Message = 2,
        Warning = 3,
        Error = 4,
        Success = 6,
        TestMessage = 7,
        Debug = 8
    }

}
