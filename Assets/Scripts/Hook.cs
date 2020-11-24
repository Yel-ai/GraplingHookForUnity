using UnityEngine;

public class Hook : MonoBehaviour
{
    private LineRenderer    lr;
    private Vector3         hookPoint;
    public LayerMask        hookLayer;
    public Transform        gunTip, cam, player;
    private float           maxDistance = 100f;
    private SpringJoint     joint;
    private Vector3         currentHookPosition;

    void Awake() 
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update() 
    {
        cam.position=player.position;

        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    void LateUpdate() 
    {
        DrawRope();
    }  

    void StartGrapple() 
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, hookLayer)) 
        {
            hookPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hookPoint;

            float distanceFromPoint = Vector3.Distance(player.position, hookPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentHookPosition = gunTip.position;
        }
    }

    void StopGrapple() 
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
  
    void DrawRope() 
    {
        if (!joint) return;

        currentHookPosition = Vector3.Lerp(currentHookPosition, hookPoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentHookPosition);
    }

    public bool IsHooking() 
    {
        return joint != null;
    }

    public Vector3 GetHookPoint() 
    {
        return hookPoint;
    }
    
}
