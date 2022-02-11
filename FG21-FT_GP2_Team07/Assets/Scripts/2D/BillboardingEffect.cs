using UnityEngine;

//[ExecuteInEditMode]
public class BillboardingEffect : MonoBehaviour
{
    [SerializeField] public SpriteSet spriteSet;
    [SerializeField] private bool useStaticBillboard = true;
    private SpriteRenderer spriteRenderer;
    private Transform cameraTransform;
    private Transform objectTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        objectTransform = transform.parent.gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        UpdateSpriteRenderer();
    }

    void Update()
    {
        UpdateSpriteRenderer();
        if (useStaticBillboard)
        {
            transform.forward = -cameraTransform.forward;
        }
        else
        {
            transform.LookAt(cameraTransform.transform);
        } 
        
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y +180, 0f);
    }

    public void UpdateSpriteSet(SpriteSet sprites)
    {
        if (sprites != null)
        {
            spriteSet = sprites;
        }
    }

    private void UpdateSpriteRenderer()
    {
        if (spriteSet == null)
        {
            return;
        }
        float signedAngle = Vector2.SignedAngle(
            new Vector2(cameraTransform.forward.x, cameraTransform.forward.z),
            new Vector2(objectTransform.forward.x, objectTransform.forward.z));
        if (signedAngle > -135f && signedAngle < -45f)
        {
            spriteRenderer.sprite = spriteSet.right;
        }
        else if (signedAngle >= -45f && signedAngle <= 45f)
        {
            spriteRenderer.sprite = spriteSet.back;
        }
        else if (signedAngle > 45f && signedAngle < 135f)
        {
            spriteRenderer.sprite = spriteSet.left;
        }
        else
        {
            spriteRenderer.sprite = spriteSet.front;
        }
    }
}
