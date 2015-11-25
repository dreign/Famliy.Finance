//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   获取系统信息
//编写日期    :   2010-12-15
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;

namespace GW.Utils.Caching
{
    /// <summary>
    /// CPU信息结构定义
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CpuInfo
    {
        /// <summary>
        /// OEM ID
        /// </summary>
        public uint dwOemId;
        /// <summary>
        /// 页面大小
        /// </summary>
        public uint dwPageSize;
        /// <summary>
        /// lpMinimumApplicationAddress
        /// </summary>
        public uint lpMinimumApplicationAddress;
        /// <summary>
        /// lpMaximumApplicationAddress
        /// </summary>
        public uint lpMaximumApplicationAddress;
        /// <summary>
        /// dwActiveProcessorMask
        /// </summary>
        public uint dwActiveProcessorMask;
        /// <summary>
        /// CPU个数
        /// </summary>
        public uint dwNumberOfProcessors;
        /// <summary>
        /// CPU类型
        /// </summary>
        public uint dwProcessorType;
        /// <summary>
        /// dwAllocationGranularity
        /// </summary>
        public uint dwAllocationGranularity;
        /// <summary>
        /// CPU等级
        /// </summary>
        public uint dwProcessorLevel;
        /// <summary>
        /// dwProcessorRevision
        /// </summary>
        public uint dwProcessorRevision;
    }

    /// <summary>
    /// 内存信息结构定义
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryInfo
    {
        /// <summary>
        /// dwLength
        /// </summary>
        public uint dwLength;
        /// <summary>
        /// 已经使用的内存
        /// </summary>
        public uint dwMemoryLoad;
        /// <summary>
        /// 总物理内存大小
        /// </summary>
        public uint dwTotalPhys;
        /// <summary>
        /// 可用物理内存大小
        /// </summary>
        public uint dwAvailPhys;
        /// <summary>
        /// 交换文件总大小
        /// </summary>
        public uint dwTotalPageFile;
        /// <summary>
        /// 可用交换文件大小
        /// </summary>
        public uint dwAvailPageFile;
        /// <summary>
        /// 总虚拟内存大小
        /// </summary>
        public uint dwTotalVirtual;
        /// <summary>
        /// 可用虚拟内存大小
        /// </summary>
        public uint dwAvailVirtual;
    }

    /// <summary>
    /// 系统时间结构定义
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTimeInfo
    {
        /// <summary>
        /// 年
        /// </summary>
        public ushort wYear;
        /// <summary>
        /// 月
        /// </summary>
        public ushort wMonth;
        /// <summary>
        /// 星期
        /// </summary>
        public ushort wDayOfWeek;
        /// <summary>
        /// 天
        /// </summary>
        public ushort wDay;
        /// <summary>
        /// 小时
        /// </summary>
        public ushort wHour;
        /// <summary>
        /// 分钟
        /// </summary>
        public ushort wMinute;
        /// <summary>
        /// 秒
        /// </summary>
        public ushort wSecond;
        /// <summary>
        /// 毫秒
        /// </summary>
        public ushort wMilliseconds;
    }

    /// <summary>
    /// 系统信息帮助类
    /// </summary>
    public static class SystemInfo
    {
        private static int CHAR_COUNT = 128;

        [DllImport("kernel32")]
        private static extern void GetWindowsDirectory(StringBuilder WinDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemDirectory(StringBuilder SysDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemInfo(ref CpuInfo cpuInfo);

        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MemoryInfo memInfo);

        [DllImport("kernel32")]
        private static extern void GetSystemTime(ref SystemTimeInfo sysInfo);

        ///// <summary>
        ///// 查询CPU编号
        ///// </summary>
        ///// <returns></returns>
        //public static string GetCpuId()
        //{
        //    ManagementClass mClass = new ManagementClass("Win32_Processor");
        //    ManagementObjectCollection moc = mClass.GetInstances();
        //    string cpuId = null;
        //    foreach (ManagementObject mo in moc)
        //    {
        //        cpuId = mo.Properties["ProcessorId"].Value.ToString();
        //        break;
        //    }
        //    return cpuId;
        //}

        ///// <summary>
        ///// 查询硬盘编号
        ///// </summary>
        ///// <returns></returns>
        //public static string GetMainHardDiskId()
        //{
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
        //    String hardDiskID = null;
        //    foreach (ManagementObject mo in searcher.Get())
        //    {
        //        hardDiskID = mo["SerialNumber"].ToString().Trim();
        //        break;
        //    }
        //    return hardDiskID;
        //}

        /// <summary>
        /// 获取Windows目录
        /// </summary>
        /// <returns>Windows目录</returns>
        public static string GetWinDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetWindowsDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取系统目录
        /// </summary>
        /// <returns>系统目录</returns>
        public static string GetSysDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetSystemDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns>CPU信息</returns>
        public static CpuInfo GetCpuInfo()
        {
            CpuInfo cpuInfo = new CpuInfo();
            GetSystemInfo(ref cpuInfo);
            return cpuInfo;
        }

        /// <summary>
        /// 获取系统内存信息
        /// </summary>
        /// <returns>系统内存信息</returns>
        public static MemoryInfo GetMemoryInfo()
        {
            MemoryInfo memoryInfo = new MemoryInfo();
            GlobalMemoryStatus(ref memoryInfo);
            return memoryInfo;
        }

        /// <summary>
        /// 获取系统时间信息
        /// </summary>
        /// <returns>系统时间信息</returns>
        public static SystemTimeInfo GetSystemTimeInfo()
        {
            SystemTimeInfo systemTimeInfo = new SystemTimeInfo();
            GetSystemTime(ref systemTimeInfo);
            return systemTimeInfo;
        }

        /// <summary>
        /// 获取系统名称
        /// </summary>
        /// <returns>系统名称</returns>
        public static string GetOperationSystemInName()
        {
            OperatingSystem os = System.Environment.OSVersion;
            string osName = "UNKNOWN";
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0: osName = "Windows 95"; break;
                        case 10: osName = "Windows 98"; break;
                        case 90: osName = "Windows ME"; break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3: osName = "Windws NT 3.51"; break;
                        case 4: osName = "Windows NT 4"; break;
                        case 5: if (os.Version.Minor == 0)
                            {
                                osName = "Windows 2000";
                            }
                            else if (os.Version.Minor == 1)
                            {
                                osName = "Windows XP";
                            }
                            else if (os.Version.Minor == 2)
                            {
                                osName = "Windows Server 2003";
                            }
                            break;
                        case 6: osName = "Longhorn"; break;
                    }
                    break;
            }
            return String.Format("{0},{1}", osName, os.Version.ToString());
        }
    }
}