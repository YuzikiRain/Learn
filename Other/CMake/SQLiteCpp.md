## 安装

clone github上的工程，然后在你的工程的根目录下的CMakeLists.txt里添加以下代码

``` cmake
add_subdirectory(${CMAKE_CURRENT_LIST_DIR}/thirdparty/SQLiteCpp)
# 如果SQLiteCpp不是当前目录的子目录，还需要额外设置
# add_subdirectory(${PACKAGE_PATH}/SQLiteCpp ${CMAKE_BINARY_DIR}/SQLiteCpp)

add_executable(main src/main.cpp)
target_link_libraries(main
  SQLiteCpp
  sqlite3
  pthread
  # 这个不要添加，会报无法链接的错
  #dl
  )
```

在windows下编译时会报`One or more carriage-return \r (^M) (Windows endline) found; Use only UNIX endline`，需要用文本编辑软件批量修改`\r`和`\r\n`为`\n`

## 使用

``` c++
#include <iostream>
#include <cstdio>
#include <cstdlib>
#include <string>
#include <SQLiteCpp/SQLiteCpp.h>
#include <SQLiteCpp/VariadicBind.h>

int main()
{
    try
    {
        SQLite::Database db("test.db", SQLite::OPEN_READWRITE | SQLite::OPEN_CREATE);

        // Compile a SQL query, containing one parameter (index 1)
        SQLite::Statement query(db, "SELECT * FROM tableName");

        // Loop to execute the query step by step, to get rows of result
        while (query.executeStep())
        {
            // Demonstrate how to get some typed column value
            int id = query.getColumn(0);
            const char *value = query.getColumn(1);

            std::cout << "row: " << id << ", " << value << std::endl;
        }
    }
    catch (std::exception &e)
    {
        std::cout << "exception: " << e.what() << std::endl;
    }

    return 0;
}

```

