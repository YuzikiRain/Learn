## makefile

环境为windows10

-   安装gcc编译器，配置环境变量PATH=安装目录/bin，即含有gcc.exe的目录（否则你在使用make命令时得输入mingw32-make.exe的全路径）；复制一份该目录下的mingw32-make.exe的副本并重命名为make（否则你需要执行mingw32-make，而不是make）

-   创建makefile文件（不需要后缀名，否则你得在执行make的时候输入全名，比如`make makefile.txt`）
-   （在makefile所在目录下，否则你还得输入makefile的全路径）执行make命令

### 变量

-   可以定义一些变量，然后用$(变量)来使用
-   变量可以放在后边定义，不一定要放在最前边

```makefile
main.o: main.c
	$(CC) $(CFLAGS) $(CPPFLAGS) -c $<

CC = gcc
CFLAGS = -O -g
CPPFLAGS = -Iinclude
```

参考：[3. 变量 (akaedu.github.io)](http://akaedu.github.io/book/ch22s03.html)

### opengl glfw+glad库的makefile

``` makefile
CC = gcc
CXX = g++

COMPILE_FLAGS = -Wall -ggdb -O3
LINK_FLAGS = -lglfw3 -lopengl32 -lglu32 -lgdi32

projectpath = D:\UnityProject\LearnOpenGL\Opengl
glfw = $(projectpath)/glfw-3.3.6.bin.WIN64
glfw_inc = $(glfw)/include
glfw_lib = $(glfw)/lib-mingw-w64

glad = $(projectpath)/glad
glad_inc = $(glad)/include
glad_src = $(glad)/src/glad.c

INCLUDES = -I$(glfw_inc) -I$(glad_inc) -I$(projectpath)/MyWindow
LIBRARIES = -L$(glfw_lib)

cpp_files = main.cpp
objects = $(cpp_files:.cpp=.o)
headers = $(projectpath)/MyWindow/MyWindow.h

all: main.exe

main.exe: $(objects) glad.o
	$(CXX) $(LIBRARIES) -o main.exe $(objects) glad.o $(LINK_FLAGS)

$(objects): %.o: %.cpp $(headers) makefile
	$(CXX) $(COMPILE_FLAGS) $(INCLUDES) -c -o $@ $<
	
#glad.c is in glad/src, so we need full path
glad.o: $(glad_src)
	$(CC) $(COMPILE_FLAGS) $(INCLUDES) -c -o glad.o $(glad_src)
		
.PHONY : clean
clean:
	del *.o main.exe -rf
```

参考：[Makefile for C++ OpenGL with GLFW and glad - Code Review Stack Exchange](https://codereview.stackexchange.com/questions/78855/makefile-for-c-opengl-with-glfw-and-glad)

