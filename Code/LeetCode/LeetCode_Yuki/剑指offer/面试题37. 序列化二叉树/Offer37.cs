using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Offer37
{
    public void Test()
    {
        TreeNode root = new TreeNode(-1);
        root.left = new TreeNode(0);
        root.right = new TreeNode(1);
        deserialize(serialize(root));
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int x) { val = x; }
    }

    private const string Null = "*";
    private StringBuilder _builder = new StringBuilder(1200);
    private string[] _datas;
    private int _index = 0;
    // Encodes a tree to a single string.
    public string serialize(TreeNode root)
    {
        PreorderTraverse(root);
        return _builder.ToString();
    }

    private void PreorderTraverse(TreeNode root)
    {
        if (root == null)
        {
            _builder.Append(Null);
            _builder.Append(",");
            return;
        }
        else
        {
            _builder.Append(root.val.ToString());
            _builder.Append(",");
            PreorderTraverse(root.left);
            PreorderTraverse(root.right);
        }
    }

    private void PreorderDeserialize(ref TreeNode root)
    {
        // TODO: 如果是负数或大于10的数需要1个以上char
        if (_index > _datas.Length - 1) { return; }
        // 父节点是叶节点
        if (_datas[_index] == Null) { _index++; return; }
        else
        {
            root = new TreeNode(int.Parse(_datas[_index++]));
            PreorderDeserialize(ref root.left);
            PreorderDeserialize(ref root.right);
        }
    }

    // Decodes your encoded data to tree.
    public TreeNode deserialize(string data)
    {
        _datas = data.Split(new char[] { ',' });
        TreeNode root = null;
        PreorderDeserialize(ref root);
        return root;
    }
}