using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Dtos
{
    public class PageResult<T>
    {
        public List<T> Items { set; get; }
        public int TotalRecord { get; set; }
    }
}
