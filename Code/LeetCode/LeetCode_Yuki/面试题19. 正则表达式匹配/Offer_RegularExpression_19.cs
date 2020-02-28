using System;

public class Offer_RegularExpression_19
{
    public void Test()
    {
        var s = "ab";
        var p = ".*c";
        Console.WriteLine(IsMatch(s, p));
    }

    public bool IsMatch(string s, string p)
    {
        return MyMatch(s, 0, p, 0);
    }

    private bool MyMatch(string s, int indexS, string p, int indexP)
    {
        // 当模式串到达末尾时，如果字符串也到达末尾则true，否则说明字符串还有未匹配的部分
        if (indexP >= p.Length)
        {
            return indexS >= s.Length;
        }
        // 模式串与字符串的当前字符是否匹配（1.普通单字符比较 2. '.'符号匹配任意单字符）。精妙之处：将字符串到末尾的情况当作一个无法匹配的字符来处理，认为递归仍可以继续，因为有匹配0个字符这种可能
        bool firstMatch = indexS <= s.Length - 1 && (p[indexP] == s[indexS] || p[indexP] == '.');
        // 模式串当前下标的下一个字符为*的特殊情况
        if (indexP + 1 <= p.Length - 1 && p[indexP + 1] == '*')
        {
            // 匹配0个字符，则模式串剪掉2个。匹配1个或以上字符，则字符串剪掉1个（1个以上的情况可以继续在这里递归）
            return MyMatch(s, indexS, p, indexP + 2) || (firstMatch && MyMatch(s, indexS + 1, p, indexP));
        }
        // 一般情况下的单个字符匹配
        else
        {
            // 若当前字符匹配，则双方都剪掉1个，继续递归
            return firstMatch && MyMatch(s, indexS + 1, p, indexP + 1);
        }
    }
}