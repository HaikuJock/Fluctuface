using System;

namespace Haiku.Fluctuface
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Fluctuant : Attribute
    {
        public readonly string Id;
        public readonly float Min;
        public readonly float Max;

        public Fluctuant(string id, float min, float max)
        {
            Id = id;
            Min = min;
            Max = max;
        }
    }
}
