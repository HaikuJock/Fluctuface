using System;

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
