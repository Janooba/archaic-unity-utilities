using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;

namespace Archaic.Core.Extensions
{
    public static class SerializationExtensions
    {
        #region Serialization Extensions
        /// <summary>
        /// Serialize the object into a string.
        /// </summary>
        public static string Serialize<T>(this T obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, obj);
                string result = textWriter.ToString();
                return result + Environment.NewLine + string.Format("<!-- Serialized on \"{0}\" -->", DateTime.Now);
            }
        }

        public static T Deserialize<T>(this XmlNode node)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new XmlNodeReader(node));
        }

        /// <summary>
        /// Deserializes string into object.
        /// </summary>
        public static T Deserialize<T>(this string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(data))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Serialize the object into a string.
        /// </summary>
        public static string ToJson<T>(this T obj)
        {
            byte[] json;
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                json = ms.ToArray();
            }
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        /// <summary>
        /// Deserializes string into object.
        /// </summary>
        public static T FromJson<T>(this string json)
        {
            T deserializedObj;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                deserializedObj = (T)ser.ReadObject(ms);
            }
            return deserializedObj;
        }
        #endregion
    }
}