using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MTWDM101.Models
{

    public class Category{
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
