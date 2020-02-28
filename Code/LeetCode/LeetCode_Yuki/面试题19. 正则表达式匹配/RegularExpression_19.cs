using System;

public class RegularExpression_19
{
    public void Test()
    {
        var s = "ab";
        var p = ".*c";
        Console.WriteLine(IsMatch(s, p));
    }

    public bool IsMatch(string s, string p)
    {
        //if (p == "") { return false; }
        if (p == "" && s != "") { return false; }

        int indexS = 0;
        int indexP = 0;

        return MyMatch(s, indexS, p, indexP);
    }

    private bool MyMatch(string s, int indexS, string p, int indexP)
    {
        Console.WriteLine($"--------------------------------------------------------");
        // 处理递归结束条件：都到达末尾
        if (indexS >= s.Length && indexP >= p.Length)
        {
            return true;
        }
        // 仅模式串到达了末尾
        else if (indexS < s.Length && indexP >= p.Length) { return false; }

        Console.WriteLine($"s[{indexS}]={(indexS < s.Length ? s[indexS] : '空')} p[{indexP}]={(indexP < p.Length ? p[indexP] : '空')}");

        // 模式串下一个字符为*
        if (indexP + 1 <= p.Length - 1 && p[indexP + 1] == '*')
        {
             if (indexS >= s.Length)
            {
                return MyMatch(s, indexS, p, indexP + 2);
            }
            if (p[indexP] == s[indexS])
            {
                if (MyMatch(s, indexS, p, indexP + 2)) { return true; }
                else if (MyMatch(s, indexS + 1, p, indexP + 1)) { return true; }
                else if (MyMatch(s, indexS + 1, p, indexP)) { return true; }
                else { return false; }
            }
            else if (p[indexP] == '.')
            {
                if (MyMatch(s, indexS, p, indexP + 2)) { return true; }
                else if (MyMatch(s, indexS + 1, p, indexP + 2)) { return true; }
                else if (MyMatch(s, indexS + 1, p, indexP)) { return true; }
                else { return false; }
            }
            else
            {
                return MyMatch(s, indexS, p, indexP + 2);
            }
        }
        else
        {
            if (indexS >= s.Length)
            {
                return false;
            }
            if (p[indexP] == s[indexS])
            {
                return MyMatch(s, indexS + 1, p, indexP + 1);
            }
            else if (p[indexP] == '.')
            {
                return MyMatch(s, indexS + 1, p, indexP + 1);
            }
            // 即当前单个字符不匹配
            else
            {
                return false;
            }
        }

    }

}