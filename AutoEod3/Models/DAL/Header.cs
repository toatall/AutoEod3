using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.DAL
{
    public class Header
    {
        public string Name
        {
            get; private set;
        }

        public bool IsText
        {
            get; private set;
        }

        public Header(string name, bool isText)
        {
            this.Name = name;
            this.IsText = isText;
        }
    }
}
