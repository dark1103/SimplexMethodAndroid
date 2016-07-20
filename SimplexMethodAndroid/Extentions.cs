using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexMethod
{
    public static class Extentions
    {
        public static int Reduce(this int value)
        {
            return value - 1;
        }
        public static string Repeat(this string s,int count,string separator = "")
        {
            string result = string.Empty;
            for (int i = 0; i < count; i++)
            {
                result = string.Join(separator, result, s);
            }
            return result;
        }
    }
}
