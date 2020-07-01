using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace layui
{
    public class XMLManager
    {
        public static readonly XMLManager Instance = new XMLManager();

        public XMLConfig config = new XMLConfig();

        private static string configFileName = "XMLConfig.xml";
        private int maxCount = 8;

        public int GetDeviceCount
        {
            get
            {
                return maxCount;
            }
        }

        private XMLManager()
        {
            Load();
        }

        public void Load()
        {
            try
            {


                // XmlSerializer ser = new XmlSerializer(typeof(XMLConfig));
                //// string path = AppDomain.CurrentDomain.BaseDirectory.ToString()  + configFileName;
                // XmlReader sr = XmlReader.Create(configFileName);
                // config = (XMLConfig)ser.Deserialize(sr);
                // sr.Close();


                XmlSerializer serializer = new XmlSerializer(typeof(XMLConfig));
                string file = AppDomain.CurrentDomain.BaseDirectory.ToString() + configFileName;
                StreamReader textReader = new StreamReader(file);
                config = serializer.Deserialize(textReader) as XMLConfig;
                textReader.Close();
            }
            catch
            {
            }
            if (config == null) config = new XMLConfig();
        }

        public static XMLConfig LoadXML()
        {
            try
            {


                // XmlSerializer ser = new XmlSerializer(typeof(XMLConfig));
                //// string path = AppDomain.CurrentDomain.BaseDirectory.ToString()  + configFileName;
                // XmlReader sr = XmlReader.Create(configFileName);
                // config = (XMLConfig)ser.Deserialize(sr);
                // sr.Close();


                XmlSerializer serializer = new XmlSerializer(typeof(XMLConfig));
                string file = AppDomain.CurrentDomain.BaseDirectory.ToString() + configFileName;
                StreamReader textReader = new StreamReader(file);
                var config = serializer.Deserialize(textReader) as XMLConfig;
                textReader.Close();
                return config;
            }
            catch
            {
                return null;
            }
            
        }

        public void Save()
        {
            try
            {
                XmlWriter wr = XmlWriter.Create(configFileName);
                XmlSerializer ser = new XmlSerializer(typeof(XMLConfig));
                ser.Serialize(wr, config);
                wr.Close();
            }
            catch (Exception e)
            {
            }
        }

    }

    [Serializable]
    public class XMLConfig
    {

        public List<funcInfo> funcInfoList = new List<funcInfo>();

        public void Add(funcInfo fInfo)
        {
            funcInfoList.Add(fInfo);
        }

        private string _filePath = "";
        public string filePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }


        private string _projectName = "";
        public string projectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
            }
        }

        private string _moudleName = "";
        public string moudleName
        {
            get
            {
                return _moudleName;
            }
            set
            {
                _moudleName = value;
            }
        }

        private string _className = "";
        public string className
        {
            get
            {
                return _className;
            }
            set
            {
                _className = value;
            }
        }



    }
    //[Serializable]
    public class funcInfo
    {
        public string funcName { get; set; }
        //public string funcDisplay { get; set; }

    }
}
