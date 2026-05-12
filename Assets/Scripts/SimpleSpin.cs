using UnityEngine;

public class SimpleSpin : MonoBehaviour
{
    public float speed;
    public float ySpeed;

    public bool matchTargetHeight;
    public Transform target;

    public float yOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward, speed * Time.deltaTime);
        if (matchTargetHeight)
        {
            Vector3 position = transform.position;
            position.y = target.position.y + yOffset;
            transform.position = position;
        }
        
        var lr = transform.localRotation;
        transform.Rotate(Vector3.up, ySpeed * Time.deltaTime);
    }
}
