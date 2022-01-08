### 搭建图床

- 新建github仓库，可见性public

- setting->develop settings->personal access tokens：生成（或重新生成）**token**

- 下载PicGo，按下图设置
    ![image-20220108102759032](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220108102759032.png)

    - 仓库名：用户名/仓库名
    - 分支名：master或main
    - token：之前步骤生成的**token**
    - 存储路径：最好不要在根目录下否则列表很长，所以可以填 `img/`
    - 自定义域名：加速访问用（否则一般就上传不了了） `https://cdn.jsdelivr.net/gh/用户名/仓库名`

- **设置为默认图床**

### Typora

- 按下图设置
    ![image-20220108103503367](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220108103503367.png)
    - 插入图片时：上传图片（这样就不用再点击上传图片的按钮了）
    - 上传服务：PicGo（app）
    - PicGo路径：PicGo.exe路径
- 验证图片上传选项：测试是否能正常上传，不行的话检查下PicGo的**自定义域名**设置、Server设置（打开或关闭）
    ![image-20220108103730124](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220108103730124.png)