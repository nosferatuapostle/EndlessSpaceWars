using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EndlessSpace
{
    public class Save
    {
        int game_id;
        string game_name, base_folder, backup_folder, backup_path;
        float research_points;
        bool loading_id;
        XDocument save_file;

        public Save(int game_id, string game_name)
        {
            this.game_id = game_id;
            this.game_name = game_name;

            backup_folder = "backups";
            base_folder = "saves";
            base_folder = Globals.AppData + "\\" + game_name + "";

            CreateFolders();
        }

        bool CheckExists(string file_name)
        {
            return File.Exists(Globals.AppData + "\\" + game_name + "\\Saves\\" + file_name + ".xml");
        }

        public void DeleteFile(string file_name)
        {
            File.Delete(Globals.AppData + "\\" + game_name + "\\Saves\\" + file_name + ".xml");
        }

        public XDocument LoadFile(string file_name, bool key = true)
        {
            return GetFile(file_name);
        }

        XDocument GetFile(string file_name)
        {
            string path = Globals.AppData + "\\" + game_name + "\\Saves\\" + file_name + ".xml";
            if (File.Exists(path))
            {
                return XDocument.Load(path);
            }
            return null;
        }

        public void HandleSaveFormat(XDocument xml)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(xml.ToString());
            File.WriteAllBytes(Globals.AppData + "\\" + game_name + "\\Saves\\" + game_name + ".xml", bytes);
        }
        
        public void HandleSaveFormat(XDocument xml, string file_name) => xml.Save(Globals.AppData + "\\" + game_name + "\\Saves\\" + file_name + ".xml");

        void CreateFolders()
        {
            CreateFolder(Globals.AppData + "\\" + game_name);
            CreateFolder(Globals.AppData + "\\" + game_name + "\\Saves");
            CreateFolder(Globals.AppData + "\\" + game_name + "\\Backups");
        }

        void CreateFolder(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }
        
        public static string BinaryToString(string data)
        {
            List<byte> byte_list = new List<byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                byte_list.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byte_list.ToArray());
        }
    }
}
