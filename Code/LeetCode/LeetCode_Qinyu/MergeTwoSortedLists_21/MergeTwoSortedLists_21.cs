using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.MergeTwoSortedLists_21
{
    class MergeTwoSortedLists_21
    {
        public void Test()
        {
            ListNode head1 = new ListNode(0);
            ListNode current = head1;
            int pos = -1;
            int[] array1 = new int[] { 1, 3, 5, 7 };
            foreach (var value in array1)
            {
                current.next = new ListNode(value);
                current = current.next;
            }
            head1 = head1.next;


            ListNode head2 = new ListNode(0);
            current = head2;
            int[] array2 = new int[] { 2, 4, 6, 8 };
            foreach (var value in array2)
            {
                current.next = new ListNode(value);
                current = current.next;
            }
            head2 = head2.next;

            ListNode head = MergeTwoLists(head1, head2);
            while (head != null)
            {
                Console.WriteLine(head.val);
                head = head.next;
            }

        }

        #region by recursion

        public ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if (l1 == null)
                return l2;
            else if (l2 == null)
                return l1;
            else
            {
                ListNode node = null;
                if (l1.val < l2.val)
                {
                    node = l1;
                    l1 = l1.next;
                }
                else
                {
                    node = l2;
                    l2 = l2.next;
                }
                node.next = MergeTwoLists(l1, l2);
                return node;
            }
        }

        #endregion

        #region by iteration

        //public ListNode MergeTwoLists(ListNode l1, ListNode l2)
        //{
        //    // we suppose that there is a node in front of head
        //    ListNode head = new ListNode(0);
        //    ListNode current = head;
        //    while (l1 != null || l2 != null)
        //    {
        //        int value1 = l1 == null ? int.MaxValue : l1.val;
        //        int value2 = l2 == null ? int.MaxValue : l2.val;
        //        if (value1 < value2)
        //        {
        //            current.next = new ListNode(value1);
        //            l1 = l1.next;
        //        }
        //        else
        //        {
        //            current.next = new ListNode(value2);
        //            l2 = l2.next;
        //        }
        //        current = current.next;
        //    }

        //    head = head.next;
        //    return head;
        //}

        #endregion
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
