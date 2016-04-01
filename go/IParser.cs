using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go
{
    interface IParser
    {
        bool ArgsValid();
        bool NameExists(string name);
        void Add(string name,string path);
        void List();
        void Update(string name,string path);
        void Delete(string name);
        void Rename(string oldName, string newName);
    }
}
