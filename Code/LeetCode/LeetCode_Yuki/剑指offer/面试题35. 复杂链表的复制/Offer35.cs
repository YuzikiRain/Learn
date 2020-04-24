using System.Collections.Generic;

public class Offer35
{
    public class Node
    {
        public int val;
        public Node next;
        public Node random;

        public Node() { }
        public Node(int _val, Node _next, Node _random)
        {
            val = _val;
            next = _next;
            random = _random;
        }
    }

    public void Test()
    {
        Node node0 = new Node(7, null, null);
        Node node1 = new Node(13, null, null);
        Node node2 = new Node(11, null, null);
        Node node3 = new Node(10, null, null);
        Node node4 = new Node(1, null, null);
        node0.next = node1;
        node0.random = null;
        node1.next = node2;
        node1.random = node0;
        node2.next = node3;
        node2.random = node4;
        node3.next = node4;
        node3.random = node2;
        node4.next = null;
        node4.random = node0;

        Node result = CopyRandomList(node0);

    }

    public Node CopyRandomList(Node head)
    {
        if (head == null) { return null; }
        // 建立node到index的映射
        Dictionary<Node, int> nodeToIndex = new Dictionary<Node, int>();
        Node node = head;
        int index = 0;
        while (node != null)
        {
            nodeToIndex[node] = index;
            index++;
            node = node.next;
        }
        // 复制节点并设置next
        Dictionary<int, Node> indexToNode = new Dictionary<int, Node>();
        Node newNodeCurrent = new Node(head.val, null, null);
        Node next = head.next;
        Node newHead = newNodeCurrent;
        // indexToNode[0] = newNodeCurrent;
        index = 0;
        while (next != null)
        {
            indexToNode[index] = newNodeCurrent;
            newNodeCurrent.next = new Node(next.val, null, null);
            newNodeCurrent = newNodeCurrent.next;
            next = next.next;
            index++;
        }
        indexToNode[index] = newNodeCurrent;
        // 根据原List的Node -> index
        newNodeCurrent = newHead;
        var current = head;
        while (current != null)
        {
            if (current.random != null)
            {
                index = nodeToIndex[current.random];
                newNodeCurrent.random = indexToNode[index];
            }
            current = current.next;
            newNodeCurrent = newNodeCurrent.next;
        }
        return newHead;
    }
}