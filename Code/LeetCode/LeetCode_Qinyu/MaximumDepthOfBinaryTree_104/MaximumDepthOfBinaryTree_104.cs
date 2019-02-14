namespace LeetCode_Qinyu.MaximumDepthOfBinaryTree_104
{
    class MaximumDepthOfBinaryTree_104
    {
        public void Test()
        {
            TreeNode root = new TreeNode(3);
            root.left = new TreeNode(9);
            root.right = new TreeNode(20);
            root.right.left = new TreeNode(15);
            root.right.right = new TreeNode(7);
            var result = MaxDepth(root);
            System.Console.WriteLine($"MaximumDepthOfBinaryTree is {result}");
        }

        public int MaxDepth(TreeNode root)
        {
            // means its' parent is leaf node
            if (root == null)
            {
                return 0;
            }
            else
            {
                int left = MaxDepth(root.left);
                int right = MaxDepth(root.right);
                // this node exists, so depth add one
                return left > right ? left + 1 : right + 1;
            }
        }

    }

     public class TreeNode
    {
         public int val;
          public TreeNode left;
          public TreeNode right;
          public TreeNode(int x) { val = x; }
      }

}
