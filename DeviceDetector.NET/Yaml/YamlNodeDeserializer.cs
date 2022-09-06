using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Yaml
{
    public class YamlNodeDeserializer : INodeDeserializer
    {
        private readonly INodeDeserializer inner;

        public YamlNodeDeserializer(INodeDeserializer inner)
        {
            this.inner = inner;
        }

        public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            try
            {
                if (inner.Deserialize(parser, expectedType, nestedObjectDeserializer, out value))
                {
                    return true;
                }
            }
            catch (SerializationException ex)
            {
                if (expectedType == typeof(DeviceDetectorNET.Class.Os))
                {
                    if (inner.Deserialize(parser, typeof(DeviceDetectorNET.Class.Os2), nestedObjectDeserializer, out value))
                    {
                        return true;
                    }
                }
            }

            value = null;
            return false;
        }
    }
}
