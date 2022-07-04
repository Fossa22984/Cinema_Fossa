using System;
using System.Collections.Generic;

namespace Online_Cinema_BLL.Interfaces.Cache
{
    public interface IBaseCacheManager<TModel>
    {
        void Set(TModel model);
        void Set(List<TModel> models);
        void Update(TModel model, int index);
        void Remove(int index);
        List<TModel> GetAll();
        List<TModel> GetByCondition(Func<TModel, bool> predicate);
    }
}
