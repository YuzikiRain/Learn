using System;
using System.Collections.Generic;

namespace LeetCode_Yuki.ImplementQueueUsingStacks_232
{
    class ImplementQueueUsingStacks
    {
        public void Test()
        {
            MyQueue queue = new MyQueue();

            queue.Push(1);
            queue.Push(2);
            Console.WriteLine(queue.Peek());  // 返回 1
            Console.WriteLine(queue.Pop());   // 返回 1
            Console.WriteLine(queue.Empty()); // 返回 false
        }
    }

    public class MyQueue
    {
        Stack<int> _stackPush = new Stack<int>();
        Stack<int> _stackPop = new Stack<int>();

        /** Push element x to the back of queue. */
        public void Push(int x)
        {
            _stackPush.Push(x);
        }

        /** Removes the element from in front of queue and returns that element. */
        public int Pop()
        {
            if (_stackPop.Count == 0)
            {
                while (_stackPush.Count > 0)
                {
                    _stackPop.Push(_stackPush.Pop());
                }

            }
            return _stackPop.Pop();
        }

        /** Get the front element. */
        public int Peek()
        {
            if (_stackPop.Count == 0)
            {
                while (_stackPush.Count > 0)
                {
                    _stackPop.Push(_stackPush.Pop());
                }

            }
            return _stackPop.Peek();
        }

        /** Returns whether the queue is empty. */
        public bool Empty()
        {
            return _stackPop.Count == 0 && _stackPush.Count == 0;
        }
    }

    /**
     * Your MyQueue object will be instantiated and called as such:
     * MyQueue obj = new MyQueue();
     * obj.Push(x);
     * int param_2 = obj.Pop();
     * int param_3 = obj.Peek();
     * bool param_4 = obj.Empty();
     */
}
