using System.Collections.Generic;

class Offer34
{
    public void Test()
    {
        TreeNode root = new TreeNode(-2);
        root.right = new TreeNode(-3);

        //root.left = new TreeNode(4);
        //root.right = new TreeNode(8);
        //root.left.left = new TreeNode(11);
        //root.left.left.left = new TreeNode(7);
        //root.left.left.right = new TreeNode(2);
        //root.right.left = new TreeNode(13);
        //root.right.right = new TreeNode(4);
        //root.right.right.left = new TreeNode(5);
        //root.right.right.right = new TreeNode(1);

        PathSum(root, -5);
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int x) { val = x; }
    }

    List<IList<int>> _result = new List<IList<int>>();
    public IList<IList<int>> PathSum(TreeNode root, int sum)
    {
        if (root == null) { return _result; }
        var list = new List<int>();
        _result.Add(list);
        PathSum(root, sum, list);
        return _result;
    }

    private void PathSum(TreeNode root, int sum, List<int> list)
    {
        if (root == null) { _result.Remove(list); return; }
        if (root.val == sum)
        {
            list.Add(root.val);
        }
        else { list.Add(root.val); }
        if (root.left != null)
        {
            var rightList = new List<int>(list);
            PathSum(root.left, sum - root.val, list);
            // 如果左子树和右子树都存在，那么为右子树新建list
            if (root.right != null)
            {
                _result.Add(rightList);
                PathSum(root.right, sum - root.val, rightList);
            }
        }
        else
        {
            if (root.right != null)
            {
                PathSum(root.right, sum - root.val, list);
            }
            // 叶节点
            else
            {
                if (sum == root.val) { return; }
                else
                {
                    _result.Remove(list);
                    return;
                }
            }

        }
    }
}
