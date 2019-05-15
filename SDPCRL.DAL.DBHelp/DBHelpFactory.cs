using SDPCRL.DAL.COM;
using SDPCRL.DAL.IDBHelp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.DAL.DBHelp
{
    public class DBHelpFactory :IDBHelpFactory
    {
        //ILibDBHelp _d
        public ILibDBHelp GetDBHelp()
        {
            return new LibDBHelp();
        }

        public ILibDBHelp GetDBHelp(LibProviderType providerType)
        {
            return new LibDBHelp(providerType);
        }


        public ILibDBHelp GetDBHelp(string guid)
        {
            return new LibDBHelp(guid);
        }
    }
}
