using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.Extensions;

namespace RedisStudio
{
    public static class DateTimeExtensions
    {
        //public static bool IsDaylightSavingsTime(this DateTimeOffset dateTimeOffset)
        //{
        //    var timezone = "Europe/Rome"; //https://nodatime.org/TimeZones
        //    ZonedDateTime timeInZone = dateTimeOffset.DateTime.InZone(timezone);
        //    var instant = timeInZone.ToInstant();
        //    var zoneInterval = timeInZone.Zone.GetZoneInterval(instant);
        //    return zoneInterval.Savings != Offset.Zero;
        //}
    }
}
