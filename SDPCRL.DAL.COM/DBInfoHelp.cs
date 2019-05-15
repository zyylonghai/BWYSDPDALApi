using BWYResFactory;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDPCRL.DAL.COM
{
    public class DBInfoHelp
    {
        string _filePath = string.Empty;
        Dictionary<string, string> _connectDic = null;
        public string ExceptionMessage
        {
            get;
            set;
        }
        string SysDBConnStr
        {
            get;
            set;
        }

        Dictionary<string, string> ConnectDic
        {
            get
            {
                if (_connectDic == null) _connectDic = ReadDBInfoToDic();
                return _connectDic;

            }
        }

        #region 公开属性
        public string Guid
        {
            get;
            set;
        }
        public string Key
        {
            get;
            set;
        }

        public LibProviderType ProviderType
        {
            get;
            set;
        }
        #endregion

        public DBInfoHelp()
        {
            //this._filePath = string.Format(@"{0}\{1}", Environment.CurrentDirectory, ConfigurationManager.AppSettings["DBFilePath"].ToString());
        }

        /// <summary>
        /// 获取系统账套链接字符串
        /// </summary>
        /// <returns></returns>
        public string ReadSysDBConnect()
        {
            Dictionary<string, string> dic = ReadDBInfoToDic();
            if (!string.IsNullOrEmpty(SysDBConnStr))
            {
                string[] strarray = SysDBConnStr.Split(SysConstManage.ColonChar);
                if (strarray.Length > 1)
                {
                    string connetStr = DesCryptFactory.DecryptString(strarray[1], strarray[0]);
                    this.ProviderType = DoGetProviderType(connetStr);
                    return DoGetConnect(connetStr);
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadDBConnect()
        {
            string connStr = DesCryptFactory.DecryptString(ConnectDic[Guid], Key);
            this.ProviderType = DoGetProviderType(connStr);
            return DoGetConnect(connStr);
        }

        public List<DBInfo> GetAccoutSetting()
        {
            List<DBInfo> list = new List<DBInfo>();
            DBInfo info;
            foreach (KeyValuePair<string, string> item in ConnectDic)
            {
                info = new DBInfo();
                info.Guid = item.Key;
                //info.DataBase =
            }
            return list;
        }

        /// <summary>
        /// 二进制读取数据库链接字符串
        /// </summary>
        public string BinaryReadDBConnectStr()
        {
            string content = BinaryReadDBInfo();
            if (string.IsNullOrEmpty(content)) return string.Empty;
            int index = content.IndexOf(SysConstManage.DBInfoSeparator);
            content = content.Substring(index + SysConstManage.DBInfoSeparator.Length);

            index = content.IndexOf(SysConstManage.DBInfoSeparator);
            content = content.Substring(0, index);
            content = content.Substring(content.IndexOf(SysConstManage.DBInfovalSeparator) + SysConstManage.DBInfovalSeparator.Length);
            return content;
        }
        /// <summary>
        /// 二进制读取数据库驱动类型
        /// </summary>
        /// <returns></returns>
        public LibProviderType BinaryReadProviderType()
        {
            string content = BinaryReadDBInfo();
            int index = content.IndexOf(SysConstManage.DBInfoSeparator);
            content = content.Substring(0, index);
            content = content.Substring(content.IndexOf(SysConstManage.DBInfovalSeparator) + SysConstManage.DBInfovalSeparator.Length);
            return (LibProviderType)(Convert.ToInt16(content));

        }
        /// <summary>
        /// 二进制读取数据库连接方式
        /// </summary>
        /// <returns></returns>
        public LibConnectType BinaryReadConnectType()
        {
            string content = BinaryReadDBInfo();
            int index = content.IndexOf(SysConstManage.DBInfoSeparator);
            content = content.Substring(index);
            index = content.IndexOf(SysConstManage.DBInfoSeparator);
            content = content.Substring(index + SysConstManage.DBInfoSeparator.Length);
            content = content.Substring(content.IndexOf(SysConstManage.DBInfovalSeparator) + SysConstManage.DBInfovalSeparator.Length);
            return (LibConnectType)(Convert.ToInt16(content));

        }

        /// <summary>
        /// 二进制形式存储链接字符串。
        /// </summary>
        /// <param name="dbinfo"></param>
        public void BinaryWriteDBInfo(DBInfo dbinfo)
        {
            string connectStr = string.Empty;
            switch (dbinfo.ProviderType)
            {
                case LibProviderType.SqlServer:
                    connectStr = string.Format(ResFactory.ResManager.GetResByKey(SysConstManage.SQLConnect), dbinfo.ServerAddr, dbinfo.DataBase, dbinfo.UserId, dbinfo.Password);
                    break;
                case LibProviderType.Oracle:
                    connectStr = string.Format(ResFactory.ResManager.GetResByKey(SysConstManage.OracleConnect), dbinfo.UserId, dbinfo.Password, dbinfo.ServerAddr, dbinfo.DataBase);
                    break;
            }
            string connectstr = string.Format(ResFactory.ResManager.GetResByKey(SysConstManage.SaveStr), SysConstManage.DBInfovalSeparator, (int)dbinfo.ProviderType, SysConstManage.DBInfoSeparator, connectStr, (int)dbinfo.ConnectType);
            //BinaryWriteInfo(info);
            EncryptWriteInfo(connectstr, dbinfo.Guid, dbinfo.Key, dbinfo.DataBase.Equals(ResFactory.ResManager.SysDBNm));
        }

        #region 私有函数
        /// <summary>
        /// 加密链接字符串并保存到文件
        /// </summary>
        /// <param name="info"></param>
        /// <param name="key"></param>
        private void EncryptWriteInfo(string info, string guid, string key, bool isSys)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                FileOperation file = new FileOperation();
                file.FilePath = _filePath;
                if (!File.Exists(_filePath))
                {
                    FileStream fi = File.Create(_filePath);
                    fi.Close();

                }
                string content = file.ReadFile();
                //builder.Append(content);
                builder.Append(SysConstManage.DBInfoArraySeparator);
                builder.Append(string.Format("{0}{1}", isSys ? key : guid, SysConstManage.ColonChar));
                builder.Append(DesCryptFactory.EncryptString(info, key));
                builder.Append(SysConstManage.DBInfoArraySeparator2);
                if (isSys)
                {
                    builder.Append(content);
                    file.WritText(builder.ToString());
                }
                else
                {
                    file.WritText(string.Format("{0}{1}", content, builder.ToString()));
                }
                ExceptionMessage = file.ExceptionMessage;
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
            finally
            {

            }
        }

        private Dictionary<string, string> ReadDBInfoToDic()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!File.Exists(_filePath))
            {
                ExceptionMessage = "File is not exist";
            }
            else
            {
                FileOperation file = new FileOperation();
                file.FilePath = _filePath;
                string content = file.ReadFile();
                string[] array = content.Split(SysConstManage.DBInfoArraySeparator, SysConstManage.DBInfoArraySeparator2);
                if (array.Length > 1)
                {
                    SysDBConnStr = array[1];
                    foreach (string item in array)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string[] strArray = item.Split(SysConstManage.ColonChar);
                            if (!dic.ContainsKey(strArray[0]))
                                dic.Add(strArray[0], strArray[1]);
                        }
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string DoGetConnect(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            int index = str.IndexOf(SysConstManage.DBInfoSeparator);
            str = str.Substring(index + SysConstManage.DBInfoSeparator.Length);

            index = str.IndexOf(SysConstManage.DBInfoSeparator);
            str = str.Substring(0, index);
            str = str.Substring(str.IndexOf(SysConstManage.DBInfovalSeparator) + SysConstManage.DBInfovalSeparator.Length);
            return str;
        }

        private LibProviderType DoGetProviderType(string content)
        {
            int index = content.IndexOf(SysConstManage.DBInfoSeparator);
            content = content.Substring(0, index);
            content = content.Substring(content.IndexOf(SysConstManage.DBInfovalSeparator) + SysConstManage.DBInfovalSeparator.Length);
            return (LibProviderType)(Convert.ToInt16(content));
        }

        private void BinaryWriteInfo(string info)
        {
            FileStream fs;
            BinaryWriter bWrite;
            try
            {
                fs = new FileStream(_filePath, FileMode.Create);
                bWrite = new BinaryWriter(fs);
                byte[] buff = System.Text.Encoding.ASCII.GetBytes(info);
                StringBuilder strBuilder = new StringBuilder(buff.Length * 8);
                foreach (byte b in buff)
                {
                    strBuilder.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                }
                bWrite.Write(strBuilder.ToString());
                fs.Close();
                bWrite.Close();
                ExceptionMessage = "success";
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
            finally
            {
                fs = null;
                bWrite = null;
            }


        }
        /// <summary>
        /// 二进制读取数据链接信息。
        /// </summary>
        /// <returns></returns>
        private string BinaryReadDBInfo()
        {
            FileStream fs;
            BinaryReader bReader;
            string content = string.Empty;
            if (!File.Exists(_filePath))
            {
                ExceptionMessage = "File is not exist";

            }
            else
            {
                try
                {
                    fs = new FileStream(_filePath, FileMode.Open);
                    bReader = new BinaryReader(fs);
                    content = bReader.ReadString();
                    System.Text.RegularExpressions.CaptureCollection cs = System.Text.RegularExpressions.Regex.Match(content, @"([01]{8})+").Groups[1].Captures;
                    byte[] data = new byte[cs.Count];
                    for (int i = 0; i < cs.Count; i++)
                    {
                        data[i] = Convert.ToByte(cs[i].Value, 2);
                    }
                    content = System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
                    bReader.Close();
                    fs.Close();

                }
                catch (Exception ex)
                {
                    ExceptionMessage = ex.Message;
                }
                finally
                {
                    bReader = null;
                    fs = null;
                }
            }
            return content;
        }
        #endregion
    }

    public class DBInfo
    {
        /// <summary>
        /// 数据库驱动类型
        /// </summary>
        public LibProviderType ProviderType { get; set; }
        /// <summary>
        /// 数据库链接方式
        /// </summary>
        public LibConnectType ConnectType { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerAddr { get; set; }
        /// <summary>
        /// 账套
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string Key { get; set; }
    }
    /// <summary>
    /// 数据库驱动类型枚举
    /// </summary>
    public enum LibProviderType
    {
        SqlServer = 0,
        Oracle = 1,
        MySQL = 2
    }
    /// <summary>
    /// 连接方式
    /// </summary>
    public enum LibConnectType
    {
        TCP = 0,
        HTTP = 1
    }
}
