using System;

namespace LeetCode_Qinyu.剑指offer.面试题20._表示数值的字符串
{
    class Offer20ValidNumber
    {
        public void Test()
        {
            var s = "12e+5.4";
            Console.WriteLine($"{s} is {IsNumber(s)}");
        }

        public bool IsNumber(string s)
        {
            s = s.Trim();
            if (s.Length == 0) { return false; }
            else if (s[0] == '+' || s[0] == '-') { s = s.Substring(1); }

            int countE = 0;
            int countDot = 0;
            int index = 0;
            while (index < s.Length)
            {
                if (char.IsDigit(s[index])) ;
                else if (s[index] == '.')
                {
                    countDot++;
                    // 1.超过1个. 2.字符串仅为"."  3.点出现时e已经存在，说明是指数部分的点 4.点前后字符都不是数字
                    if (countDot > 1 || s.Length == 1 || countE == 1) { return false; }
                }
                else if (s[index] == 'e' || s[index] == 'E')
                {
                    countE++;
                    // 1.超过1个e  2.e是最后一个字符 3. e是倒数第二个字符，最后字符是+/- 4.e是第一个字符 5.e的前一个字符是点，且点是第一个字符
                    if (countE > 1 || index == s.Length - 1 || (index == s.Length - 2 && (s[index + 1] == '+' || s[index + 1] == '-')) || index == 0 || (index - 1 == 0 && s[index - 1] == '.'))
                    { return false; }
                    // e的下一个字符是+/-，跳过下一个字符
                    if (index + 1 < s.Length && (s[index + 1] == '+' || s[index + 1] == '-')) { index += 2; continue; }
                }
                // 非+/-/e/E/.的非法字符
                else { return false; }
                index++;
            }
            return true;
        }
    }
}
