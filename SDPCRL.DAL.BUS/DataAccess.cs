using SDPCRL.CORE;
using SDPCRL.DAL.DBHelp;
using SDPCRL.DAL.IDBHelp;
using SDPCRL.IBussiness;
using System;
using System.Data;

namespace SDPCRL.DAL.BUS
{
    class DataAccess : IDataAccess, ILibEventListener, IDisposable
    {
        private static DBHelpFactory _dbFactory;
        private ILibDBHelp _dbHelp;
        public DataAccess()
        {
            if (_dbFactory == null)
            {
                _dbFactory = new DBHelpFactory();
            }
            _dbHelp = _dbFactory.GetDBHelp();
        }
        public DataAccess(string guid)
            : this()
        {
            _dbHelp = _dbFactory.GetDBHelp(guid);
            LibEventManager.SubscribeEvent(new LibSqlExceptionEventSource(this, _dbHelp), LibEventType.SqlException);
        }

        public object ExecuteScalar(string commandText)
        {
            return _dbHelp.ExecuteScalar(commandText);
        }

        public DataRow GetDataRow(string commandText)
        {
            return _dbHelp.GetDataRow(commandText);
        }


        public DataTable GetDataTable(string commandText)
        {
            return _dbHelp.GetDataTable(commandText);
        }


        public int ExecuteNonQuery(string commandText)
        {
            return _dbHelp.ExecuteNonQuery(commandText);
        }

        public void Dispose()
        {

        }

        public void DoEvents(LibEventType eventType, LibEventArgs args)
        {
            switch (eventType)
            {
                case LibEventType.SqlException:

                    break;
            }
        }
    }
}
