using Online_Cinema_BLL.Extansions;
using OnlineCinema_Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Сache.Base
{
    public abstract class BaseCacheManager<TModel> where TModel : class
    {
        protected List<TModel> _models;
        protected object _bufferLocker;

        public BaseCacheManager()
        {
            _models = new List<TModel>();
            _bufferLocker = new object();
        }

        public void Set(TModel model)
        {
            lock (_bufferLocker)
            {
                _models.Add(model);
                Log.Current.Debug($"set new model in cache");
            }
        }

        public void Set(List<TModel> models)
        {
            lock (_bufferLocker)
            {
                models.AddRange(models);
                Log.Current.Debug($"set new models in cache");
            }
        }

        public void Update(TModel model, int index)
        {
            lock (_bufferLocker)
            {
                var updateModel = _models.ElementAtOrDefault(index);
                if (updateModel != null)
                    updateModel.Copy(model);

                Log.Current.Debug($"update model in cache");
            }
        }

        public void Remove(int index)
        {
            lock (_bufferLocker)
            {
                if (_models.ElementAtOrDefault(index) != null)
                    _models.RemoveAt(index);

                Log.Current.Debug($"Remove model from cache");
            }
        }

        public List<TModel> GetAll()
        {
            lock (_bufferLocker)
            {
                return _models;
            }
        }

        public List<TModel> GetByCondition(Func<TModel, bool> predicate)
        {
            lock (_bufferLocker)
            {
                return _models.Where(predicate).ToList();
            }
        }
    }
}
