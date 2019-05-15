using SDPCRL.DAL.COM;
using SDPCRL.DAL.IDBHelp;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace SDPCRL.DAL.DBHelp
{
    class LibDBHelp : DBhelpBase, ILibDBHelp
    {
        
        public LibDBHelp()
        {
            if (this.CurrentDBOpreate == null) this.CurrentDBOpreate = new DBOperate();
            if (this.NeedInitial)
                this.CurrentDBOpreate.Initial();
            //DBOperate.Initial();
        }
        public LibDBHelp(LibProviderType pType)
        {
            if (this.CurrentDBOpreate == null) this.CurrentDBOpreate = new DBOperate();
            if (this.NeedInitial)
                this.CurrentDBOpreate.Initial(pType);
            //DBOperate.Initial(pType);
        }
        public LibDBHelp(string guid)
        {
            this.Guid = guid;
            this.AddConnect();
            if (this.NeedInitial)
                this.CurrentDBOpreate.Initial();
        }

        public DataTable GetDataTable(string commandText)
        {
            return this.CurrentDBOpreate.GetDataTable(commandText);
            //return DBOperate.GetDataTable(commandText);
        }

        int ILibDBHelp.ExecuteNonQuery(string commandText)
        {
            return this.CurrentDBOpreate.ExecuteNonQuery(commandText);
            //return DBOperate.ExecuteNonQuery(commandText);
        }

        object ILibDBHelp.ExecuteScalar(string commandText)
        {
            return this.CurrentDBOpreate.ExecuteScalar(commandText);
            //return DBOperate.ExecuteScalar(commandText);
        }

        DataRow ILibDBHelp.GetDataRow(string commandText)
        {
            return this.CurrentDBOpreate.GetDataRow(commandText);
            //return DBOperate.GetDataRow(commandText);
        }

        bool ILibDBHelp.TestConnect(string connectStr, out string ex)
        {
            return this.CurrentDBOpreate.TestConnect(connectStr, out ex);
            //return DBOperate.TestConnect(connectStr, out ex);
        }


        public bool SaveAccout(DBInfo dbinfo)
        {
            return this.CurrentDBOpreate.SaveAccout(dbinfo);
        }
    }

    class DBOperate
    {
        private DbProviderFactory _dbProvierFactory;
        private DbConnection _dbConnect = null;
        private DbCommand _dbcmd;
        private DbDataAdapter _dbAdapter;
        private string _dbConnectStr = string.Empty;
        private DBInfoHelp _dbInfoHelp;
        private string _guid = string.Empty;

        private DBInfoHelp DBInfoHelp
        {
            get
            {
                if (_dbInfoHelp == null)
                {
                    _dbInfoHelp = new DBInfoHelp();
                    _dbInfoHelp.Guid = this._guid;
                }
                return _dbInfoHelp;
            }
        }

        #region 私有属性
        private DbConnection dbConnect
        {
            get
            {
                if (_dbConnect == null)
                {
                    _dbConnect = _dbProvierFactory.CreateConnection();
                    _dbConnect.ConnectionString = _dbConnectStr;
                    _dbConnect.Open();
                }
                else if (_dbConnect.State == System.Data.ConnectionState.Broken)
                {
                    _dbConnect.Close();
                    _dbConnect.Open();
                }
                else if (_dbConnect.State == System.Data.ConnectionState.Closed)
                {
                    _dbConnect.Open();
                }
                return _dbConnect;
            }
        }
        private DbCommand dbCommand
        {
            get
            {
                if (_dbcmd == null)
                {
                    _dbcmd = _dbProvierFactory.CreateCommand();
                    _dbcmd.Connection = dbConnect;
                }
                else
                {
                    if (_dbcmd.Connection.State == ConnectionState.Closed)
                        _dbcmd.Connection.Open();
                }
                return _dbcmd;
            }
        }
        private DbDataAdapter dbAdapter
        {
            get
            {
                if (_dbAdapter == null)
                {
                    _dbAdapter = _dbProvierFactory.CreateDataAdapter();
                }
                return _dbAdapter;
            }
        }
        #endregion

        public DBOperate(string guid)
        {
            this._guid = guid;
        }
        public DBOperate()
        { }

        /// <summary>
        /// 
        /// </summary>
        private void GetConnectStr()
        {
            //_dbConnectStr = DBInfoHelp.BinaryReadDBConnectStr();
            _dbConnectStr = DBInfoHelp.ReadSysDBConnect();
            if (!string.IsNullOrEmpty(_dbConnectStr))
            {
                _dbProvierFactory = LibDBProviderFactory.GetDbProviderFactory(DBInfoHelp.ProviderType);
                DBInfoHelp.Key = GetAccoutKey();
                if (!string.IsNullOrEmpty(DBInfoHelp.Key))
                {
                    _dbConnect = null;
                    _dbcmd = null;
                    _dbConnectStr = DBInfoHelp.ReadDBConnect();
                }
            }
        }
        public void Initial()
        {
            GetConnectStr();
            _dbProvierFactory = LibDBProviderFactory.GetDbProviderFactory(DBInfoHelp.ProviderType);
        }
        public void Initial(LibProviderType pType)
        {
            GetConnectStr();
            _dbProvierFactory = LibDBProviderFactory.GetDbProviderFactory(pType);
        }

        public System.Data.ConnectionState ConnectState
        {
            get { return this._dbConnect.State; }
        }

        #region SQL命令执行相关函数
        /// <summary>
        /// 执行sql语法返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                dbCommand.CommandText = sql;
                return dbCommand.ExecuteNonQuery();
            }
            catch (Exception excep)
            {
                //ex = excep.Message;
                return -1;
            }
            finally
            {
                CloseConnect();
            }
        }

        /// <summary>
        /// 执行sql语法返回结果集中的第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            try
            {
                dbCommand.CommandText = commandText;
                return dbCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                CloseConnect();
            }
        }
        /// <summary>
        /// 执行sql语法，返回结果集的第一行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataRow GetDataRow(string commandText)
        {
            dbCommand.CommandText = commandText;
            DataRow row = null;
            using (IDataReader reader = dbCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    DataTable dt = reader.GetSchemaTable();
                    DataTable resuldt = new DataTable();
                    foreach (DataRow dr in dt.Rows)
                    {
                        resuldt.Columns.Add(dr[0].ToString());
                        //row[col] = reader[col .ColumnName];
                    }
                    row = resuldt.NewRow();
                    foreach (DataRow dr in dt.Rows)
                    {
                        row[dr[0].ToString()] = reader[dr[0].ToString()];
                    }
                }
            }
            CloseConnect();
            return row;
        }
        public DataTable GetDataTable(string commandText)
        {
            DataTable dt = new DataTable();
            dbCommand.CommandText = commandText;
            dbAdapter.SelectCommand = dbCommand;
            dbAdapter.Fill(dt);
            CloseConnect();
            return dt;
        }

        //public object DoExecuteProcedure(string procedure)
        //{

        //}

        public bool TestConnect(string connectStr, out string ex)
        {
            try
            {
                _dbConnect = _dbProvierFactory.CreateConnection();
                _dbConnect.ConnectionString = connectStr;
                _dbConnect.Open();
                //Connect.Open();
                ex = "success";
                return true;
            }
            catch (Exception excep)
            {
                ex = excep.Message;
                return false;
            }
            finally
            {
                CloseConnect();
            }
        }

        public bool SaveAccout(DBInfo info)
        {
            string commandText = string.Format("Insert Into Accout(ID,AccoutNm,IPAddress,CreateTime,Creater,[Key]) values('{0}','{1}','{2}','{3}','{4}','{5}')",
                                               info.Guid, info.DataBase, info.ServerAddr, DateTime.Now.ToString(), "admin", info.Key);
            if (ExecuteNonQuery(commandText) != -1)
                return true;
            return false;
        }
        #endregion

        #region 私有函数
        private void CloseConnect()
        {
            if (this._dbConnect != null)
                this._dbConnect.Close();
        }
        private string GetAccoutKey()
        {
            object result = ExecuteScalar("select [key] from Accout where ID='" + _guid + "'");
            if (result != null)
                return result.ToString();
            return string.Empty;
        }
        #endregion
    }

    class DBhelpBase
    {
        static Hashtable connetTable = new Hashtable();
        static readonly object locker = new object();
        DBOperate[] db;
        DBOperate _currentDBOperate;
        int _maxConnectAmount = 3;
        protected bool NeedInitial = true;
        protected string Guid
        {
            get;
            set;
        }
        public DBhelpBase()
        {

        }
        protected void AddConnect()
        {
            DBOperate dboperate = null;
            lock (locker)
            {
                if (connetTable.ContainsKey(Guid))
                {
                    db = (DBOperate[])connetTable[Guid];
                    if (db.Length < _maxConnectAmount)
                    {
                        dboperate = new DBOperate(Guid);
                        Array.Resize(ref db, db.Length + 1);
                        db[db.Length - 1] = dboperate;
                        connetTable[Guid] = db;
                    }
                    else
                    {
                        NeedInitial = false;
                    }
                }
                else
                {
                    dboperate = new DBOperate(Guid);
                    db = new DBOperate[1];
                    db[0] = dboperate;
                    connetTable.Add(Guid, db);
                }
                _currentDBOperate = dboperate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected DBOperate CurrentDBOpreate
        {
            get
            {
                if (_currentDBOperate == null && !string.IsNullOrEmpty(Guid))
                {
                    db = (DBOperate[])connetTable[Guid];
                    foreach (DBOperate item in db)
                    {
                        if (item.ConnectState == System.Data.ConnectionState.Closed)
                        {
                            _currentDBOperate = item;
                            break;
                        }

                    }
                }
                return _currentDBOperate;
            }
            set { _currentDBOperate = value; }
        }
    }
}
