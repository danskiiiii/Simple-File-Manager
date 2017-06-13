using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Simple_Manager.DataModels
{
    /// <summary>
    ///Class for directory type items 
    /// </summary>


    class MyDirectory : DiskElement
    {
        string dirPath;
        public MyDirectory(string dirPath) : base(dirPath)
        {
            this.dirPath = dirPath;
        }

        public override DateTime CreationTime
        {
            get
            {
                return Directory.GetCreationTime(dirPath);
            }
        }

        string name;
        public override string Name
        {
            get
            {
                return name = System.IO.Path.GetFileName(dirPath);
            }
        }

        /// <summary>
        ///Method returns list of all files in directory
        /// </summary>        
        public List<MyFile> GetAllFiles()
        {
            string[] subFiles = Directory.GetFiles(Path);

            List<MyFile> result = new List<MyFile>();
            foreach (string file in subFiles)
            {
                result.Add(new MyFile(file));
            }
            return result;

        }

        /// <summary>
        ///Method returns list of subdirectories
        /// </summary>
        public MyDirectory[] GetSubDirectories()
        {
            string[] subDirs = Directory.GetDirectories(dirPath);

            List<MyDirectory> result = new List<MyDirectory>();
            foreach (string dir in subDirs)
            {
                result.Add(new MyDirectory(dir));
            }
            return result.ToArray();
        }

        /// <summary>
        ///Method returns list of files and subdirectories in a directory
        /// </summary>
        public List<DiskElement> GetSubDiskElements()
        {
            List<DiskElement> result = new List<DiskElement>();
            result.AddRange(GetSubDirectories());
            result.AddRange(GetAllFiles());
            return result;
        }

        /// <summary>
        ///Method returns list of files and subdirectories in a directory, sorted by name
        /// </summary>
        public List<DiskElement> GetSubDiskElements_byName()
        {
            List<DiskElement> result = new List<DiskElement>();
            result.AddRange(GetSubDirectories());
            result.AddRange(GetAllFiles());
            return result.OrderBy(o => o.Name).ToList();
        }

        /// <summary>
        ///Method returns list of files and subdirectories in a directory, sorted by creation time
        /// </summary>
        public List<DiskElement> GetSubDiskElements_byDate()
        {
            List<DiskElement> result = new List<DiskElement>();
            result.AddRange(GetSubDirectories());
            result.AddRange(GetAllFiles());
            return result.OrderBy(o => o.CreationTime).ToList();
        }

        /// <summary>
        /// Method for copying a directory (with all subelements recursively) 
        /// Taken from: https://www.rhyous.com/2011/07/25/how-to-copy-a-directory-recursively-in-cshar/
        /// </summary>
        public void DirectoryCopy( string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = System.IO.Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}
