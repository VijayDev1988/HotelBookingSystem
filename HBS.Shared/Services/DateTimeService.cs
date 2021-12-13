using HBS.Application.Interfaces;
using System;

namespace HBS.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
