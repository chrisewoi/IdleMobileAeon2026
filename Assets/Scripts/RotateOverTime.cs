using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float speed;
    public float bobSpeed;
    public float bobAmount;
    public float rotateBias;
    private Vector3 startPos;
    public float spinMin, spinMax, spinChange;
    [SerializeField]private float spinRandomMult;

    private Vector3 startForward;
    public bool randomSpin;
    void Start()
    {
        startPos = transform.position;
        startForward = transform.forward;
        random = 1;
        spinRandomMult = 1f; //(spinMax + spinMin)/2f; // start in middle
    }

    private float random;
    void Update()
    {
        spinRandomMult += (Random.value > 0.5f ? spinChange : -spinChange)*Time.deltaTime;
        spinRandomMult = Mathf.Clamp(spinRandomMult, spinMin, spinMax);
        float dot = Vector3.Dot(startForward, transform.forward);
        random = dot > 0.99 ? Random.value > 0.5 ? 1f : -1f : random;
        // 1 = facing forward
        // -1 = facing backwards
        float speedMult = (dot + 1) / 2; // bring range between 0-1
        speedMult = 1 - speedMult; // now forwards = 0 and backwards = 1;
        print(dot);
        var lr = transform.localRotation;
        float finalRandom = randomSpin ? random : 1f;
        transform.Rotate(Vector3.up, ( speed * finalRandom * spinRandomMult * Time.deltaTime) + (rotateBias * finalRandom * spinRandomMult * speedMult));//Quaternion.Euler(new Vector3(lr.x,lr.y + (speed * Time.deltaTime),lr.z));
        Vector3 finalPos = startPos;
        finalPos.y = startPos.y + (Mathf.Sin(Time.time * bobSpeed) * bobAmount);
        transform.position = finalPos;
    }
}
