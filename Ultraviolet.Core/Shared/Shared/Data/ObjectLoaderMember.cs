using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a field or property which can be populated by the object loader's XML serializer.
    /// </summary>
    internal class ObjectLoaderMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectLoaderMember"/> class.
        /// </summary>
        /// <param name="obj">The object on which the member exists.</param>
        /// <param name="member">The member metadata.</param>
        private ObjectLoaderMember(Object obj, MemberInfo member)
        {
            this.obj = obj;
            this.member = member;

            this.isField = (member.MemberType == MemberTypes.Field);
            this.isProperty = (member.MemberType == MemberTypes.Property);
            this.indexParameters = isProperty ? ((PropertyInfo)member).GetIndexParameters() : null;
        }

        /// <summary>
        /// Attempts to find a member with the specified name on the specified object.
        /// </summary>
        /// <param name="obj">The object on which to find the member.</param>
        /// <param name="name">The name of the member to attempt to find.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether to ignore members which do not exist on the type.</param>
        /// <returns>The member that was found.</returns>
        public static ObjectLoaderMember Find(Object obj, String name, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(obj, nameof(obj));
            Contract.Require(name, nameof(name));

            var member = obj.GetType().GetMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            if (member == null)
            {
                if (ignoreMissingMembers)
                {
                    return null;
                }
                throw new MissingMemberException();
            }

            switch (member.MemberType)
            {
                case MemberTypes.Property:
                case MemberTypes.Field:
                    return new ObjectLoaderMember(obj, member);
            }
            throw new MissingMemberException();
        }

        /// <summary>
        /// Sets the member's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="element">The data element that defined the value.</param>
        public void SetValueFromData(Object value, XElement element)
        {
            if (IsIndexer)
            {
                Contract.Require(element, nameof(element));
                SetValue(value, GetIndexParameters(element));
            }
            else
            {
                SetValue(value, null);
            }
        }

        /// <summary>
        /// Sets the member's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="index">Optional index values for indexed properties.</param>
        public void SetValue(Object value, Object[] index = null)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    {
                        var field = (FieldInfo)member;
                        field.SetValue(obj, value);
                        return;
                    }
                case MemberTypes.Property:
                    {
                        var property = (PropertyInfo)member;
                        if (!property.CanWrite)
                        {
                            throw new InvalidOperationException(CoreStrings.DataObjectPropertyIsReadOnly.Format(member.Name));
                        }
                        property.SetValue(obj, value, index);
                        return;
                    }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the member's current value.
        /// </summary>
        /// <param name="element">The data element that defined the value.</param>
        /// <returns>The member's current value.</returns>
        public Object GetValueFromData(XElement element)
        {
            if (IsIndexer)
            {
                Contract.Require(element, nameof(element));

                return GetValue(GetIndexParameters(element));
            }
            else
            {
                return GetValue(null);
            }
        }

        /// <summary>
        /// Gets the member's current value.
        /// </summary>
        /// <param name="index">Optional index values for indexed properties.</param>
        /// <returns>The member's current value.</returns>
        public Object GetValue(Object[] index = null)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field: return ((FieldInfo)member).GetValue(obj);
                case MemberTypes.Property: return ((PropertyInfo)member).GetValue(obj, index);
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets a value indicating whether this member is a field.
        /// </summary>
        public Boolean IsField
        {
            get { return isField; }
        }

        /// <summary>
        /// Gets a value indicating whether this member is an indexer.
        /// </summary>
        public Boolean IsProperty
        {
            get { return isProperty; }
        }

        /// <summary>
        /// Gets a value indicating whether this member is an indexer.
        /// </summary>
        public Boolean IsIndexer
        {
            get { return this.indexParameters != null && this.indexParameters.Length > 0; }
        }

        /// <summary>
        /// Gets the member's return type.
        /// </summary>
        public Type MemberType
        {
            get
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Property: return ((PropertyInfo)member).PropertyType;
                    case MemberTypes.Field: return ((FieldInfo)member).FieldType;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets a set of index parameters from the specified data element.
        /// </summary>
        /// <param name="element">The data element from which to retrieve index parameters.</param>
        /// <returns>The index parameters that were retrieved.</returns>
        private Object[] GetIndexParameters(XElement element)
        {
            var indexParameterValues = new Object[indexParameters.Length];

            for (int i = 0; i < indexParameters.Length; i++)
            {
                var indexParameter = indexParameters[i];

                var attr = element.Attribute(indexParameter.Name);
                if (attr == null)
                    throw new InvalidOperationException(CoreStrings.DataObjectMissingIndexParam.Format(indexParameter.Name));

                indexParameterValues[i] = ObjectResolver.FromString(attr.Value, indexParameter.ParameterType);
            }

            if (element.Attributes().Where(x => !ObjectLoaderXmlSerializer.IsReservedKeyword(x.Name.LocalName)).Count() > indexParameterValues.Length)
                throw new InvalidOperationException(CoreStrings.DataObjectHasTooManyIndexParams);

            return indexParameterValues;
        }

        // The member info for this member.
        private readonly Object obj;
        private readonly MemberInfo member;

        // Property values.
        private Boolean isField;
        private Boolean isProperty;
        private ParameterInfo[] indexParameters;
    }
}
