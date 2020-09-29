using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace DomainModels.Helper
{
  
    public static class ConvertHelpers
    {
        public static string ToSafeString(this object value)
        {
            return value == null ? null : value.ToString();
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string SafeTrim(this string value)
        {
            return value.IsEmpty() ? null : value.Trim();
        }

        public static string SafeSubstring(this string text, int start, int length)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return string.Join("", text.Skip(start).Take(length));
        }

        public static object GetPropertyValue<TKey>(this TKey data, string propertyName, bool throwException = true) where TKey : class
        {
            var propty = data.GetType().GetProperties().Where(a => a.Name == propertyName).FirstOrDefault();
            if (propty == null)
            {
                if (throwException)
                    throw new Exception(propertyName + " - Column not exist");
                else
                    return null;
            }
            else
                return propty.GetValue(data, null);
        }

        public static List<List<T>> SplitList<T>(this IEnumerable<T> collection, int size)
        {
            var chunks = new List<List<T>>();
            var count = 0;
            var temp = new List<T>();

            foreach (var element in collection)
            {
                if (count++ == size)
                {
                    chunks.Add(temp);
                    temp = new List<T>();
                    count = 1;
                }
                temp.Add(element);
            }
            chunks.Add(temp);

            return chunks;
        }

        public static T ChangeType<T>(this object value)
        {
            if (value is string && string.IsNullOrWhiteSpace((string)value))
                value = null;

            if (value == null)
                return default(T);

            var t = typeof(T);
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (t == typeof(string))
                return (T)(object)Convert.ToString(value);

            var safeValue = Convert.ChangeType(value, t);
            return (T)safeValue;
        }

        public static T TryChangeType<T>(this object value, T defaultValue)
        {
            try
            {
                return value.ChangeType<T>();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static T GetEnumFromName<T>(this string value, T defaultValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public static string GetDescription(this Enum value)
        {
            Type genericEnumType = value.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(value.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return value.ToString();
        }

        public static DateTime? TryGetDateTime(this string value, string dateFormat = "dd/MM/yyyy")
        {
            try
            {
                var result = DateTime.MinValue;

                if (DateTime.TryParseExact(value, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out result))
                    return result;
                else if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out result))
                    return result;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T ToEnum<T>(this string inputEnum) where T : struct
        {
            T result = default(T);
            Enum.TryParse<T>(inputEnum, out result);
            return result;
        }

        public static T? TryParseEnum<T>(this string inputEnum) where T : struct
        {
            T result = default(T);
            var success = Enum.TryParse<T>(inputEnum, out result);
            if (success)
                return result;
            else
                return null;
        }

        public static string DefaultIfNull<T>(this T source, Func<T, string> selector, string defaultValue)
        {
            if (source == null)
                return defaultValue;

            var value = selector(source);

            if (value == null)
                return defaultValue;

            return value;
        }

        public static string CleanupEmpty(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return text;
        }

        public static Stream ToStream(this string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream ToStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return stream;
        }

        public static byte[] ToBytes(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static string ReadAsString(this byte[] content)
        {
            return Encoding.UTF8.GetString(content);
        }

        public static T DeserializeJson<T>(this string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string SerializeJson<T>(this T data) where T : class
        {
            if (data == null)
                return null;

            try
            {
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetAge(this DateTime dob)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dobN = int.Parse(dob.ToString("yyyyMMdd"));
            int age = (now - dobN) / 10000;
            return age;
        }

        /// <summary>
        /// Serialize a serializable object to XML string.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="xmlObject">Type of object</param>
        /// <param name="useNamespaces">Use of XML namespaces</param>
        /// <returns>XML string</returns>
        public static string SerializeToXmlString<T>(this T xmlObject, bool useNamespaces = true)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
            {
                xmlTextWriter.Formatting = System.Xml.Formatting.None;

                if (useNamespaces)
                {
                    XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add("", "");
                    xmlSerializer.Serialize(xmlTextWriter, xmlObject, xmlSerializerNamespaces);
                }
                else
                    xmlSerializer.Serialize(xmlTextWriter, xmlObject);

                string output = Encoding.UTF8.GetString(memoryStream.ToArray());
                return output.RemoveByteOrderMarks(Encoding.UTF8);
            }
        }

        public static string RemoveByteOrderMarks(this string text, Encoding encoding)
        {
            string _byteOrderMarkUtf8 = encoding.GetString(encoding.GetPreamble());
            var output = text;
            if (output.StartsWith(_byteOrderMarkUtf8))
            {
                output = output.Remove(0, _byteOrderMarkUtf8.Length);
            }
            return output;
        }

        public static string ToXMLString(this XmlDocument xmlDoc, bool isStandalone = false)
        {
            if (xmlDoc == null)
                return null;

            return xmlDoc.OuterXml.Trim();
        }

        public static T XMLDeserialize<T>(this string content) where T : class
        {
            if (content == null)
                return null;

            using (var ms = content.ToStream())
            {
                using (XmlReader reader = XmlReader.Create(ms))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    return (T)xs.Deserialize(reader);
                }
            }
        }

        public static string GetFileHashSha256(this byte[] bytes)
        {
            string hash = "";
            using (var ms = bytes.ToStream())
            {
                var hashBytes = GetHashSha256(ms);

                foreach (byte b in hashBytes)
                    hash += b.ToString("x2");

                return hash;
            }
        }

        private static byte[] GetHashSha256(Stream stream)
        {
            using (var Sha256 = SHA256.Create())
            {
                return Sha256.ComputeHash(stream);
            }
        }

        public static byte[] ToByteArray(this string content)
        {
            if (content == null)
                return null;

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return bytes;
        }

        public static string ToBase64(this string content)
        {
            return Convert.ToBase64String(content.ToByteArray());
        }

        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] FromBase64(this string content)
        {
            return Convert.FromBase64String(content);
        }

        public static string ToStringContent(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static T[][] DifferentCombinations<T>(this IEnumerable<T> elements, int k)
        {
            if (elements.Count() < k)
                return new[] { new T[0] }.Where(x => x.Count() == k).ToArray();

            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)))
                .Select(x => x.ToArray())
                .Where(x => x.Count() == k)
                .ToArray();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string RegexReplace(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        public static string Sha256(this string value)
        {
            using (var crypt = new SHA256Managed())
            {
                string hash = String.Empty;
                byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(value));
                foreach (byte theByte in crypto)
                {
                    hash += theByte.ToString("x2");
                }

                return hash;
            }
        }

        public static string GetFilename(this string filename)
        {
            var name = filename.Split(new char[] { '/', '\\' }).LastOrDefault();
            var fi = new FileInfo(name);
            return fi.Name;
        }

        public static bool IsImageType(this string path)
        {
            var name = path.GetFilename();
            var ext = Path.GetExtension(name)?.ToLower();

            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    return true;
                default:
                    return false;
            }
        }


        public static void EnsureDirectoryExist(this string path)
        {
            var fi = new FileInfo(path);
            if (!fi.Directory.Exists)
                fi.Directory.Create();
        }

        public static string SafeSubstringToLastSpace(this string text, int start, int length)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var truncText = string.Join("", text.Skip(start).Take(length));
            var indexOfLastSpace = truncText.LastIndexOf(" ");
            return string.Join("", text.Skip(start).Take(indexOfLastSpace));
        }



        public static string ReplaceStartWith(this string src, string start, string target)
        {
            if (src.StartsWith(start))
            {
                var sub = src.Substring(start.Count());
                return target + sub;
            }

            return src;
        }

        public static string[] GetAddressByLines(this string text, int charLimit)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new string[3];
            string[] address = new string[3];
            for (int i = 0; i < address.Length; i++)
            {
                if (text.Length >= charLimit)
                {
                    string subString = text.SafeSubstring(0, charLimit);
                    int lastSpace = subString.LastIndexOf(' ');
                    address[i] = text.SafeSubstring(0, lastSpace).Trim();
                    text = text.SafeSubstring(lastSpace, text.Length).Trim();
                }
                else
                {
                    address[i] = text;
                    text = "";
                }
            }
            return address;
        }

        public static string ReplaceSpecialChars(this string src, string target = " ")
        {
            if (string.IsNullOrWhiteSpace(src))
                return null;

            var allChars = @"`~!@#$%^&*()_+}{"":?><,./;'[]=- ";
            var charsToIgnore = " -_\\/,.";
            var charsToReplace = string.Join("", allChars.Where(x => !charsToIgnore.Contains(x)));

            return Regex.Replace(src, @"[" + Regex.Escape(charsToReplace).Replace("]", "\\]") + "]", target).Replace("  ", " ")?.Trim();
        }
    }
}
