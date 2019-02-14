using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.RotateArray_189
{
    class RotateArray_189
    {
        public void Test()
        {
            int[] numbers = new int[] { -1, -100, 3, 99 };
            Rotate(numbers, 5);
            foreach (var number in numbers)
            {
                Console.Write($"{number}, ");
            }
        }

        public void Rotate(int[] nums, int k)
        {
            RightRotateString(nums, k);
        }

        // Rotate in-place
        public void LeftRotateString(int[] s, int position)
        {
            position %= s.Length;

            ReverseString(s, 0, position - 1);
            ReverseString(s, position, s.Length - 1);
            ReverseString(s, 0, s.Length - 1);
        }

        public void RightRotateString(int[] s, int position)
        {
            position %= s.Length;
            LeftRotateString(s, s.Length - position);
        }

        public void ReverseString(int[] s, int from, int to)
        {
            int temp = 0;
            for (int i = from, j = to; i < j; i++, j--)
            {
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }
        }
    }
}
