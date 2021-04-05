### 搭建服务器

-   买一台阿里云服务器，**不需要域名**，记下外网IP
-   安装JDK：https://www.oracle.com/java/technologies/javase-downloads.html，安装版（非zip）会自动设置环境变量
-   安装apache Tomcat：https://tomcat.apache.org/download-80.cgi，**默认端口8080**（可以到```conf/server.xml```下改），```bin/startup.bat```启动服务
-   配置入口规则：开放8080端口（其他端口也行，要跟上一步一致），阿里云服务器设置：控制台 -> 实例 -> 更多 -> 网络和安全组 -> 配置安全组规则 -> 手动添加（如方向） -> 端口范围
-   提交热更新数据：将所需文件传输到云服务器的 ```tomcat安装目录/webapps/ServerData/StandaloneWindows``` 目录下，可使用以下方法
    -   远程连接，映射本地磁盘驱动器：https://cloud.tencent.com/document/product/213/2761 
    -   部署FTP服务器并上传：https://help.aliyun.com/document_detail/92046.html
        部署FTP服务器：
        -   
        -   搭建好FTP站点后，您需要在实例安全组的入方向添加规则，放行FTP服务器21端口及**FTP服务器被动模式1024/65535范围内的所有端口**。

### 热更新

-   ```AddressableAssetsData/AddressableAssetSettings```，勾选```Build Remote Catalog```（勾选后，会生成.hash 和 .json文件，得出哪些文件需要更新），将```Build Path``` 和 ```Load Path```分别设置为```RemoteBuildPath```和```RemoteLoadPath```
-   对应Unity Addressable Profile里，RemoteBuildPath=```ServerData/[BuildTarget]```（Build包时会生成bundle在```工程目录/ServerData```下，而如果是单机游戏，选择LocalBuildPath，则会生成在```工程目录\Library\com.unity.addressables\aa\[BuildTarget]```），RemoteLoadPath=```外网IP:默认端口/ServerData/[BuildTarget]```，因为这里只是简单地搭建了一个网站，**"外网IP:默认端口"对应的就是"tomcat安装目录/webapps"**
-   打初始包：分为本地包（Build时直接包含在Player中，不需要下载就可以使用）和远程更新包（需要下载更新），```Addressables Groups -> Build -> New Build -> Default Build Script```
-   将远程包放到RemoteLoadPath上（可以通过FTP等各种方式上传）
-   资源发生变动需要更新时（不论是在原group上增删修改entry，还是增删group，都算作资源变动），在Group界面 -> Build -> Update Previous Build，选择 ```AddressableAssetsData/[BuildTarget]/addressables_content_state.bin```文件
-   此时会生成新的.json文件（文件名带有时间戳，也可以设置用版本号```Assets/AddressableAssetsData/AddressableAssetSettings -> Player Version Override```，用于判断资源是否过期，文件内容包含包文件名、远程加载路径、所有资源名称等关键信息）.hash文件，将生成的新AB包和```catalog_xxx.json```和 ```catalog_xxx.hash```文件上传到 RemoteLoadPath 上

### 更新Catalog

#### 手动更新

```AddressableAssetsData/AddressableAssetSettings``` 不勾选 ```disable catalog update on startup```，关闭自动更新

```csharp
    IEnumerator UpdateCatalogs()
    {
        List<string> catalogsToUpdate = new List<string>();
        // 检查是否需要更新catalog
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
        checkForUpdateHandle.Completed += op =>
        {
            catalogsToUpdate.AddRange(op.Result);
        };
        yield return checkForUpdateHandle;
        // 需要更新catalog，开始更新
        if (catalogsToUpdate.Count > 0)
        {
            Debug.Log($"need update");
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
            while (!updateHandle.IsDone)
            {
                float percent = updateHandle.PercentComplete;
                Debug.Log($"updating, progress = {percent}");
            }
            yield return updateHandle;
        }
        else
            Debug.Log($"latest version");
    }
```

#### 自动更新

```AddressableAssetsData/AddressableAssetSettings``` 不勾选 ```disable catalog update on startup```，在使用```Addressables.LoadAssetAsync```等函数进行加载资源前，会自动调用```Addressables.InitializeAsync```进行初始化。
在调用```Addressables.InitializeAsync``` 时，自动调用 ```Addressables.CheckForCatalogUpdates``` 和 ```Addressables.UpdateCatalogs``` 进行资源更新，阻塞所有资源加载和实例化，直到完成。