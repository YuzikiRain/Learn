## 配置环境

[一步一步跟我学ImGui.第一讲.配置OpenGl2+ImGui环境 - iBinary - 博客园 (cnblogs.com)](https://www.cnblogs.com/iBinary/p/10888911.html)

## CMake

Visual Studio默认会添加opengl32.lib，而cmake需要手动配置

![image-20220725113539446](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220725113539446.png)

``` cmake
set(PROJECT_LIBRARIES)
# 不需要下面这行
# find_package(OpenGL REQUIRED)
# opengl 这个是必须的！imgui依赖opengl
list (APPEND PROJECT_LIBRARIES ${OPENGL_LIBRARIES})
#-------------
target_link_libraries(${PROJECT_NAME} PUBLIC ${PROJECT_LIBRARIES})
```

## 使用



## Drag & Drop

[Drag & Drop files using Win32 and Dear ImGui · Issue #2602 · ocornut/imgui (github.com)](https://github.com/ocornut/imgui/issues/2602)

## Docking拖拽到window之外

使用ImGui的docking分支，然后按照这个教程做 [ImGui Docking and Viewports | Fl0w3r (yousazoe.top)](https://www.yousazoe.top/archives/621d636b.html)

