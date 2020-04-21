using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ObjectShooter : MonoBehaviour
{

    public List<Rigidbody> spawnablePrefabs;
    public Vector3 spawnPointOffset;
    public float spawnSpeed;
    
    private Camera cam;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            var rb = Instantiate(
                spawnablePrefabs[Random.Range(0, spawnablePrefabs.Capacity)], 
                transform.position + transform.TransformDirection(spawnPointOffset),
                Quaternion.Euler(Random.insideUnitSphere * 90),null);

            var childRBs = rb.GetComponentsInChildren<Rigidbody>();

            for (var i = 0; i < childRBs.Length; i++)
            {
                var childRb = childRBs[i];
                childRb.velocity = Quaternion.AngleAxis(-10, transform.right) * (mouseRay.direction * spawnSpeed);
                childRb.transform.SetParent(null, true);
            }
        }
    }
}
