using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.TitleToNumber
{
    class TitleToNumber_171
    {
        public void Test()
        {
            string s = "ZY";
            TitleToNumber(s);

            //Console.ReadLine();
        }

        public int TitleToNumber(string s)
        {
            byte[] str_b = System.Text.Encoding.ASCII.GetBytes(s);
            int result = 0;
            int wei = 0;
            for (int i = str_b.Length-1; i >= 0; i--)
            {
                int asciicode = Convert.ToInt32(str_b[i]);
                result += (asciicode - 64) * (int)Math.Pow(26, wei);
                Console.WriteLine($"字母={str_b[i]}    asciicode={asciicode - 64}    equal={(asciicode - 64)} * 26^{wei} = {(asciicode - 64) * (int)Math.Pow(26, wei)}");
                wei++;
            }

            return result;
        }
    }
}
