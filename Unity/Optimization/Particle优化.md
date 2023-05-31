- 不使用mesh作为粒子（mesh粒子不会合批）。
- overdraw
- order in layer 影响合批

- 至多使用alpha blend（垫底色）和additive（曝光）两个材质球，两shader均需要支持顶点色，不需要带tint color。
- 使用粒子系统的Start Color和Color over Lifetime来改变粒子颜色（它们都是顶点色），而不是使用shader上的tint color。



[Unity 粒子特效在游戏中的运用以及优化 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5d9aa917edbc2a001fa00c38)
