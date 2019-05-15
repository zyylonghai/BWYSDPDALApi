using SDPCRL.DAL.COM;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SDPCRL.DAL.DBHelp
{
    /// <summary>
    /// 数据源驱动工厂类
    /// </summary>
    class LibDBProviderFactory
    {
        private static Dictionary<LibProviderType, string> _providerAssemblyNameDic = new Dictionary<LibProviderType, string>();
        private static Dictionary<LibProviderType, DbProviderFactory> _providerFactoryDic = new Dictionary<LibProviderType, DbProviderFactory>();
        static LibDBProviderFactory()
        {
            _providerAssemblyNameDic.Add(LibProviderType.SqlServer, "System.Data.SqlClient");
            //_providerAssemblyNameDic.Add(LibProviderType.Oracle, "System.Data.OracleClient");
            _providerAssemblyNameDic.Add(LibProviderType.Oracle, "System.Data.OleDb");
        }

        public static DbProviderFactory GetDbProviderFactory(LibProviderType pType)
        {
            DbProviderFactory dbProviderFactory = null;
            if (!_providerFactoryDic.TryGetValue(pType, out dbProviderFactory))
            {
                dbProviderFactory = DoImporProviderFactory(pType);
                _providerFactoryDic.Add(pType, dbProviderFactory);
            }
            return dbProviderFactory;
        }
        private static DbProviderFactory DoImporProviderFactory(LibProviderType pType)
        {
            string providerAssemblyNm = _providerAssemblyNameDic[pType];
            DbProviderFactory provider = null;
            try
            {
                provider = DbProviderFactories.GetFactory(providerAssemblyNm);
            }
            catch (Exception ex)
            {
                provider = null;
            }
            finally
            {

            }
            return provider;
        }

    }
}
