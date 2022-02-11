using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SmoothVignetteEffect : MonoBehaviour
{
    private GameObject child;
    [SerializeField] [Range(0f, 10f)] private float range = 7f;
    [SerializeField] private bool RequiresLineOfSight = true;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    private Vignette vignette;
    private float value;
    private bool check = true;
    private float childIsNearMultiplier;
    private SoundComponent soundComponent;
    private bool playHeartbeat = true;

    private void Awake()
    {
        transform.root.gameObject.GetComponent<Volume>().profile?.TryGet<Vignette>(out vignette);
        child = GameObject.FindGameObjectWithTag("Child");
        childIsNearMultiplier = child.GetComponent<ChildBehaviour>().dangerToPlayerMutiplier;
    }

    private void Start()
    {
        soundComponent = GetComponent<SoundComponent>();
    }

    private void Update()
    {
        if ((child.transform.position - transform.position).magnitude < range && !OutLineOfSight() && check)
        {
            value += Mathf.Lerp(0, 1, 1f - ((child.transform.position - transform.root.position).magnitude / range)) * (growSpeed * childIsNearMultiplier) * Time.deltaTime;
            vignette.intensity.value = value;

            if (value >= 0.75f)
            {
                check = false;
                GameOver.instance.SetExplanation("LITTLE TIMMY GOT YOU");
            }
        }
        else
        {
            value -= shrinkSpeed * Time.deltaTime;
            value = Mathf.Clamp(value, 0, 1);
            vignette.intensity.value = value;
        }

        if(playHeartbeat && value > 0.2f)
        {
            StartCoroutine(heartbeatCooldown(Mathf.Lerp(1.5f, 0.2f, value), Mathf.Lerp(0f, 0.5f, value)));
        }
    }

    private bool OutLineOfSight()
    {
        if (RequiresLineOfSight)
            return Physics.Raycast(transform.position, child.transform.position - transform.position, (child.transform.position - transform.position).magnitude);
        else
            return false;
    }
    private void SetMultiplier(int i) => childIsNearMultiplier = child.GetComponent<ChildBehaviour>().dangerToPlayerMutiplier;
    private IEnumerator heartbeatCooldown(float time, float volume)
    {
        playHeartbeat = false;
        soundComponent.SetAudioSourceVolume(volume);
        soundComponent.PlaySound("Heartbeat");
        yield return new WaitForSeconds(time);
        playHeartbeat = true;
    }
    private void OnEnable()
    {
        ClockHappenings.s_UpdateHour += SetMultiplier;
    }
    private void OnDisable()
    {
        ClockHappenings.s_UpdateHour -= SetMultiplier;
    }
}
