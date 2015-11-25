//using System;
//using GW.CRM.Framework.Globalization;
//using GW.CRM.Framework;

//namespace GW.CRM.Framework.TypeExtension
//{
//    public static class DateTimeExtension
//    {
//        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
//        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

//        public static bool IsValid(this DateTime target)
//        {
//            return (target >= MinDate) && (target <= MaxDate);
//        }

//        public static int ToTicket(this DateTime target)
//        {
//            DateTime baseTime = new DateTime(1970, 1, 1);
//            TimeSpan ts = target.ToUniversalTime() - baseTime.ToUniversalTime();
//            return Convert.ToInt32(ts.TotalSeconds);
//        }

//        #region GetDayOfWeek

//        /// <summary>
//        /// 计算某天是一星期的第几天（从0开始），从GlobalSettings.FirstDayOfWeek取得星期的起始天。
//        /// </summary>
//        /// <param name="date">要计算星期几。</param>
//        /// <returns></returns>
//        public static int GetDayOfWeek(DateTime date)
//        {
//            return GetDayOfWeek(date, GlobalSettings.FirstDayOfWeek);
//        }

//        /// <summary>
//        /// 计算某天是一星期的第几天（从0开始），指定哪天是一星期的第一天。
//        /// </summary>
//        /// <param name="date">要计算星期几。</param>
//        /// <param name="firstDayOfWeek">指定这一天是一星期的第一天。</param>
//        /// <returns></returns>
//        public static int GetDayOfWeek(DateTime date, DayOfWeek firstDayOfWeek)
//        {
//            int intDay = (int)date.DayOfWeek;
//            int intFirstDayOfWeek = (int)firstDayOfWeek;

//            if (intDay >= intFirstDayOfWeek)
//            {
//                return intDay - intFirstDayOfWeek;
//            }
//            return intDay + (7 - intFirstDayOfWeek);
//        }

//        #endregion

//        #region GetWeek

//        /// <summary>
//        /// 取得指定日期所属的周别，每周第一天从GlobalSettings.FirstDayOfWeek取得，第一周规则从
//        /// GlobalSettings.FirstWeekOfYear取得。
//        /// </summary>
//        /// <param name="date">指定的日期</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Microsoft.VisualBasic.DateAndTime.DatePart和System.Globalization.
//        /// Calendar.GetWeekOfYear方法可以实现同样功能
//        /// </remarks>
//        public static int GetWeek(DateTime date)
//        {
//            return GetWeek(date, GlobalSettings.FirstDayOfWeek);
//        }

//        /// <summary>
//        /// 取得指定日期所属的周别，指定每周的第一天是哪一天，第一周规则从
//        /// GlobalSettings.FirstWeekOfYear取得。
//        /// </summary>
//        /// <param name="date">指定的日期</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Microsoft.VisualBasic.DateAndTime.DatePart和System.Globalization.
//        /// Calendar.GetWeekOfYear方法可以实现同样功能
//        /// </remarks>
//        public static int GetWeek(DateTime date, DayOfWeek firstDayOfWeek)
//        {
//            return GetWeek(date, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// 取得指定日期所属的周别，指定每周的第一天是哪一天和第一周的规则。
//        /// </summary>
//        /// <param name="date">指定的日期</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <param name="firstWeekOfYear">定义确定年份第一周的不同规则。</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Microsoft.VisualBasic.DateAndTime.DatePart和System.Globalization.
//        /// Calendar.GetWeekOfYear方法可以实现同样功能
//        /// </remarks>
//        public static int GetWeek(DateTime date, DayOfWeek firstDayOfWeek, CalendarWeekRule firstWeekOfYear)
//        {
//            System.Globalization.CalendarWeekRule weekRule;
//            switch (firstWeekOfYear)
//            {
//                case CalendarWeekRule.FirstDay:
//                    weekRule = System.Globalization.CalendarWeekRule.FirstDay;
//                    break;
//                case CalendarWeekRule.FirstFourDayWeek:
//                    weekRule = System.Globalization.CalendarWeekRule.FirstFourDayWeek;
//                    break;
//                case CalendarWeekRule.FirstFullWeek:
//                    weekRule = System.Globalization.CalendarWeekRule.FirstFullWeek;
//                    break;
//                case CalendarWeekRule.FirstDayAndMore:
//                    weekRule = System.Globalization.CalendarWeekRule.FirstDay;
//                    break;
//                default:
//                    weekRule = System.Globalization.CalendarWeekRule.FirstDay;
//                    break;
//            }

