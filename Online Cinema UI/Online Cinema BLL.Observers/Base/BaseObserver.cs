using Microsoft.Extensions.DependencyInjection;
using Online_Cinema_BLL.Interfaces;
using OnlineCinema_Core.Config;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Observers.Base
{
    public abstract class BaseObserver : IBaseObserver
    {
        private Thread _thread;

        protected int _timeoutMS;
        protected readonly string _name;

        protected IServiceProvider _serviceProvider;
        protected IServiceScope _scope { get; private set; }
        public bool IsStarted { get; private set; }
        public BaseObserver(IServiceProvider serviceProvider, int timeoutMs, string observerName)
        {
            _serviceProvider = serviceProvider;
            _name = observerName;
            _timeoutMS = timeoutMs;
        }

        public void Start()
        {
            if (_thread != null && _thread.IsAlive) throw new Exception($"Observer {_name} is already started");

            IsStarted = true;
            _thread = new Thread(ProcessMethod);
            _thread.IsBackground = true;
            _thread.Start();
            Log.Current.Message($"Observer process \"{_name}\" started");
        }

        public void Stop()
        {
            IsStarted = false;
            Log.Current.Message($"Observer process \"{_name}\" stopped");

            _thread?.Abort();
        }

        protected virtual async void ProcessMethod()
        {
            while (IsStarted)
            {
                try
                {
                    await Process();
                }
                catch (Exception er)
                {
                    Log.Current.Error(er, _name);
                }
                finally
                {
                    Thread.Sleep(GetTimeoutMs());
                }
            }
        }

        protected T CreateService<T>()
        {
            if (_scope == null)
            {
                _scope = _serviceProvider.CreateScope();
            }
            return _scope.ServiceProvider.GetRequiredService<T>();
        }


        protected virtual int GetTimeoutMs()
        {
            return _timeoutMS;
        }

        protected abstract Task Process();
    }
}
