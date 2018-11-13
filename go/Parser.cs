using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace go
{
    public class Parser : IParser
    {
        string[] _args;
        string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"goData.xml");
        XmlDocument _document;
        List<string> _actions = new List<string>
        {
            "add",
            "list",
            "update",
            "delete",
            "rename"
        };
        public Parser(string[] args)
        {
            _document = new XmlDocument();
            if (!File.Exists(_path))
            {
                _document.AppendChild(_document.CreateElement("GoData"));
                _document.Save(_path);
            }
            _args = args;
        }

        public void Add()
        {
            if(_args.Count() != 3)
            {
                Console.WriteLine("Not enough arguments provided to \"go add\". \n Type go -help for usage details.");
            }
            else if (NameExists(_args[1]))
            {
                Console.WriteLine("Name already exist please choose another name ");
            }
            else
            {
                _document.Load(_path);
                var root = _document.SelectSingleNode("/GoData");
                var node = _document.CreateElement("namePathPair");
                var nameAttr = _document.CreateElement("name");
                nameAttr.InnerText = _args[1];
                var pathAttr = _document.CreateElement("path");
                pathAttr.InnerText = _args[2];
                node.AppendChild(nameAttr);
                node.AppendChild(pathAttr);
                root.AppendChild(node);
                _document.Save(_path);
                Console.WriteLine("Created : \n name = "+ _args[1] + "\n path = "+ _args[2]);
            }
        }

        public void ArgsValid()
        {
            if(_args.Count() == 0)
            {
                Console.WriteLine("No args provided to go, type \"go -help\" for usage");
                return;
            }
            if (_args[0] == "add")
            {
                Add();
                return;
            }
            else if (_args[0] == "list")
            {
                List();
                return;
            }
            else if (_args[0] == "update")
            {
                Update();
                return;
            }
            else if (_args[0] == "delete")
            {
                Delete();
                return;
            }
            else if (_args[0] == "rename")
            {
                Rename();
                return;
            }
            else if (_args[0] == "-help")
            {
                Help();
                return;
            }
            else if (_args.Count() == 1)
            {
                PerformGo();
                return;
            }
        }

        public void Delete()
        {
            if(_args.Count() != 2)
            {
                Console.WriteLine("Not enough arguments provided to \"go delete\". \n Type go -help for usage details.");
            }
            else
            {
                if (NameExists(_args[1]))
                {
                    _document.Load(_path);
                    var root = _document.FirstChild;
                    var nodes = root.ChildNodes;
                    foreach(XmlNode pair in nodes)
                    {
                        if(pair.FirstChild.InnerText == _args[1])
                        {
                            root.RemoveChild(pair);
                        }
                    }
                    _document.Save(_path);
                    Console.WriteLine("name : " + _args[1] + " has been deleted successfully");
                }
                else
                {
                    Console.WriteLine("name : " + _args[1] + " doesn't exist , type go list to view saved names");
                }
            }
        }

        public void List()
        {
            _document.Load(_path);
            var root = _document.FirstChild;
            var nodes = root.ChildNodes;
            Console.WriteLine("name-path pairs count : " + nodes.Count);
            foreach(XmlNode pair in nodes)
            {
                Console.WriteLine("name : " + pair.ChildNodes[0].InnerText + " and path : " + pair.ChildNodes[1].InnerText);
            }
        }

        public bool NameExists(string name)
        {
            _document.Load(_path);
            var root = _document.FirstChild;
            var nodes = root.ChildNodes;
            foreach (XmlNode pair in nodes)
            {
                if(pair.FirstChild.InnerText == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void Rename()
        {
            if(_args.Count() != 3)
            {
                Console.WriteLine("Not enough arguments provided to \"go rename\". \n Type go -help for usage details.");
            }
            else
            {
                _document.Load(_path);
                if (NameExists(_args[1]))
                {
                    var root = _document.FirstChild;
                    var nodes = _document.ChildNodes;
                    foreach(XmlNode pair in nodes)
                    {
                        if(pair.FirstChild.InnerText == _args[1])
                        {
                            pair.FirstChild.InnerText = _args[2];
                            Console.WriteLine("name : " + _args[1] + " renamed to : " + _args[2]);
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("name doesn't exit, type \"go list\" to view saved name-path pairs ");
                }
            }
        }

        public void Update()
        {
            if (_args.Count() != 3)
            {
                Console.WriteLine("Not enough arguments provided to \"go update\". \n Type go -help for usage details.");
            }
            else
            {
                if (NameExists(_args[1]))
                {
                    _document.Load(_path);
                    var root = _document.FirstChild;
                    var nodes = root.ChildNodes;
                    foreach(XmlNode pair in nodes)
                    {
                        if(pair.FirstChild.InnerText == _args[1])
                        {
                            pair.ChildNodes[1].InnerText = _args[2];
                            Console.WriteLine("updated !!");
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("name doesn't exit, type \"go list\" to view saved name-path pairs ");
                }
            }
        }

        public void Help()
        {
            Console.WriteLine("usage :");
            Console.WriteLine("go add <name> <path>");
            Console.WriteLine("go list");
            Console.WriteLine("go update <name> <new path>");
            Console.WriteLine("go rename <old name> <new name>");
            Console.WriteLine("go delete <name>");
        }

        public void PerformGo()
        {
            if (NameExists(_args[0]))
            {
                _document.Load(_path);
                var root = _document.FirstChild;
                var nodes = root.ChildNodes;
                foreach(XmlNode pair in nodes)
                {
                    if(pair.FirstChild.InnerText == _args[0])
                    {
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.FileName = "powershell.exe";
                        startInfo.WorkingDirectory = pair.ChildNodes[1].InnerText;
						process.StartInfo = startInfo;
                        process.Start();
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("name doesn't exit, type \"go list\" to view saved name-path pairs ");
            }
        }
    }
}
