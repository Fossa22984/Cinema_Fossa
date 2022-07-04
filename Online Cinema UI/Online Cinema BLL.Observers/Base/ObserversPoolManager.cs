using Online_Cinema_BLL.Interfaces;
using Online_Cinema_BLL.Interfaces.Observers;
using System.Collections.Generic;

namespace Online_Cinema_BLL.Observers.Base
{
    public class ObserversPoolManager : IObserversPoolManager
    {
        private readonly List<IBaseObserver> _observers;

        public ObserversPoolManager(IGetSessionObserver sessionObserver)
        {
            _observers = new List<IBaseObserver>() { sessionObserver };
        }

        public void Start()
        {
            foreach (var observer in _observers)
            {
                observer.Start();
            }
        }

        public void Stop()
        {
            foreach (var observer in _observers)
            {
                observer.Stop();
            }
        }
    }
}
