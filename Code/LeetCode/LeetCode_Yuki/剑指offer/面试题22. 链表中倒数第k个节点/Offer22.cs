
class Offer22
{
    public void Test()
    {
        int count = 5;
        ListNode head = new ListNode(1);
        var node = head;
        for (int i = 2; i < count; i++)
        {
            node = node.next = new ListNode(i);
        }
    }

    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }

    public ListNode GetKthFromEnd(ListNode head, int k)
    {
        if (head == null || k <= 0) { return null; }
        ListNode tail = head;
        while (k > 1)
        {
            tail = tail.next;
            k--;
            if (tail == null) { return null; }
        }
        // TODO: 处理非法的k（k<=0)，越界的k，head==null

        while (tail.next != null)
        {
            tail = tail.next;
            head = head.next;
        }
        return head;
    }
}
