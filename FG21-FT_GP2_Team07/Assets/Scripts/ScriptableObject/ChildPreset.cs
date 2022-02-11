using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Child/Preset")]
public class ChildPreset : ScriptableObject
{
    [Header("Behaviour weights")]
    [Range(0f, 1f)] public float roamingProbability = 1f;
    [Range(0f, 1f)] public float idleProbability = 1f;
    [Range(0f, 1f)] public float normalInteractableProbability = 1f;
    [Range(0f, 1f)] public float dangerousInteractableProbability = 1f;
    [Range(0f, 1f)] public float approachPlayerProbability = 1f;
    [Space(10)]
    public float moveSpeed = 1f;
    [Tooltip("Distance in meters")] public float maxRoamDistance = 5f;
    public float idleTime = 1f;
    public float interactTime = 2f;
    [Range(0f, 1f)] public float dangerToPlayerMutiplier = 0.1f;

    [Header("Sprite sets")] 
    public SpriteSet idle;
    public SpriteSet moving;
    
}
