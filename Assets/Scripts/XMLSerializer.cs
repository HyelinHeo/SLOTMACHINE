using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLSerializer
{
    public static bool Serialization<T>(object serializableObject, string filePath)
    {
        try
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(filePath);

            x.Serialize(writer, (T)serializableObject);

            writer.Close();

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }

    /// <summary>
    /// xml 문자열로 변환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializableObject"></param>
    /// <returns></returns>
    public static string Serialization<T>(object serializableObject)
    {
        try
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextWriter writer = new StringWriter();

            x.Serialize(writer, (T)serializableObject);

            writer.Close();

            return writer.ToString();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    public static T Deserialization<T>(string serialzizableString)
    {
        if (string.IsNullOrEmpty(serialzizableString)) return default(T);

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        T pack;
        try
        {
            StringReader sr = new StringReader(serialzizableString);
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreWhitespace = false;
            XmlReader reader = XmlReader.Create(sr, set);

            pack = (T)serializer.Deserialize(reader);
            sr.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Deserialization Error:" + e.ToString());
            Debug.LogError(serialzizableString);

            return default(T);
        }

        return pack;
    }
}
