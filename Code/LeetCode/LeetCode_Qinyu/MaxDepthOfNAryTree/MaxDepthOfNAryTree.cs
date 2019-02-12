using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.MaxDepthOfNAryTree
{

    //Definition for a Node.
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

    class MaxDepthOfNAryTree_559
    {
        int depth_max = 0;

        public void Test()
        {
            Node root = new Node(1, new List<Node>());
            root.children.Add(new Node(3, new List<Node>()));
            root.children.Add(new Node(2, new List<Node>()));
            root.children.Add(new Node(4, new List<Node>()));

            root.children[0].children.Add(new Node(5, new List<Node>()));
            root.children[0].children.Add(new Node(6, new List<Node>()));

            int max = MaxDepth(root);
            Console.WriteLine($"max = {max}");
            //Console.ReadLine();
        }

        public int MaxDepth(Node root)
        {
            int depth = 0;

            Traverse(root, depth);
            

            return depth_max;
        }

        void Traverse(Node node_parent, int depth)
        {
            if (node_parent != null)
            {
                depth++;
                if (depth > depth_max) { depth_max = depth; }
            }
            else { return; }

            foreach (var node in node_parent.children)
            {
                Traverse(node, depth);
            }
        }
    }
}
