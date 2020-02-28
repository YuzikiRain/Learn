using System.Collections.Generic;

class Offer32_2
{
    public void Test()
    {
        TreeNode root = new TreeNode(3);
        root.left = new TreeNode(9);
        root.right = new TreeNode(20);
        root.right.left = new TreeNode(15);
        root.right.right = new TreeNode(20);

        var result = LevelOrder(root);
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int x) { val = x; }
    }

    private Queue<TreeNode> _queue1 = new Queue<TreeNode>();
    private Queue<TreeNode> _queue2 = new Queue<TreeNode>();

    public IList<IList<int>> LevelOrder(TreeNode root)
    {
        if (root != null) { _queue1.Enqueue(root); }
        List<IList<int>> result = new List<IList<int>>();

        while (_queue1.Count > 0 || _queue2.Count > 0)
        {
            var level = new List<int>();
            result.Add(level);
            while (_queue1.Count > 0)
            {
                var node = _queue1.Dequeue();
                level.Add(node.val);
                System.Console.WriteLine($"node = {node.val} left = {(node.left != null ? node.left.val.ToString() : string.Empty)} right = {(node.right != null ? node.right.val.ToString() : string.Empty)}");
                if (node.left != null) { _queue2.Enqueue(node.left); }
                if (node.right != null) { _queue2.Enqueue(node.right); }
            }
            if (level.Count == 0) { result.Remove(level); }
            var level2 = new List<int>();
            result.Add(level2);
            while (_queue2.Count > 0)
            {
                var node = _queue2.Dequeue();
                level2.Add(node.val);
                System.Console.WriteLine($"node = {node.val} left = {(node.left != null ? node.left.val.ToString() : string.Empty)} right = {(node.right != null ? node.right.val.ToString() : string.Empty)}");
                if (node.left != null) { _queue1.Enqueue(node.left); }
                if (node.right != null) { _queue1.Enqueue(node.right); }
            }
            if (level2.Count == 0) { result.Remove(level2); }
        }

        return result;
    }
}
