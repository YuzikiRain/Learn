## Fails to compile. Undefined reference to YAML::LoadFile

链接错误，链接了错误的库

官网给出的build方式没有包含Mingw编译器的，如果使用yaml-cpp的项目使用Mingw编译，则yaml-cpp也需要用Mingw编译（而不是网上随便下一个）

`cmake.exe -G` ，`-G`可以指定编译器（Generator）

调用`cmake --help`会显示可用的Generator

``` 
Generators

The following generators are available on this platform (* marks default):
  Visual Studio 17 2022        = Generates Visual Studio 2022 project files.
                                 Use -A option to specify architecture.
* Visual Studio 16 2019        = Generates Visual Studio 2019 project files.
                                 Use -A option to specify architecture.
  Visual Studio 15 2017 [arch] = Generates Visual Studio 2017 project files.
                                 Optional [arch] can be "Win64" or "ARM".
  Visual Studio 14 2015 [arch] = Generates Visual Studio 2015 project files.
                                 Optional [arch] can be "Win64" or "ARM".
  Visual Studio 12 2013 [arch] = Generates Visual Studio 2013 project files.
                                 Optional [arch] can be "Win64" or "ARM".
  Visual Studio 11 2012 [arch] = Generates Visual Studio 2012 project files.
                                 Optional [arch] can be "Win64" or "ARM".
  Visual Studio 10 2010 [arch] = Deprecated.  Generates Visual Studio 2010
                                 project files.  Optional [arch] can be
                                 "Win64" or "IA64".
  Visual Studio 9 2008 [arch]  = Generates Visual Studio 2008 project files.
                                 Optional [arch] can be "Win64" or "IA64".
  Borland Makefiles            = Generates Borland makefiles.
  NMake Makefiles              = Generates NMake makefiles.
  NMake Makefiles JOM          = Generates JOM makefiles.
  MSYS Makefiles               = Generates MSYS makefiles.
  MinGW Makefiles              = Generates a make file for use with
                                 mingw32-make.
  Green Hills MULTI            = Generates Green Hills MULTI files
                                 (experimental, work-in-progress).
  Unix Makefiles               = Generates standard UNIX makefiles.
  Ninja                        = Generates build.ninja files.
  Ninja Multi-Config           = Generates build-<Config>.ninja files.
  Watcom WMake                 = Generates Watcom WMake makefiles.
  CodeBlocks - MinGW Makefiles = Generates CodeBlocks project files.
  CodeBlocks - NMake Makefiles = Generates CodeBlocks project files.
  CodeBlocks - NMake Makefiles JOM
                               = Generates CodeBlocks project files.
  CodeBlocks - Ninja           = Generates CodeBlocks project files.
  CodeBlocks - Unix Makefiles  = Generates CodeBlocks project files.
  CodeLite - MinGW Makefiles   = Generates CodeLite project files.
  CodeLite - NMake Makefiles   = Generates CodeLite project files.
  CodeLite - Ninja             = Generates CodeLite project files.
  CodeLite - Unix Makefiles    = Generates CodeLite project files.
  Eclipse CDT4 - NMake Makefiles
                               = Generates Eclipse CDT 4.0 project files.
  Eclipse CDT4 - MinGW Makefiles
                               = Generates Eclipse CDT 4.0 project files.
  Eclipse CDT4 - Ninja         = Generates Eclipse CDT 4.0 project files.
  Eclipse CDT4 - Unix Makefiles= Generates Eclipse CDT 4.0 project files.
  Kate - MinGW Makefiles       = Generates Kate project files.
  Kate - NMake Makefiles       = Generates Kate project files.
  Kate - Ninja                 = Generates Kate project files.
  Kate - Unix Makefiles        = Generates Kate project files.
  Sublime Text 2 - MinGW Makefiles
                               = Generates Sublime Text 2 project files.
  Sublime Text 2 - NMake Makefiles
                               = Generates Sublime Text 2 project files.
  Sublime Text 2 - Ninja       = Generates Sublime Text 2 project files.
  Sublime Text 2 - Unix Makefiles
                               = Generates Sublime Text 2 project files.
```

windows下直接调用cmake会默认用当前安装的visual studio版本对应的编译器，因此需要指定编译器

## cmake步骤

- github clone源码，（最好新建一个build目录，然后cd到该目录下）执行以下cmake命令（指定编译器为Mingw）
    ```
    cmake.exe -G "MinGW Makefiles" .
    ```

    官方的build说明，不指定`-DYAML_BUILD_SHARED_LIBS=on`的话默认是静态库

    ```
    mkdir build
    cd build
    cmake [-G generator] [-DYAML_BUILD_SHARED_LIBS=on|OFF] ..
    ```

- 运行make以执行makefile，window下需要额外安装[GNU]([GnuWin download | SourceForge.net](https://sourceforge.net/projects/gnuwin32/))，配环境变量的路径后还是不能执行make的话，就直接拖动安装目录下的`make.exe`到cmd里执行

- build目录下出现`libyaml-cpp.dll`文件，直接拖过去用，`target_link_libraries`里包含该文件路径，`target_include_directories`包含yaml-cpp源码的include目录，将该文件添加到build目录下

## 参考

- [Fails to compile. Undefined reference to YAML::LoadFile · Issue #591 · jbeder/yaml-cpp (github.com)](https://github.com/jbeder/yaml-cpp/issues/591)
- [jbeder/yaml-cpp: A YAML parser and emitter in C++ (github.com)](https://github.com/jbeder/yaml-cpp)