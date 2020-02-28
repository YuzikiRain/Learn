using System;

namespace LeetCode_Yuki.RemoveNthFromEnd_19
{
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }
    class RemoveNthFromEnd_19
    {
        public void Test()
        {
            ListNode head = new ListNode(1);
            //head.next = new ListNode(2);
            //head.next.next = new ListNode(3);
            //head.next.next.next = new ListNode(4);
            //head.next.next.next.next = new ListNode(5);

            head = RemoveNthFromEnd(head, 1);
            ListNode temp = head;
            while (temp != null)
            {
                Console.WriteLine(temp.val);
                temp = temp.next;
            }
        }

        public ListNode RemoveNthFromEnd(ListNode head, int n)
        {
            ListNode i = head;
            ListNode j = head;

            if (n <= 1)
            {
                j = j.next;
                // head.next is null, it means list count is 1, so delete head, return null 
                if (j == null)
                {
                    return null;
                }
                while (j.next != null)
                {
                    i = i.next;
                    j = j.next;
                }
                //j = null;
                i.next = null;
            }
            else
            {
                while (j != null && n-- > 1)
                {
                    j = j.next;
                }
                while (j.next != null)
                {
                    i = i.next;
                    j = j.next;
                }
                i.val = i.next.val;
                i.next = i.next.next;
            }
            return head;
        }
    }
}
