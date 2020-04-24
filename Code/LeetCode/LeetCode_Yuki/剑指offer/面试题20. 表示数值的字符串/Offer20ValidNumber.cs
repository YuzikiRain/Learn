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

        // A[.[B]][e|EC] 或 .B[e|EC]
        public bool IsNumber(string s)
        {
            s = s.Trim();
            int index = 0;
            bool hasB = false;
            // 扫描A部分：（可能带符号的）整数
            bool hasA = ScanInteger(s, ref index);
            // 有'.'则扫描B部分：无符号整数
            if (index < s.Length && s[index] == '.')
            {
                index++;
                hasB = ScanUnsignedInteger(s, ref index);
            }
            // 有'e'则扫描C部分：（可能带符号的）整数
            if (index < s.Length && (s[index] == 'e' || s[index] == 'E'))
            {
                index++;
                bool hasC = ScanInteger(s, ref index);
                if (!hasC) { return false; }
            }
            // 如果扫描了ABC部分后仍没有到末尾，说明遇到了非法字符
            return index >= s.Length && (hasA || hasB);
        }

        private bool ScanInteger(string s, ref int index)
        {
            if (index > s.Length - 1) { return false; }
            // +/-符号
            if (s[index] == '+' || s[index] == '-') { index++; }
            return ScanUnsignedInteger(s, ref index);
        }

        private bool ScanUnsignedInteger(string s, ref int index)
        {
            if (index > s.Length - 1) { return false; }
            int start = index;
            while (index < s.Length)
            {
                if (char.IsDigit(s[index])) { index++; }
                else { break; }
            }
            return index > start;
        }
    }
}
