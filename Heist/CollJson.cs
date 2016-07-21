using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist
{
    class CollJson
    {
        public string name { get; set; }
        public string userName;
        public List<Tuple<string, string>> list = new List<Tuple<string, string>>();
        public CollJson(string collName,string userName)
        {
            this.name = collName;
            this.userName = userName;
        }
        public void insert(string book,string chap)
        {
            list.Add(new Tuple<string, string>(book, chap));
        }

    }
}
