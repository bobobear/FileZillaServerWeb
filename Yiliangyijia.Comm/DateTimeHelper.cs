﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiliangyijia.Comm
{
    public class DateTimeHelper
    {
        /// <summary>    
        /// Unix时间戳转为C#格式时间    
        /// </summary>    
        /// <param name="timeStamp">Unix时间戳格式,例如1482115779</param>    
        /// <returns>C#格式时间</returns>    
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }


        /// <summary>    
        /// DateTime时间格式转换为Unix时间戳格式    
        /// </summary>    
        /// <param name="time"> DateTime时间格式</param>    
        /// <returns>Unix时间戳格式</returns>    
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public static bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
