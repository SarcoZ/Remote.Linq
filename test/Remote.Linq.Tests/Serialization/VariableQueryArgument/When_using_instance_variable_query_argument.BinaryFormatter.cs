﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

#if !NETCOREAPP1_0

namespace Remote.Linq.Tests.Serialization.VariableQueryArgument
{
    partial class When_using_instance_variable_query_argument
    {
        public class BinaryFormatter : When_using_instance_variable_query_argument
        {
            public BinaryFormatter()
                : base(BinarySerializationHelper.Serialize)
            {
            }
        }
    }
}

#endif