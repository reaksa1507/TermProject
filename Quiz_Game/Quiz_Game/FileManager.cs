using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using Quiz_Game.Model;

namespace Quiz_Game
{
    public class FileManager
    {
        private string path = @".\Questions";
        private string extension = ".json";

        public void Write(string fileName, object data)
        {
            try
            {
                if (!IsPathExist())
                {
                    CreateFolder();
                }

                string fullPath = GetFullPath(fileName);
                string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);

                using (StreamWriter streamWriter = new StreamWriter(fullPath))
                {
                    streamWriter.Write(jsonContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        public T Read<T>(string fileName)
        {
            string fullPath = GetFullPath(fileName);
            if (!File.Exists(fullPath))
            {
                return default(T);
            }

            string jsonContent = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        private void CreateFolder()
        {
            Directory.CreateDirectory(path);
        }

        private bool IsPathExist()
        {
            return Directory.Exists(path);
        }

        public string GetFullPath(string fileName)
        {
            return string.Format(@"{0}\{1}{2}", path, fileName, extension);
        }
    }
}