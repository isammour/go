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
        bool NameExists();
        void Add();
        void List();
        void Update();
        void Delete();
        void Rename();
    }
}
