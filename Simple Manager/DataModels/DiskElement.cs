using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Simple_Manager.DataModels
{

    /// <summary>
    ///Code for abstract base class 
    /// </summary>

    public abstract class DiskElement
    {
        string path;
        public string Path
        {
            get
            {
                return path;
            }
        }

        public DiskElement(string path)
        {
            this.path = path;
        }
        
        public abstract DateTime CreationTime
        {
            get;

        }
        
        public abstract string Name
        {
            get;
        }

        

    }
}
