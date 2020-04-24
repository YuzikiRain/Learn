using System;

class Offer33
{
    public void Test()
    {
        int[] postorder = new int[] { 3, 10, 6, 9, 2 };
        Console.WriteLine(VerifyPostorder(postorder));
    }

    public bool VerifyPostorder(int[] postorder)
    {
        // 二叉搜索树特点：左子树 < 根节点 < 右子树，用此特点来确定根节点
        // [1,3,2,6,5] 5为root，6大于5，必然在其右子树，132则在其左子树
        // [1,6,3,2,5] 2小于根节点5，因此不是二叉搜索树
        return VerifyPostorder(postorder, 0, postorder.Length - 1);
    }

    private bool VerifyPostorder(int[] postorder, int left, int right)
    {
        if (left >= right) { return true; }
        int index = right - 1;
        while (index >= left && postorder[index] > postorder[right]) { index--; }
        bool succeed = true;
        int tempIndex = index;
        // 存在右子树（大于root的部分）
        if (left <= index + 1 && index + 1 < right) { succeed &= VerifyPostorder(postorder, index + 1, right - 1); }
        while (index >= left && postorder[index] < postorder[right]) { index--; }
        // 左子树未到达left，说明不是二叉搜索树
        if (index >= left) { return false; }
        // 存在左子树（小于root的部分）
        else { succeed &= VerifyPostorder(postorder, left, tempIndex); }
        // System.Console.WriteLine($"[{index}]={postorder[index]}");
        return succeed;
    }
}