//            int week = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);
//            return week;
//        }

//        #endregion

//        #region GetFirstDateOfTheWeek

//        /// <summary>
//        /// 取得本年指定周的第一天的日期，每周第一天从GlobalSettings.FirstDayOfWeek取得。
//        /// </summary>
//        /// <param name="week">指定的周</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int week)
//        {
//            DateTime date = GetFirstDateOfTheWeek(DateTime.Today.Year, week);
//            return date;
//        }

//        /// <summary>
//        /// 取得指定年指定周的第一天的日期，每周第一天从GlobalSettings.FirstDayOfWeek取得。
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">指定的周</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int year, int week)
//        {
//            DateTime date = GetFirstDateOfTheWeek(year, week, GlobalSettings.FirstDayOfWeek);
//            return date;
//        }

//        /// <summary>
//        /// 取得指定年指定周的第一天的日期，指定每周第一天是哪一天。 
//        /// </summary>
//        /// <param name="week">The week.</param>
//        /// <param name="year">The year.</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek)
//        {
//            DateTime date = GetFirstDateOfTheWeek(year, week, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//            return date;
//        }

//        /// <summary>
//        /// 取得指定年指定周的第一天的日期，指定每周第一天是哪一天。 
//        /// </summary>
//        /// <param name="week">The week.</param>
//        /// <param name="year">The year.</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <param name="firstWeekOfYear">定义确定年份第一周的不同规则。</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek,
//                                                     CalendarWeekRule firstWeekOfYear)
//        {
//            int addDays;
//            //取得第一天
//            DateTime firstDay = new DateTime(year, 1, 1);
//            //判断第一天是否属于第一周
//            int weekOfFirstDay = GetWeek(firstDay, firstDayOfWeek, firstWeekOfYear);
//            if ((weekOfFirstDay == 1) && (firstDay.DayOfWeek == firstDayOfWeek))
//            {
//                //第一天属于第一周,并且第一天是一星期的第一天
//                addDays = (week - 1) * 7;
//            }
//            else
//            {
//                //取得第一天是一星期的第几天
//                int dayOfWeek = GetDayOfWeek(firstDay, firstDayOfWeek);

//                if (weekOfFirstDay == 1)
//                {
//                    //第一天属于第一周,第一天不是一星期的第一天
//                    if (week == 1)
//                    {
//                        if (firstWeekOfYear == CalendarWeekRule.FirstDayAndMore)
//                        {
//                            //如果要查找第1天所在周的第一天
//                            addDays = 0 - dayOfWeek;
//                        }
//                        else
//                        {
//                            //如果要查找第一周则返回第一天
//                            addDays = 0;
//                        }
//                    }
//                    else
//                    {
//                        addDays = (week - 1) * 7 - dayOfWeek;
//                    }
//                }
//                else
//                {
//                    //第一天不属于第一周
//                    //取得第一周的第一天，公式为(7-dayOfWeek)+(week - 1)*7，简化如下
//                    addDays = week * 7 - dayOfWeek;
//                }
//            }
//            return firstDay.AddDays(addDays);
//        }

//        #endregion

//        #region GetLastDateOfTheWeek

//        /// <summary>
//        /// 取得周第一天所在周的最后一天日期。
//        /// </summary>
//        /// <param name="firstDate">要找的第一天</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(DateTime firstDate)
//        {
//            return GetLastDateOfTheWeek(firstDate, GlobalSettings.FirstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// 取得周第一天所在周的最后一天日期。
//        /// </summary>
//        /// <param name="firstDate">要找的第一天</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(DateTime firstDate, DayOfWeek firstDayOfWeek)
//        {
//            return GetLastDateOfTheWeek(firstDate, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// 取得周第一天所在周的最后一天日期。
//        /// </summary>
//        /// <param name="firstDate">要找的第一天</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <param name="firstWeekOfYear">定义确定年份第一周的不同规则。</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(DateTime firstDate, DayOfWeek firstDayOfWeek, CalendarWeekRule firstWeekOfYear)
//        {
//            int week = GetWeek(firstDate, firstDayOfWeek, firstWeekOfYear);
//            DateTime result = firstDate.AddDays(6);
//            int nextWeek = GetWeek(result, firstDayOfWeek, firstWeekOfYear);
//            //仍在同一周，则为最后一天
//            if (week == nextWeek)
//            {
//                return result;
//            }
//            //仍然在同一年，则取nextWeek第一天减1即最后一天
//            if (nextWeek > week)
//            {
//                result = GetFirstDateOfTheWeek(result.Year, nextWeek, firstDayOfWeek, firstWeekOfYear);
//                return result.AddDays(-1);
//            }
//            //不在同一年，则为下一年，nextWeek值必为1
//            if (firstWeekOfYear == CalendarWeekRule.FirstDayAndMore)
//            {
//                //取得下一年第2周并减一天
//                firstWeekOfYear = CalendarWeekRule.FirstDay;
//                result = GetFirstDateOfTheWeek(result.Year, nextWeek + 1, firstDayOfWeek, firstWeekOfYear);
//            }
//            else
//            {
//                //取得下年第一周第一天减1
//                result = GetFirstDateOfTheWeek(result.Year, nextWeek, firstDayOfWeek, firstWeekOfYear);
//            }
//            return result.AddDays(-1);
//        }

