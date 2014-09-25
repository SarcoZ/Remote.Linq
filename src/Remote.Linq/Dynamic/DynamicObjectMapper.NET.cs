﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Remote.Linq.Dynamic
{
    partial class DynamicObjectMapper
    {
        /// <summary>
        /// .NET platform specific regex options
        /// </summary>
        private const RegexOptions LocalRegexOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline;

        /// <summary>
        /// Gets an uninitialized instance of the specified type by using <see cref="FormatterServices" />
        /// </summary>
        private static object GetUninitializedObject(Type type)
        {
            return FormatterServices.GetUninitializedObject(type);
        }

        /// <summary>
        /// Populate object members type by using <see cref="FormatterServices" />
        /// </summary>
        private void PopulateObjectMembers(Type type, DynamicObject from, object to)
        {
            var members = FormatterServices.GetSerializableMembers(type);
            var values = from
                .Select((x, i) =>
                {
                    var member = members[i];
                    Type memberType;
                    switch (member.MemberType)
                    {
                        case System.Reflection.MemberTypes.Field:
                            memberType = ((System.Reflection.FieldInfo)member).FieldType;
                            break;
                        case System.Reflection.MemberTypes.Property:
                            memberType = ((System.Reflection.PropertyInfo)member).PropertyType;
                            break;
                        default:
                            throw new Exception(string.Format("Unsupported member type {0}.", member.MemberType));
                    }
                    return MapFromDynamicObjectGraph(x.Value, memberType);
                })
                .ToArray();
            FormatterServices.PopulateObjectMembers(to, members, values);
        }

        /// <summary>
        /// Retrieves object members type by using <see cref="FormatterServices" /> and populates dynamic object
        /// </summary>
        private void MapObjectMembers(object from, DynamicObject to)
        {
            var type = to.Type.Type;

            var members = FormatterServices.GetSerializableMembers(type);
            var values = FormatterServices.GetObjectData(from, members);
            for (int i = 0; i < members.Length; i++)
            {
                var memberName = CleanName(members[i].Name);
                var value = MapToDynamicObjectIfRequired(values[i]);
                to[memberName] = value;
            }
        }

        private static string CleanName(string memberName)
        {
            var match = _backingFieldRegex.Match(memberName);
            if (match.Success && match.Groups.Count == 2)
            {
                memberName = match.Groups[1].Value;
            }
            return memberName;
        }
    }
}