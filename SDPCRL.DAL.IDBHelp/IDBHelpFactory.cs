using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.DAL.IDBHelp
{
    public interface IDBHelpFactory
    {
        ILibDBHelp GetDBHelp();
        ILibDBHelp GetDBHelp(string guid);
    }
}
