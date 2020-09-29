using System;

namespace Fluctuface
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Fluctuant : Attribute
    {
        public readonly string Name;
        public readonly float Min;
        public readonly float Max;

        public Fluctuant(string name, float min, float max)
        {
            Name = name;
            Min = min;
            Max = max;
        }
    }
}
