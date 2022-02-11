using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ChildBehaviour : MonoBehaviour
{
    [SerializeField] [Tooltip("Preset for each hour. If no preset for given hour exists it will take the last one")]
    private ChildPreset[] behaviourPresets;

    [Header("Exposed variables for testing:")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] public float maxRoamDistance;
    [SerializeField] public float idleTime = 1f;
    [SerializeField] public float interactTime = 2f;
    [SerializeField] [Range(0f, 1f)] public float dangerToPlayerMutiplier;

    [Header("Behaviour weights")] 
    [SerializeField] [Range(0f, 1f)] private float roamingProbability;
    [SerializeField] [Range(0f, 1f)] private float idleProbability;
    [SerializeField] [Range(0f, 1f)] private float normalInteractableProbability;
    [SerializeField] [Range(0f, 1f)] private float dangerousInteractableProbability;
    [SerializeField] [Range(0f, 1f)] private float approachPlayerProbability;
    [SerializeField] [Tooltip("This will happen in game either way")]
    private bool normalizeInInspector;

    [HideInInspector] public SpriteSet idleSprites;
    [HideInInspector] public SpriteSet movingSprites;

    private List<ActivateMinigame> interactionPoints = new List<ActivateMinigame>();
    [HideInInspector] public List<ActivateMinigame> inactiveInteractionPoints = new List<ActivateMinigame>();
    [HideInInspector] public ActivateMinigame currentInteractable;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [Space (10)]
    [SerializeField] private float timeBetweenSteps;
    [SerializeField] private Vector2 minMaxTimeBetweenRandomSounds;
    [SerializeField] private SoundComponent soundComponent;
    [SerializeField] private GameObject stepEffect;
    private int amountOfFootstepSounds = 3;

    [HideInInspector] public BillboardingEffect spriteHandler;
    private int currentPresetIndex = 0;

    private ChildState currentState;
    private ChildState prevousState;
    [Space(10)] 
    [SerializeField] private GameObject tv;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        NormalizeProbabilities();
        FindInteractionPointsInScene();
        spriteHandler = GetComponentInChildren<BillboardingEffect>();
        ToggleStepEffect(false);
        SetPresetsForHour(0);
        ChangeState(new IdleState(this));
        StartCoroutine("startPlayinFootstepSound");
        StartCoroutine("startPlayinRandomSounds");
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeState()
    {
        ChildState randomState = GetRandomState();
        ChangeState(randomState);
    }

    public void ChangeState(ChildState state)
    {
        //Debug.Log($"State: {state.GetType().Name}, distance to target: {Vector3.Distance(transform.position, navMeshAgent.destination)}");
        prevousState = currentState;
        currentState = state;
        UpdateInactiveInteractionPoints();
        currentState.Enter();
    }

    private ChildState GetRandomState()
    {
        NormalizeProbabilities();
        float randomNumber = Random.value;
        if (randomNumber < roamingProbability)
        {
            return new RoamingState(this);
        }
        if (randomNumber < roamingProbability + idleProbability)
        {
            return new IdleState(this);
        }
        if (randomNumber < roamingProbability + idleProbability + dangerousInteractableProbability)
        {
            return new MoveToInteractableState(this);
        }
        if (randomNumber < roamingProbability + idleProbability + dangerousInteractableProbability + normalInteractableProbability)
        {
            return new MoveToInteractableState(this);
        }
        else
        {
            return new ChasingState(this);
        }
    }

    private void FindInteractionPointsInScene()
    {
        interactionPoints = FindObjectsOfType<ActivateMinigame>().ToList();
        UpdateInactiveInteractionPoints();
    }

    private void UpdateInactiveInteractionPoints()
    {
        inactiveInteractionPoints.Clear();
        foreach (ActivateMinigame point in interactionPoints)
        {
            if (!point.Active)
            {
                inactiveInteractionPoints.Add(point);
            }
        }
    }

    private IEnumerator startPlayinFootstepSound()
    {
        while (true)
        {
            if (navMeshAgent.velocity != Vector3.zero)
            {
                spriteHandler.UpdateSpriteSet(movingSprites);
                ToggleStepEffect(true);
                soundComponent.PlaySound(soundComponent.Sounds[Random.Range(0, amountOfFootstepSounds)].name);
                yield return new WaitForSeconds(timeBetweenSteps + Random.Range(-timeBetweenSteps * 0.1f, timeBetweenSteps * 0.1f));
            }
            else
            {
                spriteHandler.UpdateSpriteSet(idleSprites);
                ToggleStepEffect(false);
                yield return null;
            }
        }
    }

    private IEnumerator startPlayinRandomSounds()
    {
        yield return new WaitForSeconds(Random.Range(minMaxTimeBetweenRandomSounds.x, minMaxTimeBetweenRandomSounds.y));
        soundComponent.PlaySound(soundComponent.Sounds[Random.Range(amountOfFootstepSounds, soundComponent.Sounds.Count)].name);
        StartCoroutine("startPlayinRandomSounds");
    }

    public Vector3 RandomPointOnUnitCircle()
    {
        Vector2 onCircle = Random.insideUnitCircle.normalized;
        return new Vector3(onCircle.x, 0f, onCircle.y);
    }

    private void SetPresetsForHour(int hour)
    {
        if (behaviourPresets == null)
        {
            return;
        }

        currentPresetIndex = hour > behaviourPresets.Length - 1 ? behaviourPresets.Length - 1 : hour;
        ChildPreset preset = behaviourPresets[currentPresetIndex];
        spriteHandler.UpdateSpriteSet(idleSprites == spriteHandler.spriteSet ? preset.idle : preset.moving);
        
        roamingProbability = preset.roamingProbability;
        idleProbability = preset.idleProbability;
        normalInteractableProbability = preset.normalInteractableProbability;
        dangerousInteractableProbability = preset.dangerousInteractableProbability;
        approachPlayerProbability = preset.approachPlayerProbability;
        navMeshAgent.speed = moveSpeed = preset.moveSpeed;
        maxRoamDistance = preset.maxRoamDistance;
        idleSprites = preset.idle;
        movingSprites = preset.moving;
        idleTime = preset.idleTime;
        interactTime = preset.interactTime;
        dangerToPlayerMutiplier = preset.dangerToPlayerMutiplier;
        
    }

    private void NormalizeProbabilities()
    {
        float sum = roamingProbability + idleProbability + normalInteractableProbability +
                    dangerousInteractableProbability + approachPlayerProbability;
        if (sum == 0f)
        {
            sum = 1f;
        }

        roamingProbability = roamingProbability * 1 / sum;
        idleProbability = idleProbability * 1 / sum;
        normalInteractableProbability = normalInteractableProbability * 1 / sum;
        dangerousInteractableProbability = dangerousInteractableProbability * 1 / sum;
        approachPlayerProbability = approachPlayerProbability * 1 / sum;
    }

    private void ToggleStepEffect(bool state) => stepEffect.SetActive(state);

    private void OnValidate()
    {
        if (normalizeInInspector)
        {
            NormalizeProbabilities();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 drawFrom = transform.position + Vector3.up;
        Gizmos.DrawLine(drawFrom, drawFrom + transform.forward);
    }

    public void SetChaseProbabilityMultiplier(float multiplier)
    {
        approachPlayerProbability = behaviourPresets[currentPresetIndex].approachPlayerProbability * multiplier;
    }

    public void IdleByTV(float time)
    {
        navMeshAgent.destination = tv != null ? tv.transform.position : Vector3.zero;
        ChangeState(new IdleState(this, time));
    }

    private void OnEnable()
    {
        ClockHappenings.s_UpdateHour += SetPresetsForHour;
    }

    private void OnDisable()
    {
        ClockHappenings.s_UpdateHour -= SetPresetsForHour;
    }
}