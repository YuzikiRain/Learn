using System;

namespace LeetCode_Qinyu.ReverseLinkedList_206
{
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }

    class ReverseLinkedList_206
    {
        public void Test()
        {
            ListNode head = new ListNode(1);
            head.next = new ListNode(2);
            head.next.next = new ListNode(3);
            head.next.next.next = new ListNode(4);
            head.next.next.next.next = new ListNode(5);

            head = ReverseList(head);
            while (head != null)
            {
                Console.WriteLine(head.val);
                head = head.next;
            }

        }

        #region by iteration

        public ListNode ReverseList(ListNode head)
        {
            ListNode last = null;
            ListNode next = null;

            while (head != null)
            {
                //save reference of head.next
                next = head.next;
                //reverse
                head.next = last;
                // move index to next
                last = head;
                head = next;
            }

            return last;
        }

        #endregion

        #region by recursion

        //public ListNode ReverseList(ListNode head)
        //{
        //    return Reverse(head, null);
        //}

        //private ListNode Reverse(ListNode current, ListNode parent)
        //{
        //    if (current != null)
        //    {
        //        ListNode head = Reverse(current.next, current);
        //        current.next = parent;
        //        return head;
        //    }
        //    else
        //    {
        //        return parent;
        //    }
        //}

        #endregion
    }
}
