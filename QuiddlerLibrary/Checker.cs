using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;


namespace QuiddlerLibrary
{
    internal class Checker
    {
        internal Application checker;

        internal Checker()
        {
            checker = new Application();
        }
        internal bool CheckSpelling(string candidate)
        {
            bool result = checker.CheckSpelling(candidate)? true : false;
            return result;
        }

        internal void Quit()
        {
            checker.Quit();
        }
    }
}
