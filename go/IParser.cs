using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go
{
    interface IParser
    {
        void ArgsValid();
        bool NameExists(string name);
        void Add();
        void List();
        void Update();
        void Delete();
        void Rename();
        void Help();
        void PerformGo();

    }
}
