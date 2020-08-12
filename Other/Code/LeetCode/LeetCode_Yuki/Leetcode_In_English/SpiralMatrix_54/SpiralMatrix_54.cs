using System;
using System.Collections.Generic;

namespace LeetCode_Yuki.SpiralMatrix_54
{
    class SpiralMatrix_54
    {
        public void Test()
        {
            List<int> list = SpiralOrder(new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }) as List<int>;
            foreach (var element in list)
            {
                Console.WriteLine(element);
            }
        }
        public IList<int> SpiralOrder(int[,] matrix)
        {
            List<int> list = new List<int>();
            int row = matrix.GetLength(0);
            int colunm = matrix.GetLength(1);

            for (int i = 0, j = 0; i * 2 < row && j * 2 < colunm; i++, j++)
            {
                GetMatrixElementInCircle(i, j, row, colunm, list, matrix);
            }
            return list;
        }

        private void GetMatrixElementInCircle(int i, int j, int row, int colunm, IList<int> list, int[,] matrix)
        {
            int left = j;
            int right = colunm - 1 - j;
            int up = i;
            int down = row - 1 - i;

            // from left to right
            for (i = up, j = left; j <= right; j++)
            {
                list.Add(matrix[i, j]);
            }
            // means row > 1
            if (down > up)
            {
                // from up to down
                for (i = up + 1, j = right; i <= down; i++)
                {
                    list.Add(matrix[i, j]);
                }
                // means colunm > 1
                if (right > left)
                {
                    // from right to left
                    for (i = down, j = right - 1; j >= left; j--)
                    {
                        list.Add(matrix[i, j]);
                    }
                    // from down to up
                    for (i = down - 1, j = left; i >= up + 1; i--)
                    {
                        list.Add(matrix[i, j]);
                    }
                }
            }
        }
    }
}
