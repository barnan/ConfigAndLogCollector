using BaseClasses;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ConfigAndLogCollectorProject.Repositories.XmlConfigRepo
{
    [XmlRoot(nameof(ArchiveOptions))]
    public class ArchiveOptions
    {
        [XmlArray("ArchiveOptionList"), XmlArrayItem(typeof(ArchiveOption), ElementName = "ArchiveOption")]
        public List<ArchiveOption> OptionList { get; set; }

        [XmlIgnore]
        private ILogger Logger { get; set; }

        [XmlIgnore]
        const string CLASS_NAME = nameof(ArchiveOptions);


        public ArchiveOptions()
        {
            OptionList = new List<ArchiveOption>();
        }


        public ArchiveOptions(List<ArchiveOption> optList)
        {
            OptionList = optList;
        }

        /// <summary>
        /// Serialize parameter to xml file
        /// </summary>
        /// <param name="path">path of the target xml file</param>
        /// <param name="options">instance</param>
        /// <returns></returns>
        public bool Serialize(string path, ArchiveOptions options)
        {
            try
            {
                return WriteToFile(path, options);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLog($"Exception ocured: {ex}", CLASS_NAME);
                return false;
            }
        }

        /// <summary>
        /// Deserialize from xml file
        /// </summary>
        /// <param name="path">path of source xml file</param>
        /// <returns></returns>
        public ArchiveOptions Deserialize(string path)
        {
            try
            {
                return ReadFromFile(path);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                return null;
            }
        }


        public static bool WriteToFile(string path, ArchiveOptions options)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return false;
                }

                XmlSerializer xmlser = new XmlSerializer(typeof(ArchiveOptions));
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    xmlser.Serialize(fs, options);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static ArchiveOptions ReadFromFile(string path)
        {
            ArchiveOptions serResult = null;

            try
            {
                if (!File.Exists(path))
                {
                    return serResult;
                }

                XmlSerializer xmlser = new XmlSerializer(typeof(ArchiveOptions));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    serResult = (ArchiveOptions)xmlser.Deserialize(fs);
                }

                return serResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
