## 常用

### 项目名称

``` cmake
project(hello_cmake)
```

会创建一个值为 `hello_cmake` 的变量`${PROJECT_NAME}`

### 添加子目录

``` cmake
add_subdirectory(source_dir [binary_dir] [EXCLUDE_FROM_ALL])
```

范例

``` cmake
add_subdirectory(${PROJECT_SOURCE_DIR}/src/moduleA)
```

如果`source_dir`不是当前目录的子目录，还需要额外设置`binary_dir`，否则会报错：

```bash
add_subdirectory not given a binary directory but the given source
  directory "source_dir" is not a subdirectory of
  "current_dir".  When specifying an out-of-tree source a
  binary directory must be explicitly specified.
```

`binary_dir`一般设置为`CMAKE_BINARY_DIR`下的某个子目录（一般`CMAKE_BINARY_DIR`用于`add_executable`输出可执行文件，而`CMAKE_BINARY_DIR`不能和其相同）

``` cmake
add_subdirectory(${PROJECT_SOURCE_DIR}/src/moduleA ${CMAKE_BINARY_DIR}/moduleA)
```

### 添加库

``` cmake
add_library(target STATIC src/main.cpp)
```

我们将源文件直接传递给add_library调用，这是现代 CMake 的建议。

### 包含目录

```
 target_include_directories(hello_library PUBLIC ${PROJECt_SOURCE_DIR}/include)
```

这将导致在以下位置使用的包含目录：

-   编译库时
-   编译链接库的任何其他目标时。

作用域的含义是：

-   PRIVATE - 将目录添加到此目标的包含目录中
-   INTERFACE 该目录将添加到链接此库的任何目标的包含目录中。
-   PUBLIC - 如上所述，它包含在此库中，以及链接此库的任何目标中。

### 创建可执行文件

``` cmake
add_executable(target main.cpp)
```

范例

``` cmake
add_executable(hello_cmake main.cpp)
```

### 链接库

创建使用了库的可执行文件时，必须将这个库告知编译器

``` cmake
targe_link_libraries(executable PRIVATE library)
```

范例

``` cmake
add_executable(hello_cmake src/main.cpp)
add_library(hello_library STATIC src/MyLibrary.cpp)
targe_link_libraries(hello_cmake PRIVATE hello_library)
```

## 目录路径

CMake 语法指定了许多变量，这些[变量](https://gitlab.kitware.com/cmake/community/-/wikis/doc/cmake/Useful-Variables)可用于帮助在项目或源代码树中查找有用的目录。其中一些包括：

| 变量                         | 信息                                                         |
| ---------------------------- | ------------------------------------------------------------ |
| CMAKE_BINARY_DIR             | 如果就地构建，则与CMAKE_SOURCE_DIR相同，否则这是顶级CMakeLists.txt对应的build目录 |
| **CMAKE_SOURCE_DIR**         | **顶级CMakeLists.txt目录**                                   |
| CMAKE_CURRENT_BINARY_DIR     | 如果就地构建，则与CMAKE_CURRENT_SOURCE_DIR相同，否则这是从当前CMakeLists编译或生成的文件的build目录 |
| **CMAKE_CURRENT_SOURCE_DIR** | **当前CMakeLists.txt所在的目录**                             |
| PROJECT_BINARY_DIR           | 当前project的CMakeLists.txt对应build目录                     |
| **PROJECT_SOURCE_DIR**       | **当前project的CMakeLists.txt目录**                          |
| TOP_DIR                      |                                                              |
| CMAKE_COMMAND                | 这是当前运行的 cmake 的完整路径（例如 ）。请注意，如果您有调用 的自定义命令，则使用 CMAKE_COMMAND 作为 CMake 可执行文件*非常重要*，因为 CMake 可能不在系统 PATH 上。`/usr/local/bin/cmake``cmake -E` |
| CMAKE_CURRENT_LIST_FILE      | 这是当前正在处理的列表文件的完整路径                         |
| **CMAKE_CURRENT_LIST_DIR**   | （自 **2.8.3** 起）这是当前正在处理的列表文件的目录          |
| CMAKE_CURRENT_LIST_LINE      | 这是使用变量的行号                                           |
| CMAKE_FILES_DIRECTORY        | 当前二进制目录中包含所有 CMake 生成文件的目录。通常计算结果为“/CMakeFiles”。请注意目录的前导斜杠。通常与当前二进制目录一起使用，即 `${CMAKE_CURRENT_BINARY_DIR}``${CMAKE_FILES_DIRECTORY}` |
| CMAKE_MODULE_PATH            | 当您使用FIND_PACKAGE（）或INCLUD（）时，告诉CMake首先在CMAKE_MODULE_PATH中列出的目录中搜索`SET(CMAKE_MODULE_PATH ${PROJECT_SOURCE_DIR}/MyCMakeScripts)` `FIND_PACKAGE(HelloWorld)` |
| CMAKE_ROOT                   | 这是CMake安装目录                                            |
| PROJECT_NAME                 | 由 PROJECT（） 命令设置的项目的名称                          |
| CMAKE_PROJECT_NAME           | 由 PROJECT（） 命令设置的第一个项目的名称，即顶级项目        |

## 创建变量

``` cmake
set(SOURCES src/main.cpp)
# 使用变量SOURCES
add_executable(${PROJECT_NAME} ${SOURCES})
```

## 最低CMake版本

``` cmake
cmake_minimum_required(VERSION 3.5)
```

## 二进制目录

运行 cmake 命令的根文件夹或顶级文件夹称为`CMAKE_BINARY_DIR`

### 源外构建 Out-of-Source Build

源代码外构建允许您创建单个构建文件夹，该文件夹可以位于文件系统上的任何位置。所有临时构建文件和对象文件都位于此目录中，保持源树的清洁。若要创建源外生成，请在生成文件夹中运行 cmake 命令，并将其指向包含根 CMakeLists.txt 文件的目录。

如果要从头开始重新创建 cmake 环境，请使用源代码外构建，只需删除构建目录，然后重新运行 cmake。

### 就地构建 In-Place Build

就地生成在与源代码相同的目录结构中生成所有临时生成文件。这意味着所有生成文件和对象文件都散布在正常代码中。要创建就地构建目标，请在根目录中运行 cmake 命令。

## 参考

-   [CMake 很难，因为你没看过这篇文章 (qq.com)](https://mp.weixin.qq.com/s?__biz=Mzg4ODAxMzE2OA==&mid=2247484983&idx=1&sn=af9760f5960492190efeb040596702ad&chksm=cf80ee28f8f7673e49c7f4cf8a71e248b283545492a1c231b4535db14d6b1738f3746c58982b&token=391208598&lang=zh_CN#rd)
-   [Cmake之深入理解find_package()的用法 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/97369704?utm_source=wechat_session&utm_medium=social&utm_oi=730003385939415040&utm_campaign=shareopn)
-   [CMake从入门到精通系列链接整理 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/393316878)
-   [ttroy50 (Thom Troy) (github.com)](https://github.com/ttroy50)
