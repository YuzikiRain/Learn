## 系统目录

``` bash
::当前目录
echo %~dp0
```

## 变量

``` bash
set myVariable=abc
echo %myVariable%
```

## For循环

```  bash
set outsideVariable=outside

@setlocal enabledelayedexpansion

::%%i表示迭代器的当前对象，也可以用其他字母比如%%a
for /R 循环的指定目录 %%i in (通配符表示的filter，比如*_rgb.avi表示后缀) do (
	::表示
	echo %%~ni
	set insideVariable=inside
	::循环内使用变量需要用!!包裹，而不是%%
	echo !insideVariable!
	echo !outsideVariable!
	::这种特殊的不用
	echo %%i
)
```

## Start

``` bash
start 应用程序路径 参数
start D:\test.exe -video00="D:\test.avi"
```

[批处理中for循环怎么去掉变量的后缀名 (longqi.cf)](https://www.longqi.cf/system/2014/11/18/VariableinForLoop/)