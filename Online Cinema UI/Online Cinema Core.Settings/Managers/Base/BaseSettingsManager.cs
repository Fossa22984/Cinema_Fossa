using Newtonsoft.Json;
using Online_Cinema_Core.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_Core.Settings.Managers.Base
{
    public class BaseSettingsManager<T> : ISettingsManager<T>
    {
        protected T _settings;
        protected object _loadFileLocker = new object();
        protected string _filename;
        protected DateTime? _lastChange;

        protected readonly string _path;
        public BaseSettingsManager(string filename)
        {
            _filename = filename;
            var root = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            _path = Path.Combine(root, "Configuration", filename);
        }


        public virtual T Get()
        {
            lock (_loadFileLocker)
            {
                if (_settings != null && CheckSettingChanges(_path) == false)
                {
                    return _settings;
                }
                else
                {
                    var json = File.ReadAllText(_path);
                    _settings = JsonConvert.DeserializeObject<T>(json);
                }
                return _settings;
            }
        }

        protected bool CheckSettingChanges(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Configuration '{path}' not found");
            }

            var time = File.GetLastWriteTimeUtc(path);

            if (_lastChange != time)
            {
                _lastChange = time;
                return true;
            }

            return false;
        }

    }
}
