using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ChildBehaviourOld : MonoBehaviour
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
    private SpriteSet idleSprites;
    private SpriteSet movingSprites;

    private List<ActivateMinigame> interactionPoints = new List<ActivateMinigame>();
    public List<ActivateMinigame> inactiveInteractionPoints = new List<ActivateMinigame>();
    public ActivateMinigame currentInteractable;
    private GameObject player;
    internal NavMeshAgent navMeshAgent;
    private BillboardingEffect spriteHandler;
    private ChildStateOld currentState = ChildStateOld.Idle;
    private ChildStateOld prevousState = ChildStateOld.Idle;
    private float idleTimer;
    private float interactTimer;
    private bool enteredNewState;
    private int currentPresetIndex = 0;

    [SerializeField] private GameObject stepEffect;

    private void Awake()
    {
        idleTimer = idleTime;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        NormalizeProbabilities();
        FindInteractionPointsInScene();
        spriteHandler = GetComponentInChildren<BillboardingEffect>();
        spriteHandler.UpdateSpriteSet(idleSprites);
        ToggleStepEffect(false);
    }

    private void Update()
    {
        switch (currentState)
        {
            case ChildStateOld.Idle:
                IdleBehaviour();
                break;
            case ChildStateOld.Roaming:
                RoamingBehaviour();
                break;
            case ChildStateOld.MoveToInteraction:
                if (inactiveInteractionPoints.Count > 0)
                {
                    MoveToInteractableBehaviour();
                }
                else
                {
                    RoamingBehaviour();
                }

                break;
            case ChildStateOld.Interacting:
                if (inactiveInteractionPoints.Count > 0)
                {
                    InteractionBehaviour();
                }
                else
                {
                    RoamingBehaviour();
                }

                break;
            case ChildStateOld.Chasing:
                ChaseBehaviour();
                break;
        }
    }

    private void RoamingBehaviour()
    {
        ToggleStepEffect(true);
        if (navMeshAgent.remainingDistance < 0.01)
        {
            if (enteredNewState)
            {
                navMeshAgent.destination = transform.position + RandomPointOnUnitCircle() * maxRoamDistance;
                enteredNewState = false;
            }
            else
            {
                ChangeState();
            }
        }
    }

    private void MoveToInteractableBehaviour()
    {
        ToggleStepEffect(true);
        if (navMeshAgent.remainingDistance < 0.01)
        {
            if (enteredNewState)
            {
                if (inactiveInteractionPoints.Count > 0)
                {
                    currentInteractable = inactiveInteractionPoints[Random.Range(0, inactiveInteractionPoints.Count)];
                }
                if (!currentInteractable.Active)
                {
                    navMeshAgent.destination = currentInteractable.transform.position;
                }
                else
                {
                    ChangeState(ChildStateOld.Roaming);
                }

                enteredNewState = false;
            }
            else
            {
                ChangeState(ChildStateOld.Interacting);
            }
        }
    }

    private void InteractionBehaviour()
    {
        ToggleStepEffect(false);
        if (interactTimer <= 0f)
        {
            interactTimer = interactTime;
            currentInteractable.BabyActivate();
            ChangeState();
        }
        else
        {
            interactTimer -= Time.deltaTime;
        }
    }

    private void IdleBehaviour()
    {
        ToggleStepEffect(false);
        if (idleTimer <= 0f)
        {
            idleTimer = idleTime;
            ChangeState();
        }
        else
        {
            idleTimer -= Time.deltaTime;
        }
    }

    private void ChaseBehaviour()
    {
        ToggleStepEffect(true);
        if (navMeshAgent.remainingDistance < 0.01)
        {
            if (enteredNewState)
            {
                navMeshAgent.destination = player.transform.position + RandomPointOnUnitCircle() * 2;
                enteredNewState = false;
            }
            else
            {
                ChangeState();
            }
        }
    }

    public void ChangeState()
    {
        ChildStateOld randomState = GetRandomState();
        if (randomState == ChildStateOld.Interacting)
        {
            randomState = ChildStateOld.Roaming;
        }

        ChangeState(randomState);
    }

    public void ChangeState(ChildStateOld state)
    {
        prevousState = currentState;
        currentState = state;
        enteredNewState = true;
        if (currentState == ChildStateOld.Idle || currentState == ChildStateOld.Interacting)
        {
            spriteHandler.UpdateSpriteSet(idleSprites);
        }
        else
        {
            spriteHandler.UpdateSpriteSet(movingSprites);
        }
        UpdateInactiveInteractionPoints();
    }

    private ChildStateOld GetRandomState()
    {
        NormalizeProbabilities();
        float randomNumber = Random.value;
        if (randomNumber < roamingProbability)
        {
            return ChildStateOld.Roaming;
        }
        if (randomNumber < roamingProbability + idleProbability)
        {
            return ChildStateOld.Idle;
        }
        if (randomNumber < roamingProbability + idleProbability + dangerousInteractableProbability)
        {
            return ChildStateOld.MoveToInteraction;
        }
        if (randomNumber < roamingProbability + idleProbability + dangerousInteractableProbability + normalInteractableProbability)
        {
            return ChildStateOld.MoveToInteraction;
        }
        else
        {
            return ChildStateOld.Chasing;
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

    private void OnEnable()
    {
        ClockHappenings.s_UpdateHour += SetPresetsForHour;
    }

    private void OnDisable()
    {
        ClockHappenings.s_UpdateHour -= SetPresetsForHour;
    }
}