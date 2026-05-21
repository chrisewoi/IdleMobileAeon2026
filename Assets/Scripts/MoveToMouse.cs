using UnityEngine;
using UnityEngine.UI;

public class MoveToMouse : MonoBehaviour
{
    Vector3 newPosition;
    private Vector3 newPositionBG;
    private Camera cam;
    public Transform clickPS, bgPS;
    private ParticleSystemForceField forceField;
    public ParticleSystemForceField forceFieldBG;
    public float clickPower;
    public float maxGravity;
    public Image buttonImage;
    private Color originalColor;
    public Color glowColor;
    void Start () {
        newPosition = transform.position;
        cam = Camera.main;
        forceField = GetComponent<ParticleSystemForceField>();
        clickPower = 1f;
        originalColor = buttonImage.color;
        
        lastMousePos = Input.mousePosition;
    }

    private bool buttonPressed = false;
    public void ButtonPressed()
    {
        buttonPressed = true;
    }
    
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
        {
            newPositionBG = ray.origin;
            newPositionBG.z = bgPS.position.z +0.5f;
            forceFieldBG.transform.position = newPositionBG;
        }
        if (buttonPressed)//Input.GetMouseButtonDown(0))
        {
            buttonPressed = false;
            clickPower += 1.5f;
            var gravity = forceField.gravity;
            gravity.constant = Random.Range(0, clickPower);
            forceField.gravity = gravity;
            //print("gravity:" + gravity.constant);
            
            newPosition = ray.origin;
            newPosition.z = clickPS.position.z;
            transform.position = newPosition;
        }

        clickPower -= Time.deltaTime * 5f;
        clickPower = Mathf.Clamp(clickPower, 1f, maxGravity);
        
        float colorLerp = Mathf.InverseLerp(1f, maxGravity, clickPower);
        //print(colorLerp);
        if(buttonImage)buttonImage.color = Color.Lerp(originalColor, glowColor, colorLerp);

        // Mouse screen position
        Vector3 mousePos = Input.mousePosition;

        // Convert to world position
        Vector3 worldPos = cam.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, 10f)
        );

        worldPos.z = bgPS.position.z ;

        //transform.position = worldPos;
        
        // Mouse velocity
        Vector3 mouseVelocity =
            (mousePos - lastMousePos) / Time.deltaTime;

        lastMousePos = mousePos;

        // Smooth it
        smoothVelocity = Vector3.Lerp(
            smoothVelocity,
            mouseVelocity,
            smoothing * Time.deltaTime
        );

        // Convert screen velocity into world force
        Vector3 force =
            smoothVelocity * velocityMultiplier;

        force = Vector3.ClampMagnitude(force, maxForce);

        // Apply to force field
        forceFieldBG.directionX =
            new ParticleSystem.MinMaxCurve(force.x);

        forceFieldBG.directionZ =
            new ParticleSystem.MinMaxCurve(-force.y);
    }
    
    
    
    
    public float velocityMultiplier = 0.05f;
    public float maxForce = 3f;
    public float smoothing = 10f;

    private Vector3 lastMousePos;
    private Vector3 smoothVelocity;
}
