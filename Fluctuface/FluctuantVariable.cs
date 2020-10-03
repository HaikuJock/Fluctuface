using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluctuface
{
    public class FluctuantVariable
    {
        public string Id { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Value { get; set; }

        public FluctuantVariable()
            : this("", 0f, float.MaxValue, 0f)
        {
        }

        public FluctuantVariable(Fluctuant fluctuant, float value)
            : this(fluctuant.Id, fluctuant.Min, fluctuant.Max, value)
        {
        }

        public FluctuantVariable(string id, float min, float max, float value)
        {
            Id = id;
            Min = min;
            Max = max;
            Value = value;
        }
    }
}
