//ϵͳ��      :   ͨ��Web���
//-----------<�� ˵ ��>-------------------------------------------------------------------------- 
//���ܸſ�    :   ��ȡϵͳ��Ϣ
//��д����    :   2010-12-15
//-----------<�޸ļ�¼>--------------------------------------------------------------------------
//�޸�����  �޸���  �޸�����
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
    /// CPU��Ϣ�ṹ����
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CpuInfo
    {
        /// <summary>
        /// OEM ID
        /// </summary>
        public uint dwOemId;
        /// <summary>
        /// ҳ���С
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
        /// CPU����
        /// </summary>
        public uint dwNumberOfProcessors;
        /// <summary>
        /// CPU����
        /// </summary>
        public uint dwProcessorType;
        /// <summary>
        /// dwAllocationGranularity
        /// </summary>
        public uint dwAllocationGranularity;
        /// <summary>
        /// CPU�ȼ�
        /// </summary>
        public uint dwProcessorLevel;
        /// <summary>
        /// dwProcessorRevision
        /// </summary>
        public uint dwProcessorRevision;
    }

    /// <summary>
    /// �ڴ���Ϣ�ṹ����
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryInfo
    {
        /// <summary>
        /// dwLength
        /// </summary>
        public uint dwLength;
        /// <summary>
        /// �Ѿ�ʹ�õ��ڴ�
        /// </summary>
        public uint dwMemoryLoad;
        /// <summary>
        /// �������ڴ��С
        /// </summary>
        public uint dwTotalPhys;
        /// <summary>
        /// ���������ڴ��С
        /// </summary>
        public uint dwAvailPhys;
        /// <summary>
        /// �����ļ��ܴ�С
        /// </summary>
        public uint dwTotalPageFile;
        /// <summary>
        /// ���ý����ļ���С
        /// </summary>
        public uint dwAvailPageFile;
        /// <summary>
        /// �������ڴ��С
        /// </summary>
        public uint dwTotalVirtual;
        /// <summary>
        /// ���������ڴ��С
        /// </summary>
        public uint dwAvailVirtual;
    }

    /// <summary>
    /// ϵͳʱ��ṹ����
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTimeInfo
    {
        /// <summary>
        /// ��
        /// </summary>
        public ushort wYear;
        /// <summary>
        /// ��
        /// </summary>
        public ushort wMonth;
        /// <summary>
        /// ����
        /// </summary>
        public ushort wDayOfWeek;
        /// <summary>
        /// ��
        /// </summary>
        public ushort wDay;
        /// <summary>
        /// Сʱ
        /// </summary>
        public ushort wHour;
        /// <summary>
        /// ����
        /// </summary>
        public ushort wMinute;
        /// <summary>
        /// ��
        /// </summary>
        public ushort wSecond;
        /// <summary>
        /// ����
        /// </summary>
        public ushort wMilliseconds;
    }

    /// <summary>
    /// ϵͳ��Ϣ������
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
        ///// ��ѯCPU���
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
        ///// ��ѯӲ�̱��
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
        /// ��ȡWindowsĿ¼
        /// </summary>
        /// <returns>WindowsĿ¼</returns>
        public static string GetWinDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetWindowsDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// ��ȡϵͳĿ¼
        /// </summary>
        /// <returns>ϵͳĿ¼</returns>
        public static string GetSysDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetSystemDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// ��ȡCPU��Ϣ
        /// </summary>
        /// <returns>CPU��Ϣ</returns>
        public static CpuInfo GetCpuInfo()
        {
            CpuInfo cpuInfo = new CpuInfo();
            GetSystemInfo(ref cpuInfo);
            return cpuInfo;
        }

        /// <summary>
        /// ��ȡϵͳ�ڴ���Ϣ
        /// </summary>
        /// <returns>ϵͳ�ڴ���Ϣ</returns>
        public static MemoryInfo GetMemoryInfo()
        {
            MemoryInfo memoryInfo = new MemoryInfo();
            GlobalMemoryStatus(ref memoryInfo);
            return memoryInfo;
        }

        /// <summary>
        /// ��ȡϵͳʱ����Ϣ
        /// </summary>
        /// <returns>ϵͳʱ����Ϣ</returns>
        public static SystemTimeInfo GetSystemTimeInfo()
        {
            SystemTimeInfo systemTimeInfo = new SystemTimeInfo();
            GetSystemTime(ref systemTimeInfo);
            return systemTimeInfo;
        }

        /// <summary>
        /// ��ȡϵͳ����
        /// </summary>
        /// <returns>ϵͳ����</returns>
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