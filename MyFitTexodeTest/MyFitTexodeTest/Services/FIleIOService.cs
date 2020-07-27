using System.Collections.Generic;
using System.IO;
using System.Text;
using MyFitTexodeTest.Model;
using Newtonsoft.Json;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace MyFitTexodeTest.Services
{
    class FIleIOService
    {
        private readonly string _path;

        public FIleIOService(string path)
        {
            _path = path;
        }

        public List<Record> LoadData()
        {
            var fileExist = File.Exists(_path);

            if (!fileExist)
            {
                File.CreateText(_path).Dispose();
                return new List<Record>();
            }

            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Record>>(fileText);
            }
        }
                
        public void ExportDataJson(Data data)
        {
            using (StreamWriter writer = File.CreateText(_path))
            {
                string output = JsonConvert.SerializeObject(data);
                writer.Write(output);
            }
        }

        public void ExportDataXml(Data data)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Data));

            using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, data);
            }
        }

        public void ExportDataCsv(Data data)
        {
            var sb = new StringBuilder();
            var header = "";
            var into = typeof(Data).GetProperties();

            using (StreamWriter writer = new StreamWriter(new FileStream(_path, FileMode.OpenOrCreate), Encoding.UTF8))
            {

                foreach (var prop in typeof(Data).GetProperties())
                {
                    header += prop.Name + "; ";
                }

                header = header.Substring(0, header.Length - 2);
                sb.AppendLine(header);
                writer.Write(sb.ToString());

                var sb2 = new StringBuilder();
                var line = "";
                foreach (var prop in into)
                {
                    line += prop.GetValue(data, null) + "; ";
                }

                line = line.Substring(0, line.Length - 2);
                sb2.AppendLine(line);
                writer.Write(sb2.ToString());
            }

        }
    }
}
