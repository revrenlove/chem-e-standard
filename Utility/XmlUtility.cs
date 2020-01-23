using System.Xml;
using System.Xml.Serialization;

namespace EFC.AgGateway.Integration.Utility
{
    public static class XmlUtility
    {
        public static XmlElement Serialize<T>(T o)
            where T : class
        {
            XmlDocument doc = new XmlDocument();

            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(typeof(T)).Serialize(writer, o);
            }

            return doc.DocumentElement;
        }

        public static XmlElement Serialize(string s)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            return doc.DocumentElement;
        }

        public static T Deserialize<T>(XmlNode xmlNode)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T));

            return Deserialize<T>(xmlNode, serializer);
        }

        public static T Deserialize<T>(XmlNode xmlNode, XmlRootAttribute xmlRoot)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T), xmlRoot);

            return Deserialize<T>(xmlNode, serializer);
        }

        public static T Deserialize<T>(XmlNode xmlNode, XmlSerializer serializer)
            where T : class
        {
            using (var xmlNodeReader = new XmlNodeReader(xmlNode))
            {
                return (T)serializer.Deserialize(xmlNodeReader);
            }
        }
    }
}
