using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.RequestModel
{
    public class RequestParameter
    {
        const int maxPageSize = 1000000;
        //50
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 1000000;
        //10
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string? Search { get; set; }
    }
}
