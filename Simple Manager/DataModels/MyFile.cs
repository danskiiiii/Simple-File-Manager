using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Simple_Manager.DataModels
{
    /// <summary>
    ///Class for file type items 
    /// </summary>

    class MyFile : DiskElement
    {
        string filePath;
        public MyFile(string filePath) : base(filePath)
        {
            this.filePath = filePath;
        }

        DateTime creationTime;
        public override DateTime CreationTime
        {
            get
            {
                return File.GetCreationTime(filePath);
            }
        }
        string name;
        public override string Name
        {
            get
            {
                return name = System.IO.Path.GetFileName(filePath);
            }
        }
        int size;
        public long Size
        {
            get
            {
                FileInfo fInfo = new FileInfo(filePath);
                return fInfo.Length;
            }
        }
        string extension;
        public string Extension
        {
            get
            {
                return extension = System.IO.Path.GetExtension(filePath);
            }
        }
        
    }
}
