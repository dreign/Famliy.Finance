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
//        /// ����ĳ����һ���ڵĵڼ��죨��0��ʼ������GlobalSettings.FirstDayOfWeekȡ�����ڵ���ʼ�졣
//        /// </summary>
//        /// <param name="date">Ҫ�������ڼ���</param>
//        /// <returns></returns>
//        public static int GetDayOfWeek(DateTime date)
//        {
//            return GetDayOfWeek(date, GlobalSettings.FirstDayOfWeek);
//        }

//        /// <summary>
//        /// ����ĳ����һ���ڵĵڼ��죨��0��ʼ����ָ��������һ���ڵĵ�һ�졣
//        /// </summary>
//        /// <param name="date">Ҫ�������ڼ���</param>
//        /// <param name="firstDayOfWeek">ָ����һ����һ���ڵĵ�һ�졣</param>
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
//        /// ȡ��ָ�������������ܱ�ÿ�ܵ�һ���GlobalSettings.FirstDayOfWeekȡ�ã���һ�ܹ����
//        /// GlobalSettings.FirstWeekOfYearȡ�á�
//        /// </summary>
//        /// <param name="date">ָ��������</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Microsoft.VisualBasic.DateAndTime.DatePart��System.Globalization.
//        /// Calendar.GetWeekOfYear��������ʵ��ͬ������
//        /// </remarks>
//        public static int GetWeek(DateTime date)
//        {
//            return GetWeek(date, GlobalSettings.FirstDayOfWeek);
//        }

//        /// <summary>
//        /// ȡ��ָ�������������ܱ�ָ��ÿ�ܵĵ�һ������һ�죬��һ�ܹ����
//        /// GlobalSettings.FirstWeekOfYearȡ�á�
//        /// </summary>
//        /// <param name="date">ָ��������</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Microsoft.VisualBasic.DateAndTime.DatePart��System.Globalization.
//        /// Calendar.GetWeekOfYear��������ʵ��ͬ������
//        /// </remarks>
//        public static int GetWeek(DateTime date, DayOfWeek firstDayOfWeek)
//        {
//            return GetWeek(date, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// ȡ��ָ�������������ܱ�ָ��ÿ�ܵĵ�һ������һ��͵�һ�ܵĹ���
//        /// </summary>
//        /// <param name="date">ָ��������</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <param name="firstWeekOfYear">����ȷ����ݵ�һ�ܵĲ�ͬ����</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Microsoft.VisualBasic.DateAndTime.DatePart��System.Globalization.
//        /// Calendar.GetWeekOfYear��������ʵ��ͬ������
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
//        /// ȡ�ñ���ָ���ܵĵ�һ������ڣ�ÿ�ܵ�һ���GlobalSettings.FirstDayOfWeekȡ�á�
//        /// </summary>
//        /// <param name="week">ָ������</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int week)
//        {
//            DateTime date = GetFirstDateOfTheWeek(DateTime.Today.Year, week);
//            return date;
//        }

//        /// <summary>
//        /// ȡ��ָ����ָ���ܵĵ�һ������ڣ�ÿ�ܵ�һ���GlobalSettings.FirstDayOfWeekȡ�á�
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">ָ������</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int year, int week)
//        {
//            DateTime date = GetFirstDateOfTheWeek(year, week, GlobalSettings.FirstDayOfWeek);
//            return date;
//        }

//        /// <summary>
//        /// ȡ��ָ����ָ���ܵĵ�һ������ڣ�ָ��ÿ�ܵ�һ������һ�졣 
//        /// </summary>
//        /// <param name="week">The week.</param>
//        /// <param name="year">The year.</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek)
//        {
//            DateTime date = GetFirstDateOfTheWeek(year, week, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//            return date;
//        }

