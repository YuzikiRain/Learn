using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class Extensions
{
    public static async void WrapErrors(this Task task)
    {
        await task;
    }
}

class Program
{
    static int progress = 0;
    static int speed = 10;
    static event Action<string> EnterScene;
    static int interval = 500;
    static void Main(string[] args)
    {
        var test = new LeetCode_Qinyu.ImplementQueueUsingStacks_232.ImplementQueueUsingStacks();
        test.Test();

        #region 旧的

        //500. 键盘行
        //var keyboardRow = new LeetCode_Qinyu.KeyboardRow.KeyboardRow();
        //keyboardRow.Test();

        //557. 反转字符串中的单词 III
        //var reverseWords_557 = new LeetCode_Qinyu.ReverseWords.ReverseWords_557();
        //reverseWords_557.Test();

        //728. 自除数
        //var selfDividingNumbers = new LeetCode_Qinyu.SelfDividingNumbers.SelfDividingNumbers_728();
        //selfDividingNumbers.Test();

        //867. 转置矩阵
        //var transpose_867 = new LeetCode_Qinyu.Transpose.Transpose_867();
        //transpose_867.Test();

        //171. Excel表列序号
        //var titleToNumber_171 = new LeetCode_Qinyu.TitleToNumber.TitleToNumber_171();
        //titleToNumber_171.Test();

        //559. N叉树的最大深度
        //var maxDepthOfNAryTree_559 = new LeetCode_Qinyu.MaxDepthOfNAryTree.MaxDepthOfNAryTree_559();
        //maxDepthOfNAryTree_559.Test();

        //821. 字符的最短距离
        //var shortestToChar_821 = new LeetCode_Qinyu.ShortestToChar.ShortestToChar_821();
        //shortestToChar_821.Test();

        //589. N叉树的前序遍历
        //var naryTreePreoderTraversal = new LeetCode_Qinyu.NaryTreePreoderTraversal.NaryTreePreoderTraversal_589();
        //naryTreePreoderTraversal.Test();

        // 338. 比特位计数
        //var countingBits338 = new LeetCode_Qinyu.CountingBits338.CountingBits338();
        //countingBits338.Test();
        #endregion

        // 46. Permutations
        //var permutations46 = new LeetCode_Qinyu.Permutations46.Permutations();
        //permutations46.Test();

        //EnterScene += LoadSceneAsync;
        //EnterScene("战斗界面");
        //Console.WriteLine("触发进入下一场景事件");
        //Console.WriteLine("不阻塞，继续触发后的处理");
        //Console.WriteLine("显示 Loading");
        //Console.ReadLine();

        ////693. 是否是交替位二进制数
        //var hasAlternatingBits = new LeetCode_Qinyu.HasAlternatingBits.HasAlternatingBits_693();
        //hasAlternatingBits.Test();

        //344. 反转字符串
        //ReverseStringTest reverseStringTest = new ReverseStringTest();
        //Console.WriteLine(reverseStringTest.ReverseString("A man, a plan, a canal: Panama"));

        //104.二叉树的最大深度
        //TreeNode root_maxDepthOfBSTTest = new TreeNode(3);
        //root_maxDepthOfBSTTest.left = new TreeNode(9);
        //root_maxDepthOfBSTTest.right = new TreeNode(20);
        //root_maxDepthOfBSTTest.right.left = new TreeNode(15);
        //root_maxDepthOfBSTTest.right.right = new TreeNode(7);

        //MaxDepthOfBSTTest maxDepthOfBSTTest = new MaxDepthOfBSTTest();
        //maxDepthOfBSTTest.MaxDepth(root_maxDepthOfBSTTest);
        Console.ReadLine();
    }

    static async void LoadSceneAsync(string sceneName)
    {
        Console.WriteLine($"准备进入 {sceneName}");

        Console.WriteLine("开始读取资源");
        //int length = await LoadResources();
        //LoadResources().WrapErrors();
        LoadResources();
        //await LoadResources();
        //Console.WriteLine($"资源长度：{length}");
        //await Task.Run(() => { LoadResources(); });
        Console.WriteLine("读取资源完成");

        Console.WriteLine($"进入 {sceneName}");
    }

    static async Task<int> LoadResources()
    {
        Console.WriteLine("正在读取资源");

        var httpClient = new System.Net.Http.HttpClient();
        int exampleInt = (await httpClient.GetStringAsync("http://msdn.microsoft.com")).Length;
        Console.WriteLine("返回资源");
        return exampleInt;
    }
}

public class ReverseStringTest
{
    public string ReverseString(string s)
    {
        StringBuilder str_new = new StringBuilder(s);

        for (int i = 0, j = s.Length - 1; i < j; i++, j--)
        {
            char temp = s[i];
            str_new[i] = s[j];
            str_new[j] = s[i];
        }
        return str_new.ToString();
    }
}

public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int x) { val = x; }
}

public class MaxDepthOfBSTTest
{
    int depth_max = 0;
    public int MaxDepth(TreeNode root)
    {
        int depth = 0;
        PreorderTraverse(root, depth);
        return depth_max;
    }

    void PreorderTraverse(TreeNode node, int depth)
    {
        if (node != null)
        { depth += 1; if (depth > depth_max) depth_max = depth; }
        else { return; }

        PreorderTraverse(node.left, depth);
        PreorderTraverse(node.right, depth);
    }
}

