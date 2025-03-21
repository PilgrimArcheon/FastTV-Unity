using System.IO;
using System.Xml.Serialization;

public static class Helper
{
    // Serialize
    public static string Serialize<T>(this T toSerialize)
    {
        // Create an instance of XmlSerializer for the type T
        XmlSerializer xml = new XmlSerializer(typeof(T));

        // Create a StringWriter to hold the serialized XML
        StringWriter writer = new StringWriter();

        // Serialize the object to the StringWriter
        xml.Serialize(writer, toSerialize);

        // Return the serialized XML as a string
        return writer.ToString();
    }

    // De-Serialize 
    public static T Deserialize<T>(this string toDeSerialize)
    {
        // Create an instance of XmlSerializer for the type T
        XmlSerializer xml = new XmlSerializer(typeof(T));

        // Create a StringReader to read the XML string
        StringReader reader = new StringReader(toDeSerialize);

        // Deserialize the XML string to an object of type T
        return (T)xml.Deserialize(reader);
    }
}
