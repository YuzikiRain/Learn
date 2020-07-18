using System;

class Offer29
{
    const int row = 3;
    const int column = 3;
    public void Test()
    {
        int[][] matrix = new int[row][];
        int count = 1;
        for (int rowIndex = 0; rowIndex < matrix.Length; rowIndex++)
        {
            matrix[rowIndex] = new int[column];
            for (int columnIndex = 0; columnIndex < matrix[rowIndex].Length; columnIndex++)
            {
                matrix[rowIndex][columnIndex] = count++;
            }
        }
        var result = SpiralOrder(matrix);
        foreach (var value in result)
        {
            Console.Write($"{value}, ");
        }
    }

    public int[] SpiralOrder(int[][] matrix)
    {
        if (matrix.Length == 0 || matrix[0].Length == 0) { return new int[0]; }
        int rank0 = matrix.Length;
        int rank1 = matrix[0].Length;
        int[] result = new int[rank0 * rank1];
        int round = 0;
        int index = 0;
        while (round <= System.Math.Ceiling(rank0 / 2f) - 1 && round <= System.Math.Ceiling(rank1 / 2f) - 1)
        {
            // 朝右
            for (int row = round, column = round; column < rank1 - 1 * round; column++)
            {
                result[index++] = matrix[row][column];
            }
            if (rank0 - round - 1 > round)
            {
                // 朝下
                for (int row = round + 1, column = rank1 - round - 1; row < rank0 - round; row++)
                {
                    result[index++] = matrix[row][column];
                }
                if (round < rank1 - round - 1)
                {
                    // 朝左
                    for (int row = rank0 - round - 1, column = rank1 - round - 2; column >= round; column--)
                    {
                        result[index++] = matrix[row][column];
                    }
                    // 朝上
                    for (int row = rank0 - round - 2, column = round; row >= 1 * round + 1; row--)
                    {
                        result[index++] = matrix[row][column];
                    }
                }
            }
            round++;
        }
        return result;
    }
}
