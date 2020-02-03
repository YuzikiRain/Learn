## Create custom package
- 确保要做成package的目录代码没问题之后，在编辑器的Project视图中在该目录下右键Create->Assembly Definition
- 在GitHub创建远程仓库，在Unity工程的Packages目录下，创建子文件夹并命名为你的package名称（保证文件夹是空的），使用仓库地址和本地相对路径创建子模块

  <img alt="Sprite.png" src="assets/add submodule.png" width="500" height="" >
- 提交.gitmodules和submodule的commit（完成子模块的创建）
- 将第一步中已经检查没问题的package文件夹与刚才创建的文件夹合并，在子模块目录下创建package.json并按如下格式修改 **（name一定要全小写）**
``` json
{
	"name": "com.modesttree.yourpackagename",
	"displayName": "YourPackageName",
	"description": "",
	"version": "0.1.2",
	"unity": "2017.1.1f",
	"author": "modesttree",
	"github": "https://github.com/modesttree/YourPackageName",
	"unityAssetStore": "https://assetstore.unity.com/packages/tools/integration/async-await-support-101056",
	"dependencies": {}
}
```
- 再次测试package
- 打开子模块，提交合并的内容，并推送
- 返回仓库，提交子模块的修改，并推送