//        /// <summary>
//        /// ȡ��ָ����ָ���ܵĵ�һ������ڣ�ָ��ÿ�ܵ�һ������һ�졣 
//        /// </summary>
//        /// <param name="week">The week.</param>
//        /// <param name="year">The year.</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <param name="firstWeekOfYear">����ȷ����ݵ�һ�ܵĲ�ͬ����</param>
//        /// <returns></returns>
//        public static DateTime GetFirstDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek,
//                                                     CalendarWeekRule firstWeekOfYear)
//        {
//            int addDays;
//            //ȡ�õ�һ��
//            DateTime firstDay = new DateTime(year, 1, 1);
//            //�жϵ�һ���Ƿ����ڵ�һ��
//            int weekOfFirstDay = GetWeek(firstDay, firstDayOfWeek, firstWeekOfYear);
//            if ((weekOfFirstDay == 1) && (firstDay.DayOfWeek == firstDayOfWeek))
//            {
//                //��һ�����ڵ�һ��,���ҵ�һ����һ���ڵĵ�һ��
//                addDays = (week - 1) * 7;
//            }
//            else
//            {
//                //ȡ�õ�һ����һ���ڵĵڼ���
//                int dayOfWeek = GetDayOfWeek(firstDay, firstDayOfWeek);

//                if (weekOfFirstDay == 1)
//                {
//                    //��һ�����ڵ�һ��,��һ�첻��һ���ڵĵ�һ��
//                    if (week == 1)
//                    {
//                        if (firstWeekOfYear == CalendarWeekRule.FirstDayAndMore)
//                        {
//                            //���Ҫ���ҵ�1�������ܵĵ�һ��
//                            addDays = 0 - dayOfWeek;
//                        }
//                        else
//                        {
//                            //���Ҫ���ҵ�һ���򷵻ص�һ��
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
//                    //��һ�첻���ڵ�һ��
//                    //ȡ�õ�һ�ܵĵ�һ�죬��ʽΪ(7-dayOfWeek)+(week - 1)*7��������
//                    addDays = week * 7 - dayOfWeek;
//                }
//            }
//            return firstDay.AddDays(addDays);
//        }

//        #endregion

//        #region GetLastDateOfTheWeek

//        /// <summary>
//        /// ȡ���ܵ�һ�������ܵ����һ�����ڡ�
//        /// </summary>
//        /// <param name="firstDate">Ҫ�ҵĵ�һ��</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(DateTime firstDate)
//        {
//            return GetLastDateOfTheWeek(firstDate, GlobalSettings.FirstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// ȡ���ܵ�һ�������ܵ����һ�����ڡ�
//        /// </summary>
//        /// <param name="firstDate">Ҫ�ҵĵ�һ��</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(DateTime firstDate, DayOfWeek firstDayOfWeek)
//        {
//            return GetLastDateOfTheWeek(firstDate, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// ȡ���ܵ�һ�������ܵ����һ�����ڡ�
//        /// </summary>
//        /// <param name="firstDate">Ҫ�ҵĵ�һ��</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <param name="firstWeekOfYear">����ȷ����ݵ�һ�ܵĲ�ͬ����</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(DateTime firstDate, DayOfWeek firstDayOfWeek, CalendarWeekRule firstWeekOfYear)
//        {
//            int week = GetWeek(firstDate, firstDayOfWeek, firstWeekOfYear);
//            DateTime result = firstDate.AddDays(6);
//            int nextWeek = GetWeek(result, firstDayOfWeek, firstWeekOfYear);
//            //����ͬһ�ܣ���Ϊ���һ��
//            if (week == nextWeek)
//            {
//                return result;
//            }
//            //��Ȼ��ͬһ�꣬��ȡnextWeek��һ���1�����һ��
//            if (nextWeek > week)
//            {
//                result = GetFirstDateOfTheWeek(result.Year, nextWeek, firstDayOfWeek, firstWeekOfYear);
//                return result.AddDays(-1);
//            }
//            //����ͬһ�꣬��Ϊ��һ�꣬nextWeekֵ��Ϊ1
//            if (firstWeekOfYear == CalendarWeekRule.FirstDayAndMore)
//            {
//                //ȡ����һ���2�ܲ���һ��
//                firstWeekOfYear = CalendarWeekRule.FirstDay;
//                result = GetFirstDateOfTheWeek(result.Year, nextWeek + 1, firstDayOfWeek, firstWeekOfYear);
//            }
//            else
//            {
//                //ȡ�������һ�ܵ�һ���1
//                result = GetFirstDateOfTheWeek(result.Year, nextWeek, firstDayOfWeek, firstWeekOfYear);
//            }
//            return result.AddDays(-1);
//        }

