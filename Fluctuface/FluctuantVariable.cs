﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluctuface
{
    public class FluctuantVariable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Value { get; set; }

        private FieldInfo fieldInfo;


        public FluctuantVariable()
            : this("", "", 0f, 1f, 0f)
        {
        }

        public FluctuantVariable(Fluctuant fluctuant, float value)
            : this(System.Guid.NewGuid().ToString(), fluctuant.Name, fluctuant.Min, fluctuant.Max, value)
        {
        }

        public FluctuantVariable(string id, string name, float min, float max, float value)
        {
            Id = id;
            Name = name;
            Min = min;
            Max = max;
            Value = value;
        }
    }
}