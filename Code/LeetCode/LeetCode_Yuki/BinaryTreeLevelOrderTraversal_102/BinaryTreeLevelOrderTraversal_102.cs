using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Yuki.BinaryTreeLevelOrderTraversal_102
{
    class BinaryTreeLevelOrderTraversal_102
    {
        public void Test()
        {
            TreeNode root = new TreeNode(3);
            root.left = new TreeNode(9);
            root.right = new TreeNode(20);
            root.right.left = new TreeNode(15);
            root.right.right = new TreeNode(7);

            IList<IList<int>> list = LevelOrder(root);
            foreach(var l in list)
            {
                foreach(var element in l)
                {
                    Console.Write($"{element}, ");
                }
                Console.WriteLine();
            }
        }

        public IList<IList<int>> LevelOrder(TreeNode root)
        {
            IList<IList<int>> list = new List<IList<int>>();
            if (root == null)
            {
                return list as IList<IList<int>>;
            }

            Queue<TreeNode> children = new Queue<TreeNode>();
            children.Enqueue(root);

            while (children.Count > 0)
            {
                List<int> level = new List<int>();
                list.Add(level);
                int count = children.Count;

                while (count-- > 0)
                {
                    TreeNode node = children.Dequeue();

                    if (node != null)
                    {
                        level.Add(node.val);
                        if (node.left != null)
                            children.Enqueue(node.left);
                        if (node.right != null)
                            children.Enqueue(node.right);
                    }
                }
            }

            return list;
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
