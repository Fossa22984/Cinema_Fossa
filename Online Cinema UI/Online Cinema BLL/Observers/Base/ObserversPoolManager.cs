using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Observers.Base
{
    public class ObserversPoolManager
    {
        private readonly List<BaseObserver> _observers;

        public ObserversPoolManager(GetSessionObserver sessionObserver)
        {
            _observers = new List<BaseObserver>() { sessionObserver };
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
