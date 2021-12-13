using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Interfaces
{
    public interface IDateTimeService
    {
        public DateTime NowUtc { get; }
    }
}
