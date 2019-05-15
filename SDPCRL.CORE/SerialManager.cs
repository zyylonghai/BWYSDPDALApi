using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.CORE
{
    /// <summary>编码管理类</summary>
    class SerialManager
    {

    }

    /// <summary>系统常量管理</summary>
    public class SysConstManage
    {
        /// <summary>数据源列表文件路径 </summary>
        public static readonly string DSListFile = string.Format(@"{0}\Models\DSList.xml", Environment.CurrentDirectory);
        /// <summary>数据源文件根目录</summary>
        public static readonly string DSFileRootPath = string.Format(@"{0}\Models\DataSource", Environment.CurrentDirectory);
        /// <summary>账套信息文件</summary>
        //public static readonly string DBInfoFilePath = string.Format(@"{0}\Runtime\DBInfo.xml", Environment.CurrentDirectory);
        public static readonly string DBFilePath = string.Format(@"{0}\Runtime\DBInfo.bin", Environment.CurrentDirectory);
        /// <summary>模型列表文件</summary>
        public static readonly string ModelTemp = string.Format(@"{0}\Runtime\ModelTreeTemp.xml", Environment.CurrentDirectory);
        public static readonly string ModelPath = string.Format(@"{0}\Models", Environment.CurrentDirectory);
        //DAL程序集所在路径
        public static readonly string DALAssemblyPath = string.Format(@"{0}\Runtime\DAL", Environment.CurrentDirectory);
        //客户端存储的服务端信息文件路径
        public static readonly string ServerConfigPath = string.Format(@"{0}\Runtime\ServerInfo.bin", Environment.CurrentDirectory);
        #region 特殊字符
        public const string DBInfoSeparator = "&&";
        public const string DBInfovalSeparator = "::";
        public const char DBInfoArraySeparator = '[';
        public const char DBInfoArraySeparator2 = ']';
        public const char ColonChar = ':';
        public const char Underline = '_';
        public const char Comma = ',';
        public const char Asterisk = '*';
        #endregion

        public const string SQLConnect = "SQLSERVERCONN";
        public const string OracleConnect = "ORACLECONN";
        public const string SaveStr = "SaveStr";

        #region xml操作常量
        public const string ClassNodeNm = "Class";
        public const string FuncNodeNm = "Func";
        public const string AtrrName = "name";
        public const string AtrrPackage = "package";
        #endregion

        #region 模型存储 所在路径的 文件夹名
        public const string DataSourceNm = "DataSource";
        public const string FormSourceNm = "FormSource";
        public const string PermissionSourceNm = "PermissionSource";
        #endregion

        public const string BtnCtrlNmPrefix = "btn_";
        public const string BtnCtrlDefaultText = "...";


        #region BWYSDPWeb
        public const string PageinfoCookieNm = "PageInfo";
        public const string ProgidCookieKey = "Progid";
        public const string PackageCookieKey = "Package";

        public const string OperateAction = "Action";

        public const string ExtProp = "extProp";
        #endregion
    }

    /// <summary>编码管理类接口</summary>
    public interface ISerialManager
    {

    }
}
