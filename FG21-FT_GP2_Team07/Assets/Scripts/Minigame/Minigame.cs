using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public void Finished()
    {
        GetComponentInParent<ActivateMinigame>().Finished();
    }
}
