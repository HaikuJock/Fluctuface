using System;
using System.Collections.Generic;
using System.Text;

namespace Fluctuface
{

    [AttributeUsage(AttributeTargets.Assembly)]
    public class FluctuantAssembly : Attribute
    {
        public FluctuantAssembly()
            : base()
        {
        }
    }
}
