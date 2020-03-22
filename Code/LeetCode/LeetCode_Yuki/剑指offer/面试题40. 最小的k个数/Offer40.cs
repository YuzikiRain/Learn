using System;
using System.Collections.Generic;
using System.Linq;

class Offer40
{
    public void Test()
    {
        int[] values = new int[] { 0, 0, 1, 2, 4, 2, 2, 3, 1, 4 };
        var result = GetLeastNumbers(values, 8);
    }

    public int[] GetLeastNumbers(int[] arr, int k)
    {
        // 复杂度O(logn) k较大时使用快排思维将数组划分为较大和较小的部分。
        // 如果较小的部分长度等于k，直接返回，大于则对较小部分继续，小于则对较大部分继续


        // 复杂度O(k*n) k较小时采用选择排序
        for (int j = 0; j < k; j++)
        {
            int minIndex = j;
            for (int i = j; i < arr.Length; i++)
            {
                if (arr[i] < arr[minIndex]) { minIndex = i; }
            }
            var temp = arr[minIndex];
            arr[minIndex] = arr[j];
            arr[j] = temp;
        }

        Array.Resize(ref arr, k);
        return arr;
    }
}