using UnityEngine;

public class GroundCursor : MonoBehaviour
{
   public GameObject cursorPrefab;

   private GameObject _cursorObject;
   private Camera _cam;
   private LayerMask _groundMask;

   private void Start()
   {
      _cam = GetComponent<Camera>();

      _groundMask = 1 << LayerMask.NameToLayer("Ground");
      _cursorObject = Instantiate(cursorPrefab);
   }

   private void Update()
   {

      var mouseRay = _cam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(mouseRay, out var hit, 100, _groundMask.value))
      {
         if (_cursorObject.activeSelf == false)
         {
            _cursorObject.SetActive(true);
         }
         //Debug.DrawRay(hit.point, hit.normal, Color.red, .1f);

         _cursorObject.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
         _cursorObject.transform.position = hit.point + new Vector3(0, .01f, 0);
         
      }
      else
      {
         _cursorObject.gameObject.SetActive(false);
      }
      
   }
}
