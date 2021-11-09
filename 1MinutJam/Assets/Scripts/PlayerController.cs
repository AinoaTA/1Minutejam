using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float m_Yaw;
    private float m_Pitch;

    public Transform m_PitchControllerTransform;
    public float m_YawRotationalSpeed = 360.0f;
    public float m_PitchRotationalSpeed = -180.0f;
    public float m_MinPitch = -80.0f;
    public float m_MaxPitch = 50.0f;
    public bool m_InvertedYaw = true;
    public bool m_InvertedPitch = true;
    public LayerMask m_AttachLayer;

    public Camera m_Camera;
    private CharacterController m_CharacterController;

    public float m_Speed = 10.0f;
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;

    public KeyCode m_Attach = KeyCode.Mouse0;
    private bool pressed = false;
    public float m_DotToEnterPortal = 0.5f;

    private GameObject m_AttachObject = null;
    public Transform m_AttachPosition;
    public float m_AttachObjectTime = 1f;
    private float m_CurrentAttachObjectTime = 0f;

    private float forceImpulse = 100f;
    private bool controlForce = false;

    public float smooth = 4f;
    public float speed = 4f;
    private Vector3 velocity = Vector3.one;
    //private bool catching = false;
    //delagates

    public delegate void DelegateForceImpulse(float value);
    public static DelegateForceImpulse delegateForceImpulse;

    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_CharacterController = GetComponent<CharacterController>();
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        PlayerCamera();
        PlayerMovement();


        //print(catching);

        if (CanAttach() && Input.GetKeyDown(m_Attach))
            Attach();

        else if (!CanAttach() && Input.GetKeyDown(m_Attach))
            Detach(0f);



        if (m_AttachObject != null && Input.GetMouseButton(1))
            pressed = true;
        else if (m_AttachObject != null && Input.GetMouseButtonUp(1))
        {
            print("force impulse actual: " + forceImpulse);
            Detach(forceImpulse);
            forceImpulse = 0;
        }

        UpdateAttachPosition();

        if (pressed && m_AttachObject != null)
            DetachWithForce();
    }


    private void DetachWithForce()
    {
        if (Input.GetMouseButton(1) && !controlForce)
        {
            forceImpulse += 10;
            if (forceImpulse >= 1000)
                controlForce = !controlForce;
            delegateForceImpulse?.Invoke(forceImpulse);
        }
        else if (Input.GetMouseButton(1) && controlForce)
        {
            forceImpulse -= 10;
            if (forceImpulse <= 0)
                controlForce = !controlForce;

            delegateForceImpulse?.Invoke(forceImpulse);
        }
    }
    private void Detach(float ForceToApply)
    {


        Rigidbody l_Rigid = m_AttachObject.GetComponent<Rigidbody>();
        l_Rigid.isKinematic = false;
        l_Rigid.AddForce(m_Camera.transform.forward * ForceToApply);
        m_AttachObject.transform.SetParent(null);
        m_AttachObject = null;
        //catching = false;


    }
    private void PlayerCamera()
    {
        float l_MouseAxisY = Input.GetAxis("Mouse Y");
        m_Pitch += l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime * (m_InvertedPitch ? -1.0f : 1.0f);
        float l_MouseAxisX = Input.GetAxis("Mouse X");
        m_Yaw += l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime * (m_InvertedYaw ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);


        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchControllerTransform.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
    }
    private void PlayerMovement()
    {
        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;
        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0.0f, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0.0f, Mathf.Cos(l_Yaw90InRadians));
        Vector3 l_Movement = Vector3.zero;

        if (Input.GetKey(m_UpKeyCode))
            l_Movement = l_Forward;
        else if (Input.GetKey(m_DownKeyCode))
            l_Movement = -l_Forward;
        if (Input.GetKey(m_RightKeyCode))
            l_Movement += l_Right;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Movement -= l_Right;

        l_Movement.Normalize();
        l_Movement = l_Movement * Time.deltaTime * m_Speed;
        l_Movement.y = 0;

        m_CharacterController.Move(l_Movement);


    }
    void UpdateAttachPosition()
    {

        if (m_AttachObject != null)
        {
            m_AttachObject.transform.position = Vector3.SmoothDamp(m_AttachObject.transform.position, m_AttachPosition.position, ref velocity, smooth, speed * Time.deltaTime);
            m_AttachObject.transform.rotation = Quaternion.Lerp(m_AttachObject.transform.rotation, m_AttachPosition.rotation, smooth);
        }
    }

    void Attach()
    {
        Ray l_ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_ray, out l_RaycastHit, 20.0f, m_AttachLayer.value))
        {
            //if (l_RaycastHit.collider.CompareTag("Object"))
            StartAttachObject(l_RaycastHit.collider.gameObject);
            //catching = true;
        }
    }
    void StartAttachObject(GameObject AttachObject)
    {
        if (m_AttachObject == null)
        {
            m_AttachObject = AttachObject;
            m_AttachObject.GetComponent<Rigidbody>().isKinematic = true;
            m_CurrentAttachObjectTime = 0;
        }
    }
    public bool CanAttach()
    {
        return m_AttachObject == null;
    }
}
