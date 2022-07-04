using Online_Cinema_BLL.Interfaces.Cache;
using OnlineCinema_Core.Config;
using OnlineCinema_Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Online_Cinema_BLL.Cache.Base
{
    public abstract class BaseCacheManager<TModel> : IBaseCacheManager<TModel>
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
                _models.AddRange(models);
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
