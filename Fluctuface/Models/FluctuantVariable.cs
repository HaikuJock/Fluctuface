using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluctuface.Models
{
    public class FluctuantVariable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Value { get; set; }

        private readonly FieldInfo fieldInfo;

        public FluctuantVariable()
            : this("", "", 0f, 1f, 0f)
        {
        }

        public FluctuantVariable(Fluctuant fluctuant, FieldInfo fieldInfo)
            : this(System.Guid.NewGuid().ToString(), fluctuant.Name, fluctuant.Min, fluctuant.Max, (float)fieldInfo.GetValue(null))
        {
            this.fieldInfo = fieldInfo;
        }

        public FluctuantVariable(string guid, string name, float min, float max, float value)
        {
            Id = guid;
            Name = name;
            Min = min;
            Max = max;
            Value = value;
        }
    }
}
