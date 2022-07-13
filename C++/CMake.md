## 最低CMake版本

``` cmake
cmake_minimum_required(VERSION 3.5)
```

## 项目名称

``` cmake
project(hello_cmake)
```

创建一个值为 `hello_cmake` 的变量`${PROJECT_NAME}`

## 创建可执行文件

``` cmake
add_executable(hello_cmake main.cpp)
```

## 创建变量

``` cmake
set(SOURCES src/main.cpp)
# 使用变量SOURCES
add_executable(${PROJECT_NAME} ${SOURCES})
```

## 二进制目录

运行 cmake 命令的根文件夹或顶级文件夹称为`CMAKE_BINARY_DIR`

### 源外构建 Out-of-Source Build

源代码外构建允许您创建单个构建文件夹，该文件夹可以位于文件系统上的任何位置。所有临时构建文件和对象文件都位于此目录中，保持源树的清洁。若要创建源外生成，请在生成文件夹中运行 cmake 命令，并将其指向包含根 CMakeLists.txt 文件的目录。

如果要从头开始重新创建 cmake 环境，请使用源代码外构建，只需删除构建目录，然后重新运行 cmake。

### 就地构建 In-Place Build

就地生成在与源代码相同的目录结构中生成所有临时生成文件。这意味着所有生成文件和对象文件都散布在正常代码中。要创建就地构建目标，请在根目录中运行 cmake 命令。

## 参考

-   [CMake从入门到精通系列链接整理 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/393316878)
-   [ttroy50 (Thom Troy) (github.com)](https://github.com/ttroy50)