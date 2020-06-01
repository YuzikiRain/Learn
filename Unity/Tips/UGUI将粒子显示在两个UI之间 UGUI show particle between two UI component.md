- set UI canvas render mode "Screen Space - Camera"
- add canvas component to particle gameobject, check override sorting layer
- add canvas component to ui gameobject image1 and image2(such as a image), check override sorting layer
- set order in layer: particle is 2, image1 is 1, image2 is 3

  <img alt="Sprite.png" src="assets/UGUI show particle between two UI component.png" width="400" height="" >

you will see the particle is between image1 and image2.

### 参考
https://gamedev.stackexchange.com/questions/175145/how-to-process-particle-effect-between-ui