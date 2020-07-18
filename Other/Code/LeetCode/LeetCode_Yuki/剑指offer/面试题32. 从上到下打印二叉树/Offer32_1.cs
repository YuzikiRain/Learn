using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.剑指offer.面试题32___I._从上到下打印二叉树
{
    class Offer32_1
    {
        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int x) { val = x; }
        }

        private Queue<TreeNode> _queue = new Queue<TreeNode>();
        public int[] LevelOrder(TreeNode root)
        {
            // 先分配足够的空间，最后根据访问的节点数resize
            int count = 0;
            int[] result;
            result = new int[1100];
            if (root != null) { _queue.Enqueue(root); }
            while (_queue.Count > 0)
            {
                root = _queue.Dequeue();
                result[count] = root.val;
                count++;
                if (root.left != null)
                {
                    _queue.Enqueue(root.left);
                }
                if (root.right != null)
                {
                    _queue.Enqueue(root.right);
                }
            }

            System.Array.Resize(ref result, count);
            return result;
        }
    }
}
