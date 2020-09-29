using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluctuface.Models
{
    public class FluctuantVariable
    {
        private string _id;
        public string Id { 
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (fluctuantFields.ContainsKey(_id))
                {
                    fieldInfo = fluctuantFields[_id];
                    fieldInfo.SetValue(null, _value);
                }
            }
        }
        public string Name { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        private float _value;
        public float Value
        {
            get
            {
                if (fieldInfo != null)
                {
                    return (float)fieldInfo.GetValue(null);
                }
                return _value;
            }

            set
            {
                _value = value;
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(null, _value);
                }
            }
        }

        private FieldInfo fieldInfo;

        static Dictionary<string, FieldInfo> fluctuantFields = new Dictionary<string, FieldInfo>();

        public FluctuantVariable()
            : this("", "", 0f, 1f, 0f)
        {
        }

        public FluctuantVariable(Fluctuant fluctuant, FieldInfo fieldInfo)
            : this(System.Guid.NewGuid().ToString(), fluctuant.Name, fluctuant.Min, fluctuant.Max, (float)fieldInfo.GetValue(null))
        {
            this.fieldInfo = fieldInfo;
            fluctuantFields[Id] = fieldInfo;
        }

        public FluctuantVariable(string id, string name, float min, float max, float value)
        {
            Id = id;
            Name = name;
            Min = min;
            Max = max;
            Value = value;
            if (fluctuantFields.ContainsKey(Id))
            {
                fieldInfo = fluctuantFields[Id];
            }
        }
    }
}
