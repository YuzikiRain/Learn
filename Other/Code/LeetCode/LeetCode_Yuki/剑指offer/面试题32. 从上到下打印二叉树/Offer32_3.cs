using System.Collections.Generic;

class Offer32_3
{
    public void Test()
    {
        TreeNode root = new TreeNode(3);
        root.left = new TreeNode(9);
        root.right = new TreeNode(20);
        root.right.left = new TreeNode(15);
        root.right.right = new TreeNode(7);

        var result = LevelOrder(root);
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int x) { val = x; }
    }

    private Stack<TreeNode> _stack1 = new Stack<TreeNode>();
    private Stack<TreeNode> _stack2 = new Stack<TreeNode>();

    public IList<IList<int>> LevelOrder(TreeNode root)
    {
        if (root != null) { _stack1.Push(root); }
        List<IList<int>> result = new List<IList<int>>();

        while (_stack1.Count > 0 || _stack2.Count > 0)
        {
            var level = new List<int>();
            result.Add(level);
            while (_stack1.Count > 0)
            {
                var node = _stack1.Pop();
                level.Add(node.val);
                System.Console.WriteLine($"node = {node.val} left = {(node.left != null ? node.left.val.ToString() : string.Empty)} right = {(node.right != null ? node.right.val.ToString() : string.Empty)}");
                if (node.left != null) { _stack2.Push(node.left); }
                if (node.right != null) { _stack2.Push(node.right); }
            }
            if (level.Count == 0) { result.Remove(level); }
            var level2 = new List<int>();
            result.Add(level2);
            while (_stack2.Count > 0)
            {
                var node = _stack2.Pop();
                level2.Add(node.val);
                System.Console.WriteLine($"node = {node.val} left = {(node.left != null ? node.left.val.ToString() : string.Empty)} right = {(node.right != null ? node.right.val.ToString() : string.Empty)}");
                if (node.right != null) { _stack1.Push(node.right); }
                if (node.left != null) { _stack1.Push(node.left); }
            }
            if (level2.Count == 0) { result.Remove(level2); }
        }

        return result;
    }
}
