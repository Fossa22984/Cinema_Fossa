using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema_Core.Helpers
{
    public class DefaultIconHelper
    {
        private static readonly DefaultIconHelper _instance = new DefaultIconHelper();
        public static DefaultIconHelper Current => _instance;

        public string DefaultIconPath { get; private set; } = @".\wwwroot\Images\background-fon.jpg";
    }
}
