using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run8Tools
{
    internal interface ICommand
    {
        public int Main(string[] args);
    }
}
