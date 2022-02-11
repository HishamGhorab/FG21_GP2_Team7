using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sprite/SpriteSet")]
public class SpriteSet : ScriptableObject
{
    public Sprite front;
    public Sprite back;
    public Sprite right;
    public Sprite left;
}
