using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.NaryTreePreoderTraversal
{
    // Definition for a Node.
    public class Node
    {
        public int val;
        public IList<Node> children;

        public Node() { }
        public Node(int _val, IList<Node> _children)
        {
            val = _val;
            children = _children;
        }
    }

    class NaryTreePreoderTraversal_589
    {
        public void Test()
        {
            Node root = new Node(1, new List<Node>());

            root.children.Add(new Node(3, new List<Node>()));
            root.children[0].children.Add(new Node(5, new List<Node>()));
            root.children[0].children.Add(new Node(6, new List<Node>()));

            root.children.Add(new Node(2, new List<Node>()));
            root.children.Add(new Node(4, new List<Node>()));

            foreach (var element in Preorder(root))
            { Console.Write($"{element}, "); }

            //Console.ReadLine();
        }


        public IList<int> Preorder(Node root)
        {
            Node node = root;
            List<int> result = new List<int>();

            Stack<Node> stack = new Stack<Node>();

            while(stack.Count != 0 || node != null)
            {
                if (node != null) { result.Add(node.val); }

                for (int i = node.children.Count - 1; i >= 1; i--)
                {
                    stack.Push(node.children[i]);
                }

                node = node.children.Count > 0 ? node.children.First() : null;
                if(node == null && stack.Count > 0)
                {
                    node = stack.Pop();
                }
                
            }

            return result;
        }

    }

}
