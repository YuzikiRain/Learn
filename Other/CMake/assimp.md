使用cmake+mingw编译5.2.4版本的源码会报错，还未测试Visual Studio

``` cmake
assimp-5.2.4\contrib\unzip\crypt.c:168: error: ignoring '#pragma warning ' [-Werror=unknown-pragmas]
  168 | #   pragma warning(pop)
```

想到LearnOpenGL教程里写的当时使用3.1.1版本，以及forge网站的最新二进制构建下载也只有3的版本

## 解决方案

不使用5.2.4版本，改为使用3.1.1，然后在CMake里不启用ASSIMP_BUILD_ASSIMP_TOOLS即可