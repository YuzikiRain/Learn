using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UniquePaths_62
{
    public static void Test()
    {
        Console.WriteLine(UniquePaths(7, 3));
    }

    public static int UniquePaths(int m, int n)
    {
        int[,] dp = new int[n, m];

        for (int row = 0; row < n; row++)
        {
            for (int column = 0; column < m; column++)
            {
                if (row == 0 || column == 0) { dp[row, column] = 1; }
                else { dp[row, column] = dp[row - 1, column] + dp[row, column - 1]; }
            }
        }

        return dp[n - 1, m - 1];
    }
}
