using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float speed;
    public float bobSpeed;
    public float bobAmount;
    public float rotateBias;
    private Vector3 startPos;

    private Vector3 startForward;
    void Start()
    {
        startPos = transform.position;
        startForward = transform.forward;
    }

    void Update()
    {
        float dot = Vector3.Dot(startForward, transform.forward);
        // 1 = facing forward
        // -1 = facing backwards
        float speedMult = (dot + 1) / 2; // bring range between 0-1
        speedMult = 1 - speedMult; // now forwards = 0 and backwards = 1;
        print(dot);
        var lr = transform.localRotation;
        transform.Rotate(Vector3.up, (speed * Time.deltaTime) + (rotateBias * speedMult));//Quaternion.Euler(new Vector3(lr.x,lr.y + (speed * Time.deltaTime),lr.z));
        Vector3 finalPos = startPos;
        finalPos.y = startPos.y + (Mathf.Sin(Time.time * bobSpeed) * bobAmount);
        transform.position = finalPos;
    }
}
