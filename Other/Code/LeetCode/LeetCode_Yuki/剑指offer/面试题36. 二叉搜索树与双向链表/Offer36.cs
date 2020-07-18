class Offer36
{
    public void Test()
    {
        Node root = new Node(4, null, null);
        root.left = new Node(2, null, null);
        root.right = new Node(5, null, null);
        root.left.left = new Node(1, null, null);
        root.left.right = new Node(3, null, null);
        Node head = TreeToDoublyList(root);
    }

    // Definition for a Node.
    public class Node
    {
        public int val;
        public Node left;
        public Node right;

        public Node() { }
        public Node(int _val, Node _left, Node _right)
        {
            val = _val;
            left = _left;
            right = _right;
        }
    }

    private int _index = 0;
    Node[] _nodes = new Node[2000];
    public Node TreeToDoublyList(Node root)
    {
        if (root == null) { return null; }
        // 找出最小值的节点作为head
        Node min = root;
        while (min.left != null)
        {
            min = min.left;
        }
        InOrderTraverse(root);
        System.Array.Resize<Node>(ref _nodes, _index);
        _index = 0;
        for (; _index < _nodes.Length && _nodes[_index] != null; _index++)
        {
            int left = _index - 1;
            if (left < 0) { left = _nodes.Length - 1; }
            int right = _index + 1;
            if (right > _nodes.Length - 1) { right = 0; }
            _nodes[_index].left = _nodes[left];
            _nodes[_index].right = _nodes[right];
        }
        return min;
    }

    private void InOrderTraverse(Node root)
    {
        if (root == null) { return; }
        InOrderTraverse(root.left);
        _nodes[_index++] = root;
        InOrderTraverse(root.right);
    }
}