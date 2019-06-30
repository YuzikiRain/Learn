## Create custom package

- 确保要做成package的目录代码没问题之后，在Editor的Project视图中在该目录下右键Create->Assembly Definition
- 在GitHub创建远程仓库，创建子文件夹package并命名（保证文件夹是空的），使用仓库地址和本地相对路径创建子模块
  <img alt="Sprite.png" src="assets/add submodule.png" width="500" height="" >
- 提交.gitmodules和submodule的commit
- 将实际的package与刚才创建的文件夹合并，在子模块目录下创建package.json并按如下格式修改
``` json
{
	"name": "com.modesttree.AsyncAwaitUtil",
	"displayName": "AsyncAwaitUtil",
	"description": "unity package AsyncAwaitUtil from github",
	"version": "0.1.2",
	"unity": "2017.1.1f",
	"author": "modesttree",
	"github": "https://github.com/modesttree/Unity3dAsyncAwaitUtil",
	"unityAssetStore": "https://assetstore.unity.com/packages/tools/integration/async-await-support-101056",
	"dependencies": {}
}
```
- 打开子模块，提交合并的内容
- 返回仓库，提交子模块的修改
