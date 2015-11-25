//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   文件导出处理类
//编写日期    :   2010-11-19
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

using GW.Utils.Web;

namespace GW.Utils
{
    /// <summary>
    /// 文件导出处理类
    /// </summary>
    public sealed class FileHelper
    {
        #region 成员变量

        /// <summary>
        /// 支持导出的文件类型
        /// </summary>
        public enum ExportFileType
        {
            /// <summary>
            /// Excel文件
            /// </summary>
            Excel,
            /// <summary>
            /// Word文件
            /// </summary>
            Word,
            /// <summary>
            /// PDF文件
            /// </summary>
            PDF,
            /// <summary>
            /// XML文件
            /// </summary>
            XML,
            /// <summary>
            /// ZIP文件
            /// </summary>
            ZIP,
            /// <summary>
            /// Txt文件
            /// </summary>
            Txt,
            /// <summary>
            /// CSV文件
            /// </summary>
            CSV
        }

        /// <summary>
        /// 文件流的头信息
        /// </summary>
        private const string CONST_REPONSEHEADER_EXCEL = "application/msexcel";
        private const string CONST_REPONSEHEADER_WORD = "application/msword";
        private const string CONST_REPONSEHEADER_PDF = "application/pdf";
        private const string CONST_REPONSEHEADER_XML = "application/xml";
        private const string CONST_REPONSEHEADER_ZIP = "application/zip";
        private const string CONST_REPONSEHEADER_TEXT = "text/plain";
        private const string CONST_REPONSEHEADER_CSV = "text/comma-separated-values";

        #endregion

