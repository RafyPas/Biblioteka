using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public static class Session
    {
        public static string CurrentUser { get; set; }
        public static string CurrentUserEmail { get; set; }
    }
}
