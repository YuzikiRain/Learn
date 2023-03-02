## 安装

cmake选择源码目录，build目录，因为我们不需要单元测试、范例、文档，所以取消勾选 `CUSTOM_DOXYGEN_STYLE` `BUILD_DOCUMENTATION` `EXAMPLES` `BUILD_UNIT_TESTS`，因为其中一些需要额外的库支持。

![image-20230220112347523](https://fastly.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20230220112347523.png)

Configure，然后Generate（选择对应的编译器），然后到对应目录下执行make即可。

到build->bin下取得`librttr_core.dll`，然后放到用到的工程的build（即可执行文件所在目录）目录下

**到`build\src\rttr\detail\base`目录下将`version.h`和`version.rc`文件拷贝到对应的include目录（Include\rttr\detail\base）下。**

经过测试，使用GCC8.1并指定C++17会编译报错（因为不支持C++17），GCC12.2并指定C++17可以编译通过，但在实际运行时进行类型反射时会抛出异常。**使用10.3.0并指定C++17可以编译通过，也可以正常运行，但不能注册glm::mat4等matrix类型**

## API

### Registration

注册类型信息

``` c++
using namespace rttr;
struct Foo{};
RTTR_REGISTRATION
{
     registration::class_<Foo>("Foo")
                  .constructor<>();
}
```

调整默认注册行为：在构造函数之后添加policy

``` c++
 registration::class_<Foo>("Foo")
              .constructor<>()(policy::ctor::as_std_shared_ptr);
```

- as_std_shared_ptr：默认。通过`std::make_shared<T>`返回类的实例

- as_raw_ptr：返回指向类实例的普通指针。需要手动调用destroy方法（等同于调用`delete ptr`）
    ``` c++
    RTTR_REGISTRATION
    {
         registration::class_<Foo>("Foo")
                      .constructor<>()(policy::ctor::as_raw_ptr);
    }
    int main()
    {
        variant var = type::get<Foo>().create();
        std::cout << var.is_type<Foo*>();          // prints "true"
        var.get_type().destroy(var);               // free's the memory with 'delete'
        std::cout << var.is_valid();               // prints "false"
        return 0;
    }
    
    ```
    
- as_object：分配在栈上的局部对象。will create an instance of a class with automatic storage。离开作用域时自动销毁。对象必须是可拷贝的（*constructible*）

### Type

- 取得Type信息
    - `type::get(instance)`：传入类的实例，比如`BorderlessEngine::Transform transform`，则传入transform即可
    - `type::get_by_name(string_view name)`：name为注册类信息里填的类名称，比如`registration::class_<BorderlessEngine::Transform>("Transform")`里的`"Transform"`，而不是`BorderlessEngine::Transform`
    - `type::get<Type>()`：Type为实际类型，比如`BorderlessEngine::Transform`
- 取得Type的注册名称：`auto t = type::get<Type>(); t.get_name().to_string()`

### Property

- 遍历所有字段：`for (auto &property : type.get_properties())`
- 取得字段的注册名称：`property.get_name()`
- 判断字段的类型：`propertyValue.is_type<Type>()`或`property.get_type() == type::get<Type>()`
- 读取字段的值：`auto propertyValue = property.get_value(instanceOrPointerOfInstance);`
- 转换为实际字段类型：`auto &fieldVariable = propertyValue.get_value<Type>();`
- 写入字段的值：`property.set_value(instanceOrPointerOfInstance, newValue);`

## 范例

- 注册类信息

    ``` c++
    // Component.h
    #pragma once
    
    #include <rttr/registration>
    using namespace rttr;
    
    class Component
    {
    	RTTR_ENABLE()
    };
    ```

    ``` c++
    // Transform.h
    #pragma once
    
    #include <rttr/registration>
    using namespace rttr;
    
    class Transform : public Component
    {
    public:
    	glm::vec3 Position;
        RTTR_ENABLE(Component)
    };
    ```

    ``` c++
    // 放在任意文件中，但只能存在一处，否则会报错：rttr/detail/registration/registration_impl.h:292:12: error: redefinition of 'struct {anonymous}::rttr__auto__register__'
    RTTR_REGISTRATION
    {
    	registration::class_<Component>("Component")
    		.constructor<>();
    		
    	registration::class_<BorderlessEngine::Transform>("Transform")
    		.constructor<>()(policy::ctor::as_raw_ptr)
    		.property("Position", &BorderlessEngine::Transform::Position);
        
        registration::class_<glm::vec3>("glm::vec3")
    		.constructor<>()
    		.property("x", &glm::vec3::x)
    		.property("y", &glm::vec3::y)
    		.property("z", &glm::vec3::z)
    		.method("length", &glm::vec3::length);
    }
    ```

- 其他用到的类

    ``` c++
    class GameObject
    {
    public:
        std::unordered_map<std::string, Component *> components;
    }
    ```

- 使用

    ``` c++
    GameObject obj;
    for (auto &kv : obj->components)
    {
        auto name = kv.first;
        ImGui::Text(name.c_str());
        auto componentPointer = kv.second;
        auto type = type::get(*componentPointer);
        for (auto &property : type.get_properties())
        {
            auto propertyValue = property.get_value(componentPointer);
            // 判断类型，也可以用prop.get_type() == type::get<glm::vec3>()
            if (propertyValue.is_type<glm::vec3>())
            {
                // 转换成实际类型，然后可以随意读取字段的值了
                auto &position = propertyValue.get_value<glm::vec3>();
                float f3[3] = {position.x, position.y, position.z};
                ImGui::InputFloat3(property.get_name().to_string().c_str(), f3);
                position.x = f3[0];
                position.y = f3[1];
                position.z = f3[2];
                // 虽然position是个引用，修改值仍需要使用set_value才能生效（可能position只是字段的拷贝）
                property.set_value(componentPointer, position);
            }
        }
    }
    ```

## 链接

- [Building & Installation - 0.9.7 | RTTR](https://www.rttr.org/doc/master/building_install_page.html)
- [5 minute Tutorial - 0.9.7 | RTTR](https://www.rttr.org/doc/master/five_minute_tutorial_page.html)
- [Properties - 0.9.7 | RTTR](https://www.rttr.org/doc/master/register_properties_page.html)
- [Policies - 0.9.7 | RTTR](https://www.rttr.org/doc/master/register_policies_page.html)
