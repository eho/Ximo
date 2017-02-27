using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ximo.Ef.EventSourcing
{
    /// <summary>
    ///     An custom implementation of <see cref="IContractResolver" />.
    /// </summary>
    internal class JsonContractResolver : DefaultContractResolver
    {
        /// <summary>
        ///     Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
        ///     <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>
        ///     A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
        ///     <see cref="T:System.Reflection.MemberInfo" />.
        /// </returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }

        /// <summary>
        ///     Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            return properties
                .Where(p =>
                    !p.PropertyName.Equals("HasValue", StringComparison.OrdinalIgnoreCase) &&
                    p.PropertyName.IndexOf("UncommittedEvents", StringComparison.OrdinalIgnoreCase) == -1)
                .ToList();
        }
    }
}