using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CoinChange_322
{
    public static void Test()
    {
        Console.WriteLine(CoinChange(new int[] { 2, 4 }, 3));
    }

    private static int CoinChange(int[] coins, int amount)
    {
        int[] dp = new int[amount + 1];
        dp[0] = 0;
        for (int i = 1; i <= amount; i++)
        {
            int minCost = int.MaxValue;
            for (int j = 0; j < coins.Length; j++)
            {
                // dp[i - coins[j]] == int.MaxValue 表示用coins[j]这种硬币来凑 i 是不可能的
                // 比如只有2元硬币，1元无法找零，而3元也无法找零，因为（找了1个2元硬币后剩下的）1元找不出来
                if (i >= coins[j] && dp[i - coins[j]] != int.MaxValue)
                {
                    int newCost = dp[i - coins[j]] + 1;
                    minCost = Math.Min(minCost, newCost);
                }
            }
            dp[i] = minCost;
        }

        // 如果是找不了零的情况，根据题意应该返回-1
        return dp[amount] == int.MaxValue ? -1 : dp[amount];
    }
}
