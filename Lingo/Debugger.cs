using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingo
{
    /// <summary>
    /// Aids in debugging - mostly printing stuff to console so as not to overcrowd code in main
    /// </summary>
    class Debugger
    {
        // Print array contents
        public void PrintArray(int[] array, string name)
        {
            Console.WriteLine(name + " contents: ");
            foreach (int i in array)
                Console.Write(i.ToString() + ", ");
            Console.Write("\n");           
        }

        // Print list contents
        public void PrintList(List<string> list, string name)
        {
            Console.WriteLine(name + " contents: ");
            foreach (string s in list)
                Console.Write(s.ToString() + ", ");
            Console.Write("\n");
        }

    }
}
