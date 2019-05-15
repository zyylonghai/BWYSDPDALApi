using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDPCRL.CORE.FileUtils
{
    /// <summary> </summary>
    public class FileOperation
    {
        #region 私有变量
        private string _filePath;
        private LibEncoding _Encoding;
        #endregion

        #region 共有属性
        /// <summary>文件路径</summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public LibEncoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        public string ExceptionMessage
        {
            get; set;
        }
        #endregion

        #region 共有方法

        /// <summary>读取文件</summary>
        /// <returns></returns>
        public string ReadFile()
        {
            //string _context = string.Empty;

            //try
            //{
            //    _context = File.ReadAllText(_filePath, getEncoding());
            //}
            //catch (Exception ex)
            //{
            //    ExceptionMessage = ex.Message;
            //}
            //return _context;
            return DoRead(_filePath);
        }

        public bool WritText(string context)
        {
            try
            {
                File.WriteAllText(_filePath, context, getEncoding());
                return true;
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
                return false;
            }
        }
        public bool ExistsFile()
        {
            return File.Exists(_filePath);
        }

        public bool IsDirectory
        {
            get
            {
                return Directory.Exists(_filePath);
            }
        }

        public string SearchAndRead(string filename)
        {
            string filepath = string.Empty;
            string fileContent = string.Empty;
            if (IsDirectory)
            {

                string[] dirpath = Directory.GetDirectories(_filePath);
                foreach (string path in dirpath)
                {
                    filepath = string.Format(@"{0}\{1}", path, filename);
                    if (File.Exists(filepath))
                    {
                        fileContent = DoRead(filepath);
                        break;
                    }
                }
                return fileContent;

            }
            else
            {
                return string.Empty;
            }
        }
        public void CreateFile(bool cover)
        {
            FileStream stream = null;
            if (cover)
                stream = File.Create(_filePath);
            else
            {
                if (ExistsFile())
                {
                    throw new LibExceptionBase("文件已经存在");
                }
                else
                {
                    stream = File.Create(_filePath);
                }
            }
            if (stream != null)
                stream.Close();
        }
        //public string 
        #endregion

        #region 私有方法
        private System.Text.Encoding getEncoding()
        {
            System.Text.Encoding encode = System.Text.Encoding.UTF8;
            switch (_Encoding)
            {
                case LibEncoding.ASCII:
                    encode = System.Text.Encoding.ASCII;
                    break;
                case LibEncoding.Unicode:
                    encode = System.Text.Encoding.Unicode;
                    break;
                case LibEncoding.UTF32:
                    encode = System.Text.Encoding.UTF32;
                    break;
                case LibEncoding.UTF7:
                    encode = System.Text.Encoding.UTF7;
                    break;
                case LibEncoding.UTF8:
                    encode = System.Text.Encoding.UTF8;
                    break;
                case LibEncoding.Default:
                    encode = System.Text.Encoding.Default;
                    break;
            }
            return encode;
        }

        private string DoRead(string filePath)
        {
            string _context = string.Empty;

            try
            {
                _context = File.ReadAllText(filePath, getEncoding());
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
            return _context;
        }
        #endregion

    }

    public enum LibEncoding
    {
        Default = 0,
        UTF8 = 1,
        UTF7 = 2,
        UTF32 = 3,
        Unicode = 4,
        ASCII = 5
    }
}
