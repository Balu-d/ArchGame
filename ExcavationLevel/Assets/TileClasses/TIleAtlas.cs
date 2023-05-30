using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newtileatlas", menuName = "Tile Atlas")]
public class TIleAtlas : ScriptableObject
{
    public TileClass stone;
    public TileClass dirt;
    public TileClass grass;
    public TileClass ruins;
    public TileClass cointreasure;
    public TileClass goldtreasure;
}
