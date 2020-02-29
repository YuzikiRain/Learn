using System;

class Offer26
{
    public void Test()
    {
        TreeNode rootA = new TreeNode(3);
        rootA.left = new TreeNode(4);
        rootA.right = new TreeNode(5);
        rootA.left.left = new TreeNode(1);
        rootA.left.right = new TreeNode(2);

        TreeNode rootB = new TreeNode(4);
        rootA.left = new TreeNode(1);

        Console.WriteLine($"{IsSubStructure(rootA, rootB)}");
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int x) { val = x; }
    }

    public bool IsSubStructure(TreeNode A, TreeNode B)
    {
        if (B == null) { return false; }
        return FindSubtreeRoot(A, B);
    }

    private bool HasSubtree(TreeNode A, TreeNode B)
    {
        // B节点为null，可将该节点看作是匹配任意值的节点
        if (B == null) { return true; }
        // B不为null，但A为null，说明B树的该节点不包含于A树
        else if (B != null && A == null) { return false; }
        // 两者都不为null
        else
        {
            return A.val == B.val && HasSubtree(A.left, B.left) && HasSubtree(A.right, B.right);
        }
    }

    private bool FindSubtreeRoot(TreeNode A, TreeNode B)
    {
        if (A == null) { return false; }
        else if (A.val == B.val) { return HasSubtree(A, B); }
        else
        {
            var isFound = FindSubtreeRoot(A.left, B);
            return FindSubtreeRoot(A.right, B) || isFound;
        }
    }
}
