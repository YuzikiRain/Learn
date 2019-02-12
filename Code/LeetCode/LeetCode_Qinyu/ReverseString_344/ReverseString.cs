namespace LeetCode_Qinyu.ReverseString_344
{
    class Test
    {
        public Test()
        {
            ReverseString("hello".ToCharArray());
        }

        public void ReverseString(char[] s)
        {
            for(int i = 0, j = s.Length - 1; i < j; i++, j--)
            {
                var temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }
        }
    }
}
