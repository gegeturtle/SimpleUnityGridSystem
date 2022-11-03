using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.Mathematics;

[RequireComponent(typeof(PlayerInput))]
public class KeyboardListener : MonoBehaviour
{
    PlayerInput inputAction;
    GameObject cam;
    [SerializeField] float camMoveSpeed = 20f;
    [SerializeField] float camRotateSpeed = 45f;
    public UnityEvent CameraRotated = new();
    public UnityEvent<Vector3> GridClick = new();
    private void Start()
    {
        inputAction = GetComponent<PlayerInput>();
        cam = GameObject.Find("CamContainer");
        Transform camera = cam.GetComponentInChildren<Camera>().transform;
        camera.position -= new Vector3(0, (math.tan(math.radians(360-camera.eulerAngles.x))*math.abs(camera.position.z)) ,0);
    }
    private void Update()
    {
        if (inputAction == null || cam == null)
        {

            inputAction = GetComponent<PlayerInput>();
            cam = GameObject.Find("CamContainer");
            return;
        }
        if (inputAction.actions["Rotate Right"].IsPressed()) RotateRight();
        if (inputAction.actions["Rotate Left"].IsPressed()) RotateLeft();
        
        if (inputAction.actions["Move Up"].IsPressed()) MoveUp();
        if (inputAction.actions["Move Down"].IsPressed()) MoveDown();
        if (inputAction.actions["Move Right"].IsPressed()) MoveRight();
        if (inputAction.actions["Move Left"].IsPressed()) MoveLeft();
        if (inputAction.actions["Click"].triggered) Click(inputAction.actions["Mouse Position"].ReadValue<Vector2>());
    }
    public void RotateRight()
    {
        SmoothCamRotate(false);
    }
    public void RotateLeft()
    {
        SmoothCamRotate(true);
    }
    public void MoveUp()
    {
        Vector3 movement = Quaternion.AngleAxis(cam.transform.rotation.y,Vector3.up) * new Vector3(0, camMoveSpeed, 0);
        cam.transform.Translate(movement * Time.deltaTime);
    }
    public void MoveDown()
    {
        Vector3 movement = Quaternion.AngleAxis(cam.transform.rotation.y, Vector3.up) * new Vector3(0, -camMoveSpeed, 0);
        cam.transform.Translate(movement * Time.deltaTime);
    }
    public void MoveLeft()
    {
        Vector3 movement = Quaternion.AngleAxis(cam.transform.rotation.y, Vector3.up) * new Vector3(-camMoveSpeed, 0, 0);
        cam.transform.Translate(movement * Time.deltaTime);
    }
    public void MoveRight()
    {
        Vector3 movement = Quaternion.AngleAxis(cam.transform.rotation.y, Vector3.up) * new Vector3(camMoveSpeed, 0, 0);
        cam.transform.Translate(movement * Time.deltaTime);
    }
    void SmoothCamRotate(bool isCW)
    {
        float mult = 0f;
        if (isCW) mult = -1f * Time.deltaTime;
        if (!isCW) mult = 1f * Time.deltaTime;
        cam.transform.Rotate(0, 0, mult * camRotateSpeed, Space.Self);
        CameraRotated.Invoke();
    }
    void Click(Vector3 value)
    {
        Ray ray = cam.GetComponentInChildren<Camera>().ScreenPointToRay(value);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity))
        {
            GridClick.Invoke(hit.transform.position);
        }
    }
}
