﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq
{
    using Remote.Linq.TypeSystem;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This type is used to distinguish variable query arguments from constant query arguments
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class VariableQueryArgument
    {
        public VariableQueryArgument()
        {
        }

        public VariableQueryArgument(object value, Type type = null)
        {
            if (ReferenceEquals(null, type))
            {
                if (ReferenceEquals(null, value))
                {
                    type = typeof(object);
                }
                else
                {
                    type = value.GetType();
                }
            }

            Type = new TypeInfo(type, includePropertyInfos: false);

            Value = value;
        }

        public VariableQueryArgument(object value, TypeInfo type = null)
        {
            if (ReferenceEquals(null, type))
            {
                var valueType = ReferenceEquals(null, value) ? typeof(object) : value.GetType();
                
                type = new TypeInfo(valueType, includePropertyInfos: false);
            }

            Type = type;

            Value = value;
        }

        [DataMember(Order = 1, IsRequired = true, EmitDefaultValue = false)]
        public TypeInfo Type { get; set; }

        [DataMember(Order = 2, IsRequired = true, EmitDefaultValue = true)]
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({2}{1}{2})",
                GetType().Name,
                Value ?? "null", 
                Value is string || Value is char ? "'" : null);
        }
    }
}