//        /// <summary>
//        /// 取得周第一天所在周的最后一天日期。
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">The week.</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(int year, int week)
//        {
//            return GetLastDateOfTheWeek(year, week, GlobalSettings.FirstDayOfWeek);
//        }

//        /// <summary>
//        /// 取得周第一天所在周的最后一天日期。
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">The week.</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek)
//        {
//            return GetLastDateOfTheWeek(year, week, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// 取得周第一天所在周的最后一天日期。
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">The week.</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <param name="firstWeekOfYear">定义确定年份第一周的不同规则。</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek,
//            CalendarWeekRule firstWeekOfYear)
//        {
//            DateTime firstDate = GetFirstDateOfTheWeek(year, week, firstDayOfWeek, firstWeekOfYear);
//            return GetLastDateOfTheWeek(firstDate, firstDayOfWeek, firstWeekOfYear);
//        }

//        #endregion

//        #region GetWeeksOfTheYear

//        /// <summary>
//        /// 获取某年周数
//        /// </summary>
//        /// <param name="year">指定年份(如:2006)</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <returns>该年周数</returns>
//        public static int GetWeeksOfTheYear(int year, DayOfWeek firstDayOfWeek)
//        {
//            DateTime lastDate = new DateTime(year, 12, 31);
//            return GetWeek(lastDate, firstDayOfWeek);
//        }

//        /// <summary>
//        /// 获取某年周数
//        /// </summary>
//        /// <param name="year">指定年份(如:2006)</param>
//        /// <param name="firstDayOfWeek">每周的第一天</param>
//        /// <param name="firstWeekOfYear">定义确定年份第一周的不同规则。</param>
//        /// <returns>该年周数</returns>
//        public static int GetWeeksOfTheYear(int year, DayOfWeek firstDayOfWeek, CalendarWeekRule firstWeekOfYear)
//        {
//            DateTime lastDate = new DateTime(year, 12, 31);
//            return GetWeek(lastDate, firstDayOfWeek, firstWeekOfYear);
//        }

//        /// <summary>
//        /// 获取某年周数
//        /// </summary>
//        /// <param name="year">指定年份(如:2006)</param>
//        /// <returns>该年周数</returns>
//        public static int GetWeeksOfTheYear(int year)
//        {
//            DateTime lastDate = new DateTime(year, 12, 31);
//            return GetWeek(lastDate);
//        }

//        #endregion

//        #region RebuildDateScope

//        /// <summary>
//        /// 重建起讫时间，起始时间将变为所在天的00:00:00，结束时间将变为所在天的23:59:59.999
//        /// </summary>
//        /// <param name="beginDate">The begin date.</param>
//        /// <param name="endDate">The end date.</param>
//        public static void RebuildDateScope(ref DateTime beginDate, ref DateTime endDate)
//        {
//            beginDate = beginDate.Date;
//            endDate = endDate.Date.AddDays(1).AddMilliseconds(-1);
//        }

//        /// <summary>
//        /// 重建起讫时间，起始时间将变为所在天的00:00:00，结束时间将变为所在天的23:59:59.999
//        /// </summary>
//        /// <param name="beginDate">The begin date.</param>
//        /// <param name="endDate">The end date.</param>
//        public static void RebuildDateScope(ref DateTime? beginDate, ref DateTime? endDate)
//        {
//            if (Checker.AllAreNotEmpty(beginDate, endDate))
//            {
//                DateTime tempBeginDate = beginDate.Value;
//                DateTime tempEndDate = endDate.Value;

//                RebuildDateScope(ref tempBeginDate, ref tempEndDate);

//                beginDate = tempBeginDate;
//                endDate = tempEndDate;
//            }
//        }

//        #endregion
//    }
//}