//        /// <summary>
//        /// ȡ���ܵ�һ�������ܵ����һ�����ڡ�
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">The week.</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(int year, int week)
//        {
//            return GetLastDateOfTheWeek(year, week, GlobalSettings.FirstDayOfWeek);
//        }

//        /// <summary>
//        /// ȡ���ܵ�һ�������ܵ����һ�����ڡ�
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">The week.</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <returns></returns>
//        public static DateTime GetLastDateOfTheWeek(int year, int week, DayOfWeek firstDayOfWeek)
//        {
//            return GetLastDateOfTheWeek(year, week, firstDayOfWeek, GlobalSettings.FirstWeekOfYear);
//        }

//        /// <summary>
//        /// ȡ���ܵ�һ�������ܵ����һ�����ڡ�
//        /// </summary>
//        /// <param name="year">The year.</param>
//        /// <param name="week">The week.</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <param name="firstWeekOfYear">����ȷ����ݵ�һ�ܵĲ�ͬ����</param>
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
//        /// ��ȡĳ������
//        /// </summary>
//        /// <param name="year">ָ�����(��:2006)</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <returns>��������</returns>
//        public static int GetWeeksOfTheYear(int year, DayOfWeek firstDayOfWeek)
//        {
//            DateTime lastDate = new DateTime(year, 12, 31);
//            return GetWeek(lastDate, firstDayOfWeek);
//        }

//        /// <summary>
//        /// ��ȡĳ������
//        /// </summary>
//        /// <param name="year">ָ�����(��:2006)</param>
//        /// <param name="firstDayOfWeek">ÿ�ܵĵ�һ��</param>
//        /// <param name="firstWeekOfYear">����ȷ����ݵ�һ�ܵĲ�ͬ����</param>
//        /// <returns>��������</returns>
//        public static int GetWeeksOfTheYear(int year, DayOfWeek firstDayOfWeek, CalendarWeekRule firstWeekOfYear)
//        {
//            DateTime lastDate = new DateTime(year, 12, 31);
//            return GetWeek(lastDate, firstDayOfWeek, firstWeekOfYear);
//        }

//        /// <summary>
//        /// ��ȡĳ������
//        /// </summary>
//        /// <param name="year">ָ�����(��:2006)</param>
//        /// <returns>��������</returns>
//        public static int GetWeeksOfTheYear(int year)
//        {
//            DateTime lastDate = new DateTime(year, 12, 31);
//            return GetWeek(lastDate);
//        }

//        #endregion

//        #region RebuildDateScope

//        /// <summary>
//        /// �ؽ�����ʱ�䣬��ʼʱ�佫��Ϊ�������00:00:00������ʱ�佫��Ϊ�������23:59:59.999
//        /// </summary>
//        /// <param name="beginDate">The begin date.</param>
//        /// <param name="endDate">The end date.</param>
//        public static void RebuildDateScope(ref DateTime beginDate, ref DateTime endDate)
//        {
//            beginDate = beginDate.Date;
//            endDate = endDate.Date.AddDays(1).AddMilliseconds(-1);
//        }

//        /// <summary>
//        /// �ؽ�����ʱ�䣬��ʼʱ�佫��Ϊ�������00:00:00������ʱ�佫��Ϊ�������23:59:59.999
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