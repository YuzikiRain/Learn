using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.Transpose
{
    class Transpose_867
    {
        public void Test()
        {
            int[][] test = new int[3][];
            test[0] = new int[3] { 1, 2, 3 };
            test[1] = new int[3] { 4, 5, 6 };
            test[2] = new int[3] { 7, 8, 9 };
            //int[][] test = new int[2][];
            //test[0] = new int[3] { 1, 2, 3 };
            //test[1] = new int[3] { 4, 5, 6 };


            foreach (var row in Transpose(test))
            {
                foreach (var colunm in row)
                {
                    Console.Write($"{colunm} ");
                }
                Console.WriteLine($"");
            }

            //Console.ReadLine();
        }

        public int[][] Transpose(int[][] A)
        {
            int colunm = A[0].Length;
            int row = A.Length;

            int[][] result = new int[colunm][];

            for (int colunm_index = 0; colunm_index < colunm; colunm_index++)
            {
                result[colunm_index] = new int[row];
            }

            for (int row_index = 0; row_index < row; row_index++)
            {
                for (int colunm_index = 0; colunm_index < colunm; colunm_index++)
                {
                    result[colunm_index][row_index] = A[row_index][colunm_index];
                }
            }
            return result;
        }
    }
}
