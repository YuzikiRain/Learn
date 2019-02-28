using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.MinStack_155
{
    class MinStack_155
    {
        public void Test()
        {
            MinStack obj = new MinStack();
            obj.Push(-2);
            obj.Push(0);
            obj.Push(-1);
            int param_1 = obj.GetMin();
            int param_2 = obj.Top();
            obj.Pop();
            int param_3 = obj.GetMin();
        }
    }

    public class MinStack
    {
        Stack<int> _stack = new Stack<int>();
        Stack<int> _stackMin = new Stack<int>();
        /** initialize your data structure here. */
        public MinStack()
        {

        }

        public void Push(int x)
        {
            if (_stack.Count > 0 && _stackMin.Count > 0)
            {
                _stackMin.Push(Math.Min(_stackMin.Peek(), x));
            }
            else
            {
                _stackMin.Push(x);
            }

            _stack.Push(x);
        }

        public void Pop()
        {
            _stack.Pop();
            _stackMin.Pop();
        }

        public int Top()
        {
            return _stack.Peek();
        }

        public int GetMin()
        {
            return _stackMin.Peek();
        }
    }
}
