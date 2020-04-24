using System;

namespace LeetCode_Yuki.LinkedListCycle_II_142
{
    class LinkedListCycle_II_142
    {
        public void Test()
        {
            ListNode head = new ListNode(0);
            ListNode current = head;

            //int pos = 0;
            //int[] array = new int[] { -21, 10, };
            int pos = 2;
            int[] array = new int[] { -21, 10, 17, 8, };

            foreach (var value in array)
            {
                current.next = new ListNode(value);
                current = current.next;
            }
            head = head.next;
            ListNode tail = current;
            if (pos != -1)
            {
                current = head;
                while (pos-- > 0)
                {
                    current = current.next;
                }
                tail.next = current;
            }
            else
            {
                tail.next = null;
            }

            ListNode cycleEntrance = DetectCycle(head);
            Console.WriteLine(cycleEntrance?.val);
        }

        public ListNode DetectCycle(ListNode head)
        {
            ListNode nodeInCycle = FindNodeInCycle(head);
            if (nodeInCycle != null)
            {
                int length = GetLengthOfCycle(nodeInCycle);
                ListNode slow = head;
                ListNode fast = head;
                while (length > 0)
                {
                    fast = fast.next;
                    length--;
                }

                while (slow != fast)
                {
                    slow = slow.next;
                    fast = fast.next;
                }

                return fast;
            }
            else
            {
                return null;
            }
        }

        public ListNode FindNodeInCycle(ListNode head)
        {
            // situation: list has no element or no cycle
            if (head == null)
                return null;
            ListNode slow = head;
            ListNode fast = slow.next;
            // situation: list has only one element and no cycle
            if (slow == null)
                return null;

            // situation: length of list >= 2 or list has cycle
            while (fast != null)
            {
                slow = slow.next;
                fast = fast.next;
                if (fast != null)
                    fast = fast.next;
                else
                    break;
                if (slow == fast)
                {
                    return fast;
                }
            }

            // situation: length of list >= 2 and list has no cycle
            return null;
        }

        public int GetLengthOfCycle(ListNode nodeInCycle)
        {
            ListNode start = nodeInCycle.next;
            int count = 1;
            while (start != nodeInCycle)
            {
                start = start.next;
                count++;
            }

            return count;
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
