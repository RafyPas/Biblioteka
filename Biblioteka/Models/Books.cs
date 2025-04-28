using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Book
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public DateTime? BorrowedUntil { get; set; }
        public string BorrowedBy { get; set; }
    }
}
