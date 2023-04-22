using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Dtos
{
    public class PagingRequestBase
    {
        public int pageIndex { set; get; }
        public int pageSize { set; get; }
    }
}