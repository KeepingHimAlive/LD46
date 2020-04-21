using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Transform orbit;
    
    private float angularSpeed = 0;

    public float turnAccel = 20;

    public float maxOrbitSpeed = 45;
        
    // Start is called before the first frame update
    void Start()
    {
        orbit = transform.parent;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var pressingLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        var PressingRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (pressingLeft || PressingRight)
        {
            if (pressingLeft) angularSpeed = Mathf.MoveTowards(angularSpeed, maxOrbitSpeed, turnAccel * Time.deltaTime);
            if (PressingRight) angularSpeed = Mathf.MoveTowards(angularSpeed, -maxOrbitSpeed, turnAccel * Time.deltaTime);
        }
        else
            angularSpeed = Mathf.MoveTowards(angularSpeed, 0, turnAccel * Time.deltaTime);

        orbit.rotation = Quaternion.Euler(0,angularSpeed * 2 * Time.deltaTime, 0) *  orbit.rotation;

    }
}
