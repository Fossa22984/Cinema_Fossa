using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_Models.View
{
  public  class GenreViewModel
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public bool IsRemoved { get; set; }
    }
}
