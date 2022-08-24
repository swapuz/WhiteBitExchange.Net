using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhiteBit.Net.Models.Requests
{
    public class PaginationRequest
    {
        private int? _offset;
        private int? _limit;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit">LIMIT is a special clause used to limit records a particular query can return. 
        /// Default: 50, Min: 1, Max: 100</param>
        /// <param name="offset">If you want the request to return entries starting from a particular line,
        /// you can use OFFSET clause to tell it where it should start. 
        /// Default: 0, Min: 0, Max: 10000
        /// </param>
        protected PaginationRequest(int? limit = null, int? offset = null)
        {
            Limit = limit;
            Offset = offset;
        }

        /// <summary>
        /// LIMIT is a special clause used to limit records a particular query can return. 
        /// Default: 50, Min: 1, Max: 100
        /// </summary>
        public int? Limit {
            get => _limit;
            set
            {

                _limit = value switch
                {
                    < 1 => 1,
                    > 100 => 100,
                    _ => value
                };
            }
        }

        /// <summary>
        /// If you want the request to return entries starting from a particular line,
        /// you can use OFFSET clause to tell it where it should start. 
        /// Default: 0, Min: 0, Max: 10000
        /// </summary>
        public int? Offset {
            get => _offset;
            set
            {
                _offset = value switch
                {
                    < 1 => 1,
                    > 10000 => 10000,
                    _ => value
                };
            }
        }
    }
}