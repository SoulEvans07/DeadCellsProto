using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteLoader {
    private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private static List<string> loadedSpriteSheets = new List<string>();
    
    public static void loadSpritesFrom(string spriteSheetPath) {
        if (!loadedSpriteSheets.Contains(spriteSheetPath)) {
            loadedSpriteSheets.Add(spriteSheetPath);
            
            Sprite[] array = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
            foreach (Sprite sprite in array) {
//                Debug.Log("added " + sprite.name);
                sprites.Add(sprite.name, sprite);
            }
        }
    }

    public static Sprite getSprite(string name) {
//        Debug.Log("get(" + name + ")");
        return sprites[name];
    }
}