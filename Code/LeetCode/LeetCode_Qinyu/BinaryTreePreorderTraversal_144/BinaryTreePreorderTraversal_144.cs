using System;
using System.Collections.Generic;

namespace LeetCode_Qinyu.BinaryTreePreorderTraversal_144
{
    class BinaryTreePreorderTraversal_144
    {
        public void Test()
        {
            //TreeNode root = null;
            TreeNode root = new TreeNode(1);
            root.right = new TreeNode(2);
            root.right.left = new TreeNode(3);

            IList<int> list = PreorderTraversal(root);
            foreach(var element in list)
            {
                Console.WriteLine(element);
            }
        }

        #region by iteration

        public IList<int> PreorderTraversal(TreeNode root)
        {
            IList<int> result = new List<int>();
            Stack<TreeNode> stack = new Stack<TreeNode>();

            while (true)
            {
                while (root != null)
                {
                    // visit root
                    result.Add(root.val);
                    // cache root's right subtree
                    if (root.right != null)
                        stack.Push(root.right);
                    // visit along with left subtree
                    root = root.left;
                }

                // no left subtree, so pop cached right subtree to continue
                if (stack.Count > 0)
                {
                    root = stack.Pop();
                }
                // no cached right subtree, end
                else
                {
                    break;
                }
            }

            return result;
        }

        #endregion

        #region by recursion

        //public IList<int> PreorderTraversal(TreeNode root)
        //{
        //    IList<int> result = new List<int>();
        //    PreorderTraversalByRecursion(root, result);
        //    return result;
        //}

        //public void PreorderTraversalByRecursion(TreeNode root, IList<int> result)
        //{
        //    if (root != null)
        //    {
        //        result.Add(root.val);
        //        PreorderTraversalByRecursion(root.left, result);
        //        PreorderTraversalByRecursion(root.right, result);
        //    }
        //}

        #endregion

        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int x) { val = x; }
        }
    }
}
