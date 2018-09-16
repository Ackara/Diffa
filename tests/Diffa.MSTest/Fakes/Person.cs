using System.Xml.Serialization;

namespace Acklann.Diffa.Fakes
{
    [XmlRoot("person")]
    public class Person
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("age")]
        public int Age { get; set; }
    }
}