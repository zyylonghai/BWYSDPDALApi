using System;
using System.Data;

namespace SDPCRL.DAL.IDBHelp
{
    public interface ILibDBHelp
    {
        /// <summary>执行sql语法返回受影响的行数</summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string commandText);
        /// <summary>
        /// 执行sql语法返回结果集中的第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        object ExecuteScalar(string commandText);
        /// <summary>
        /// 执行sql语法，返回结果集的第一行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataRow GetDataRow(string commandText);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetDataTable(string commandText);
        //object ExecuteProcedure(string procedureNm,
        bool TestConnect(string connectStr, out string ex);
        bool SaveAccout(SDPCRL.DAL.COM.DBInfo dbinfo);
    }
}
