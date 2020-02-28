using System;

namespace LeetCode_Yuki.ReverseLinkedList_92
{
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }

    class ReverseLinkedList_92
    {
        public void Test()
        {
            ListNode head = new ListNode(1);
            head.next = new ListNode(2);
            //head.next.next = new ListNode(3);
            //head.next.next.next = new ListNode(4);
            //head.next.next.next.next = new ListNode(5);

            head = ReverseBetween(head, 1, 2);

            while (head != null)
            {
                Console.WriteLine(head.val);
                head = head.next;
            }
        }

        public ListNode ReverseBetween(ListNode head, int m, int n)
        {
            ListNode one = null;
            ListNode two = null;
            if (m <= 1)
            {
                one = null;
                two = head;
            }
            else
            {
                one = head;
                for (int i = 0; i < m - 2; i++)
                {
                    one = one.next;
                }
                two = one.next;
            }

            // reverse node between m and n
            ListNode current = two;
            ListNode last = null;
            ListNode next = null;
            int length = n - m + 1;
            while (length-- > 0 && current != null)
            {
                //save reference of head.next
                next = current.next;
                //reverse
                current.next = last;
                // move index to next
                last = current;
                current = next;
            }

            if (one != null)
            {
                one.next = last;
            }
            else
            {
                head = last;
            }

            if (two != null)
            {
                two.next = current;
            }

            return head;
        }
    }
}
