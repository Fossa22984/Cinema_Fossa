using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Settings.Interfaces
{
    public interface ISettingsManager<T>
    {
        T Get();
    }
}
