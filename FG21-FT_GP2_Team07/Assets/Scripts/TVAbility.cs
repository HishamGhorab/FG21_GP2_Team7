using UnityEngine;

public class TVAbility : MonoBehaviour, IInteractable
{
    public bool InCooldown { get; set; }
    [SerializeField] private CountdownTimer afterCooldownTimer;
    [SerializeField] private GameObject TVOnPanel;
    
    private CountdownTimer myTimer;
    private ChildBehaviour childBehaviour;

    public bool active = false;

    void Start()
    {
        myTimer = GetComponent<CountdownTimer>();
        childBehaviour = GameObject.FindGameObjectWithTag("Child").GetComponent<ChildBehaviour>();
        InCooldown = false;
    }
    public void Activate()
    {
        if(active || InCooldown) 
            return;

        active = true;
        myTimer.Active = true;
        OnPlay();
    }

    public void OnPlay()
    {
        EnableAbility();
        TVOnPanel.SetActive(true);
        childBehaviour.IdleByTV(myTimer.MaxTimerValue);
    }

    public void EndCondition()
    {
        DisableAbility();
        myTimer.Active = false;
        active = false;
        afterCooldownTimer.Active = true;
        InCooldown = true;
    }

    public void DisableAbility()
    {
        Debug.Log("TV is off!");
        TVOnPanel.SetActive(false);
        //close screen
        //make baby move normally
    }

    public void EnableAbility()
    {
        Debug.Log("TV is on!");
        //open screen
        //make baby idle at position
    }
    
}
