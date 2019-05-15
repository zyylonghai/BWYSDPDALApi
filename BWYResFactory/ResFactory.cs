using System;
using System.Collections.Generic;
using System.Text;

namespace BWYResFactory
{
    public class ResFactory
    {
        static IResManager _resMng;
        public static IResManager ResManager
        {
            get
            {
                if (_resMng == null)
                    _resMng = new ResManager();
                return _resMng;
            }
        }
    }
}
