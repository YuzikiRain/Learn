using System;

namespace LeetCode_Qinyu.LinkedListCycle_141
{
    class LinkedListCycle_141
    {
        public void Test()
        {
            ListNode head = new ListNode(0);
            ListNode current = head;
            int pos = -1;
            int[] array = new int[] { -21, 10, 17, 8, 4, 26, 5, 35, 33, -7, -16, 27, -12, 6, 29, -12, 5, 9, 20, 14, 14, 2, 13, -24, 21, 23, -21, 5 };
            foreach(var value in array)
            {
                current.next = new ListNode(value);
                current = current.next;
            }
            head = head.next;
            ListNode tail = current;
            if (pos != -1)
            {
                current = head;
                while ( pos-- > 0)
                {
                    current = current.next;
                }
                tail.next = current;
            }
            else
            {
                tail.next = null;
            }

            bool result = HasCycle(head);
            Console.WriteLine(result);
        }

        public bool HasCycle(ListNode head)
        {
            if (head == null || head.next == null)
                return false;

            ListNode fast = head;
            ListNode slow = head;
            fast = fast.next;
            // situation: list has only 1 element and no cycle
            if (fast == null)
                return false;

            while (fast != slow)
            {
                slow = slow.next;
                fast = fast.next;
                if (fast != null)
                    fast = fast.next;
                if (fast == null || slow == null)
                    return false;
            }
            return true;
        }

        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int x)
            {
                val = x;
                next = null;
            }
        }
    }
}
