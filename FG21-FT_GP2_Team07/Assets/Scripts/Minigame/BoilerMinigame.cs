using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoilerMinigame : Minigame
{
    [SerializeField] private Button tempUp;
    [SerializeField] private Button tempDown;
    [SerializeField] private Button pressureUp;
    [SerializeField] private Button pressureDown;
    [SerializeField] private GameObject tempPointer;
    [SerializeField] private GameObject pressurePointer;
    [SerializeField][Range(0f,1f)] private float changePerClick;
    [SerializeField][Range(0f,1f)] private float changeVariety;
    [SerializeField][Range(0f,1f)] private float distanceToMiddleForWin;
    [SerializeField][Range(0f,180f)] private float maxRotationDegree;
    [SerializeField] private float initialOffset;

    private SoundComponent soundComponent;
    private float temperature;
    private float pressure;

    private void Start()
    {
        soundComponent = GetComponent<SoundComponent>();
        Reset();
    }

    public void IncreaseTemperature()
    {
        ChangeValue(ref temperature, 1);
        soundComponent.PlaySound("Change Temperature");
    }

    public void DecreaseTemperature()
    {
        ChangeValue(ref temperature, -1);
        soundComponent.PlaySound("Change Temperature");
    }
    
    public void IncreasePressure()
    {
        ChangeValue(ref pressure, 1);
        soundComponent.PlaySound("Change Pressure");
    }

    public void DecreasePressure()
    {
        ChangeValue(ref pressure, -1);
        soundComponent.PlaySound("Change Pressure");
    }

    private void ChangeValue(ref float value, float sign)
    {
        float difference = Mathf.Sign(sign) * changePerClick + Random.Range(-changeVariety, changeVariety) * changePerClick;
        value = Mathf.Clamp(value + difference, -1f, 1f);
        UpdatePointers();
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (Mathf.Abs(temperature) < distanceToMiddleForWin && Mathf.Abs(pressure) < distanceToMiddleForWin)
        {
            StartCoroutine(Finish());
        }
    }

    private void Reset()
    {
        temperature = Random.Range(0, 2) == 0
            ? Random.Range(distanceToMiddleForWin, 1f)
            : -Random.Range(distanceToMiddleForWin, 1f);
        pressure = Random.Range(0, 1) == 0
            ? Random.Range(distanceToMiddleForWin, 1f)
            : -Random.Range(distanceToMiddleForWin, 1f);
        UpdatePointers();
    }
    
    private void UpdatePointers()
    {
        tempPointer.transform.rotation = quaternion.identity;
        tempPointer.transform.Rotate(new Vector3(0,0,initialOffset - temperature * maxRotationDegree));
        pressurePointer.transform.rotation = quaternion.identity;
        pressurePointer.transform.Rotate(new Vector3(0,0,initialOffset - pressure * maxRotationDegree));
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(.2f);
        Reset();
        Finished();
    }
}
