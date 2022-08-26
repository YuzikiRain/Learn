## 数学公式
$$
\begin{align}
h(x) =& \frac{1}{\int_xt(x)\mathrm{d}x} \tag{1}\\
f(x) =& \frac{1}{\int_x\eta(x)\mathrm{d}x}g(x)\tag{2}
\end{align}
$$

必须要以```\begin{align}```开头，```\end{align}```结尾表示对齐，使用```&```标记对齐位置
```\\```用于公式换行

## 矩阵

-   以`\begin{matrix}`开头，以`\end{matrix}`结尾
-   每一行用`\\`结尾，用`&`分隔行内元素

$$
\begin{matrix}
 x/w&a\\
 y/w&b\\
 z/w&c\\
 \end{matrix}
$$
### 矩阵边框

matrix表示默认矩阵边框，可替换为以下

-   `pmatrix`：小括号边框
-   `bmatrix`：中括号边框
-   `Bmatrix`：大括号边框
-   `vmatrix`：单竖线边框
-   `Vmatrix`：双竖线边框

### 矩阵运算

$$
\left[
    \begin{matrix}
    1 & 0 & 0 & t_x\\
    0 & 1 & 0 & t_y\\
    0 & 0 & 1 & t_z\\
    0 & 0 & 0 & 1
    \end{matrix}
\right]
\left[
    \begin{matrix}
    x\\
    y\\
    z\\
    1
    \end{matrix}
\right]=
\left[
    \begin{matrix}
    x+t_x\\
    y+t_y\\
    z+t_z\\
    1
    \end{matrix}
\right]
$$



## 符号

- \cdot $\cdot$
- \frac $\frac{分子}{分母}$
- \displaystype\frac $\displaystyle\frac{分子}{分母}$  Typora编辑器可用