        #region 文件操作

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="directoryName">目录名</param>
        /// <returns>True表示目录存在，False表示目录不存在</returns>
        public static bool IsDirectoryExist(string directoryName)
        {
            return Directory.Exists(directoryName);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName">带路径的文件名</param>
        /// <returns>True表示文件存在，False表示文件不存在</returns>
        public static bool IsFileExist(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件的扩展名</returns>
        public static string GetFileExtendName(string fileName)
        {
            string extendName = string.Empty;

            if (IsFileExist(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                extendName = fileInfo.Extension;
            }

            return extendName;
        }

        /// <summary>
        /// 获取包含路径的文件名中的文件名
        /// </summary>
        /// <param name="path">带路径的文件名</param>
        /// <returns>路径中的文件名</returns>
        public static string GetFileName(string path)
        {
            string fileName = string.Empty;

            if (IsFileExist(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                fileName = fileInfo.Name;
            }

            return fileName;
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件的大小</returns>
        public static long GetFileLength(string fileName)
        {
            long fileLength = 0;

            if (IsFileExist(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                fileLength = fileInfo.Length;
            }
            return fileLength;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件信息对象</returns>
        public static FileInfo GetFileInfo(string fileName)
        {
            FileInfo fileInfo = null;

            if (IsFileExist(fileName))
            {
                fileInfo = new FileInfo(fileName);
            }

            return fileInfo;
        }

        #endregion

        #region 文件导出操作

        /// <summary> 
        /// 将物理地址下的某文件提供给用户下载 
        /// </summary> 
        /// <param name="filePath">文件路径</param> 
        public static void ExportFile(string filePath)
        {
            ExportFile(filePath, Encoding.UTF8);
        }

        /// <summary> 
        /// 将物理地址下的某文件提供给用户下载 
        /// </summary> 
        /// <param name="filePath">文件路径</param> 
        /// <param name="encoding">输出流HTTP字符集</param> 
        public static void ExportFile(string filePath, Encoding encoding)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name, encoding));
                HttpContext.Current.Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                HttpContext.Current.Response.WriteFile(fileInfo.FullName);

                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            else
            {
                throw new Exception("文件不存在！");
            }
        }

        /// <summary>
        /// 将内容导出成UTF8的编码的Excel文件
        /// </summary>
        /// <param name="contents">需要导出的内容</param>
        /// <param name="fileName">导出的文件名</param>
        public static void ExportToExcel(string contents, string fileName)
        {
            ExportToExcel(contents, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 将内容导出成特定编码的Excel文件
        /// </summary>
        /// <param name="contents">需要导出的内容</param>
        /// <param name="fileName">导出的文件名</param>
        /// <param name="encoding">输出流HTTP字符集</param>
        public static void ExportToExcel(string contents, string fileName, Encoding encoding)
        {
            ExportToExcel(contents, fileName, ExportFileType.Excel, encoding);
        }

        /// <summary>
        /// 将内容导出成特定格式的文件
        /// </summary>
        /// <param name="contents">需要导出的内容</param>
        /// <param name="fileName">导出的文件名</param>
        /// <param name="fileType">导出的文件类型，默认为导出Excel文件</param>
        /// <param name="encoding">输出流HTTP字符集</param>
        public static void ExportToExcel(string contents, string fileName, ExportFileType fileType, Encoding encoding)
        {
            if (HttpContext.Current.Response == null) return;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = encoding.ToString();
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));

            //设置输出流HTTP字符集
            HttpContext.Current.Response.ContentEncoding = encoding;
            //设置输出文件类型 
            switch (fileType)
            {
                case ExportFileType.Excel:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_EXCEL;
                    break;
                case ExportFileType.Word:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_WORD;
                    break;
                case ExportFileType.PDF:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_PDF;
                    break;
                case ExportFileType.XML:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_XML;
                    break;
                case ExportFileType.ZIP:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_ZIP;
                    break;
                case ExportFileType.Txt:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_TEXT;
                    break;
                case ExportFileType.CSV:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_CSV;
                    break;
                default:
                    HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_EXCEL;
                    break;
            }

            HttpContext.Current.Response.Write(contents);

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 将GridView控件中的数据导出excel
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="gridView">GridView控件</param>
        public static void ExportToExcel(string fileName, GridView gridView)
        {
            if (gridView.Rows.Count != 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
                HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=UTF-8\">");

                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                HttpContext.Current.Response.ContentType = CONST_REPONSEHEADER_EXCEL;

                StringWriter writer = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(writer);

                gridView.RenderControl(htmlTextWriter);

                HttpContext.Current.Response.Output.Write(writer.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        #endregion

        #region 压缩文件操作

        /// <summary>
        /// 将传入的文件集合压缩成一个包（不适用于好压软件解压）
        /// </summary>
        /// <param name="zipfilename">压缩包的文件名</param>
        /// <param name="fileInfos">需要压缩的文件集合</param>
        public static void ZipFile(string zipfilename, List<FileInfo> fileInfos)
        {
            ZipFile(zipfilename, fileInfos, false);
        }

        /// <summary>
        ///  将传入的文件集合压缩成一个包（不适用于好压软件解压）
        /// </summary>
        /// <param name="zipfilename">压缩包的文件名</param>
        /// <param name="fileInfos">需要压缩的文件集合</param>
        /// <param name="isDeleteFiles">压缩后是否删除压缩前的所有文件</param>
        public static void ZipFile(string zipfilename, List<FileInfo> fileInfos, bool isDeleteFiles)
        {
            ZipFile(zipfilename, fileInfos, isDeleteFiles, string.Empty);
        }

        /// <summary>
        ///  将传入的文件集合压缩成一个包（不适用于好压软件解压）
        /// </summary>
        /// <param name="zipfilename">压缩包的文件名</param>
        /// <param name="fileInfos">需要压缩的文件集合</param>
        /// <param name="isDeleteFiles">压缩后是否删除压缩前的所有文件</param>
        /// <param name="password">加密密码</param>
        public static void ZipFile(string zipfilename, List<FileInfo> fileInfos, bool isDeleteFiles, string password)
        {
            //处理IO流和压缩
            ZipOutputStream s = new ZipOutputStream(File.Create(zipfilename));
            Crc32 crc = new Crc32();
            //0 - store only to 9 - means best compression
            s.SetLevel(6);

            //加密
            if (!string.IsNullOrEmpty(password))
            {
                s.Password = password;
            }

            //压缩
            try
            {
                writeStream(ref s, fileInfos, crc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                s.Finish();
                s.Close();
            }

            //删除之前的被压缩的文件
            if (isDeleteFiles)
            {
                foreach (FileInfo fi in fileInfos)
                {
                    try
                    {
                        if (fi != null)
                        {
                            fi.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 将传入的文件集合压缩成一个包（适用于所有的解压软件）
        /// </summary>
        /// <param name="dirName">目录名</param>
        /// <param name="zipFileName">压缩包的文件名</param>
        /// <param name="fileInfos">文件集合信息</param>
        public static void ZipFile(string dirName, string zipFileName, List<FileInfo> fileInfos)
        {
            //压缩前删除旧的文件及路径
            if (File.Exists(zipFileName))
            {
                File.Delete(zipFileName);
            }

            if (Directory.Exists(dirName))
            {
                Directory.Delete(dirName, true);
            }

            //重新创建文件路径
            Directory.CreateDirectory(dirName);

            foreach (FileInfo f in fileInfos)
            {
                File.Copy(f.FullName, dirName + "\\" + f.Name);
            }

            FastZip fz = new FastZip();
            fz.CreateZip(zipFileName, dirName, true, string.Empty);
        }

        /// <summary> 
        /// 解压缩文件(压缩文件中含有子目录) 
        /// </summary> 
        /// <param name="zipFilePath">待解压缩的文件路径 </param> 
        /// <param name="unZipPath">解压缩到指定目录 </param> 
        /// <returns>解压后的文件列表 </returns> 
        public List<string> UnZip(string zipFilePath, string unZipPath)
        {
            return UnZip(zipFilePath, unZipPath, string.Empty);
        }

        /// <summary> 
        /// 解压缩文件(压缩文件中含有子目录) 
        /// </summary> 
        /// <param name="zipFilePath">待解压缩的文件路径 </param> 
        /// <param name="unZipPath">解压缩到指定目录 </param> 
        /// <param name="password">解压缩密码 </param> 
        /// <returns>解压后的文件列表 </returns> 
        public List<string> UnZip(string zipFilePath, string unZipPath, string password)
        {
            //解压出来的文件列表 
            List<string> unZipFiles = new List<string>();

            //检查输出目录是否以“\\”结尾 
            if (unZipPath.EndsWith("\\") == false || unZipPath.EndsWith(":\\") == false)
            {
                unZipPath += "\\";
            }

            ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath));

            if (!string.IsNullOrEmpty(password)) s.Password = password;

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(unZipPath);
                string fileName = Path.GetFileName(theEntry.Name);

                //生成解压目录【用户解压到硬盘根目录时，不需要创建】 
                if (!string.IsNullOrEmpty(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (fileName != String.Empty)
                {
                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入 
                    if (theEntry.CompressedSize == 0)
                        break;
                    //解压文件到指定的目录 
                    directoryName = Path.GetDirectoryName(unZipPath + theEntry.Name);
                    //建立下面的目录和子目录 
                    Directory.CreateDirectory(directoryName);

                    //记录导出的文件 
                    unZipFiles.Add(unZipPath + theEntry.Name);

                    FileStream streamWriter = File.Create(unZipPath + theEntry.Name);

                    int size = 2048;
                    byte[] data = new byte[2048];

                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
            s.Close();

            return unZipFiles;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="zStream">文件流</param>
        /// <param name="fileInfos">文件集合</param>
        /// <param name="crc">Crc32</param>
        private static void writeStream(ref ZipOutputStream zStream, List<FileInfo> fileInfos, Crc32 crc)
        {
            foreach (FileInfo fi in fileInfos)
            {
                FileStream fs = fi.OpenRead();

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);

                string fileName = fi.Name;
                ZipEntry entry = new ZipEntry(fileName);
                int i = entry.Version;
                entry.DateTime = DateTime.Now;

                // set Size and the crc, because the information about the size and crc should be stored in the header
                // if it is not set it is automatically written in the footer.
                // (in this case size == crc == -1 in the header)
                // Some ZIP programs have problems with zip files that don't store the size and crc in the header.
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                zStream.PutNextEntry(entry);

                zStream.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
        
           #region File Operation Mothed
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile1(string fileName)
        {
            string result = string.Empty;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("GW.Utils.FileHelper.ReadFile error", ex);
            }
            return result;
        }
        public static string ReadFile(string fileName, out Encoding encoding)
        {
            string result = string.Empty;
            encoding = null;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        result = sr.ReadToEnd();
                        encoding = sr.CurrentEncoding;
                        sr.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("GW.Utils.FileHelper.ReadFile error", ex);
            }
            return result;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void WriteFile(string fileName, string content,Encoding encoding)
        {
            try
            {
                string path = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, encoding))
                    {
                        sw.Write(content);
                        sw.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("GW.Utils.FileHelper.WriteFile error", ex);
            }
        }
        #endregion
    }
}
