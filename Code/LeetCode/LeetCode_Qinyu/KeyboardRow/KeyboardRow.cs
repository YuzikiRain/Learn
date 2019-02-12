using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.KeyboardRow
{
    class KeyboardRow
    {
        string[] rows = new string[3]
{
                "QWERTYUIOPqwertyuiop",
                "ASDFGHJKLasdfghjkl",
                "ZXCVBNMzxcvbnm"
};
        int row = -1;

        public void Test()
        {
            //string[] test = new string[] { "Hello", "Alaska", "Dad", "Peace" };
            string[] test = new string[] {"qz", "wq", "asdddafadsfa", "adfadfadfdassfawde"};
            foreach(var str in FindWords(test))
            {
                Console.Write($"{str}  ");
            }
            //Console.ReadLine();
        }

        bool IsSame(char letter)
        {
            for (int k = 0; k < rows.Length; k++)
            {
                if (rows[k].Contains(letter))
                {
                    if (row != -1)
                    {
                        if (row != k) { return false; }
                    }
                    row = k;
                }
            }
            return true;
        }

        public string[] FindWords(string[] words)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < words.Length; i++)
            {
                row = -1;
                if (words[i].Length <= 1) { result.Add(words[i]); continue; }
                bool same = false;
                for (int j = 0; j < words[i].Length; j++)
                {
                    if (!(same=IsSame(words[i][j])))
                    { break; }
                }
                if (same)
                { result.Add(words[i]); }
            }

            return result.ToArray();
        }
    }
}
