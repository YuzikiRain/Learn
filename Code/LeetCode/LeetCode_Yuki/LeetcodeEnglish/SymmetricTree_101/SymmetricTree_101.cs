using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Yuki.SymmetricTree_101
{
    class SymmetricTree_101
    {
        public void Test()
        {
            //TreeNode root = new TreeNode(1);
            //root.left = new TreeNode(2);
            //root.right = new TreeNode(2);
            //root.left.right = new TreeNode(3);
            //root.right.right = new TreeNode(3);

            TreeNode root = new TreeNode(1);
            root.left = new TreeNode(2);
            root.right = new TreeNode(2);
            root.left.left = new TreeNode(3);
            root.left.right = new TreeNode(4);
            root.right.left = new TreeNode(4);
            root.right.right = new TreeNode(3);

            Console.WriteLine(IsSymmetric(root));
        }

        public bool IsSymmetric(TreeNode root)
        {
            // left node value equals right node
            if (root == null)
            {
                return true;
            }
            return IsSymmetric(root.left, root.right);
        }

        public bool IsSymmetric(TreeNode left, TreeNode right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            else
            {
                // one subtree is null and the other one is not
                if (left == null || right == null)
                {
                    return false;
                }
                // left node value not equals right node
                if (left.val != right.val)
                {
                    return false;
                }
                // left subtree is symmetric to right subtree
                return IsSymmetric(left.left, right.right) &&
                    IsSymmetric(left.right, right.left);
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
}
