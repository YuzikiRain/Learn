using System;

class Offer21
{
    public void Test()
    {
        var nums = new int[] { 1, 4, 6, 8, 3, 4, 5, 1, 9, 4, 2 };
        foreach (var number in nums) { Console.Write($"{number}, "); }
        Exchange(nums);
        Console.WriteLine("\n-----------------------");
        foreach (var number in nums) { Console.Write($"{number}, "); }
    }

    public int[] Exchange(int[] nums)
    {
        int left = 0;
        int right = nums.Length - 1;

        while (left < right)
        {
            if (nums[left] % 2 != 0) { left++; continue; }
            if (nums[right] % 2 == 0) { right--; continue; }
            // 逆序对，交换
            int temp = nums[left];
            nums[left] = nums[right];
            nums[right] = temp;
            left++;
            right--;
        }

        return nums;
    }
}
