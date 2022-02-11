using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class FuseBox : Minigame, IMinigame
{
    [FormerlySerializedAs("knobs")] [SerializeField] private Button[] fuseswitch = new Button[12];
    private int[] fuse = new int[12];

    // to check if it's enabled or not
    private Canvas canvas;
    private bool canvasEnabled;
    [SerializeField] private int minimum = 5;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        Flip(minimum);
    }
    
    private int flipSwitches()
    {
        int counter = 0;
        for (int i = 0; i < fuseswitch.Length; i++)
        {
            fuse[i] = Random.Range(0, 2);
            fuseswitch[i].GetComponentInChildren<Text>().text = fuse[i].ToString();
            fuseswitch[i].GetComponent<FuseTurn>().SetRotation();
            if (fuse[i] == 1)
             counter++;
        }
        return counter;
    }
    
    private void Flip(int min)
    {
        int switchesFlipped = 0;
        while (switchesFlipped < min)
        {
            switchesFlipped = flipSwitches();
        }
    }

    private void WiningCondition()
    {
        int turnedSwitches = 0;
        for (int i = 0; i < fuseswitch.Length; i++)
        {
            if (fuse[i] == 0)
            {
                turnedSwitches++;
            }
        }

        if (turnedSwitches == 12)
        {
            LightManager.Instance.TurnOffAllLamps(true);
            StartCoroutine(Finish());
        }
    }

    private void HasCanvasBeenEnabled()
    {
        if (canvas.enabled && !canvasEnabled)
        {
            canvasEnabled = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(fuseswitch[0].gameObject);
        }
        else if (!canvas.enabled)
        {
            canvasEnabled = false;
        }
    }

    void Update()
    {
        HasCanvasBeenEnabled();
        for (int i = 0; i < fuseswitch.Length; i++)
        {
            fuse[i] = Int32.Parse(fuseswitch[i].GetComponentInChildren<Text>().text);
        }

        WiningCondition();
    }

    public void EastControllerButton()
    {
        throw new NotImplementedException();
    }
    
    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(.7f);
        flipSwitches();
        Finished();
    }
  
    
    
}

