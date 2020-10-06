using System;

namespace Haiku.Fluctuface
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
