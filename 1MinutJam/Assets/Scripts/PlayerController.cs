using System;
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

    private float forceImpulse = 100f;
    private bool controlForce = false;

    public float smooth = 4f;
    public float speed = 4f;
    private Vector3 velocity = Vector3.one;
    //delagates

    private float touchingGroundValue = 0.5f;
    private float m_VerticalSpeed = 0.0f;
    private float touchingGround = 0.3f; //initial value
    private bool m_OnGround;

    public delegate void DelegateForceImpulse(float value);
    public static DelegateForceImpulse delegateForceImpulse;

    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_CharacterController = GetComponent<CharacterController>();
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;

        GameController.GetGameController().SetPlayer(this);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        PlayerCamera();
        PlayerMovement();


        if (CanAttach() && Input.GetKeyDown(m_Attach))
            Attach();
        else if (!CanAttach() && Input.GetKeyDown(m_Attach))
            Detach(0f);

        //if (m_AttachObject != null && Input.GetMouseButton(1))
        //    pressed = true;
        //else if (m_AttachObject != null && Input.GetMouseButtonUp(1))
        //{
        //    print("force impulse actual: " + forceImpulse);
        //    Detach(forceImpulse);
        //    forceImpulse = 0;
        //}

        UpdateAttachPosition();

        //if (pressed && m_AttachObject != null)
        //    DetachWithForce();
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

        GameController.GetGameController().GetAreasLight().OpenHelp();
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

        l_Movement.y = m_VerticalSpeed * Time.deltaTime;
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;


        Gravity(l_Movement);
       // m_CharacterController.Move(l_Movement);
    }

    private void Gravity(Vector3 movement)
    {
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(movement);
        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            touchingGround += Time.deltaTime;
            m_OnGround = true;
            m_VerticalSpeed = 0.0f;
        }
        else
            m_OnGround = false;
        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;

        if (touchingGround > touchingGroundValue && m_OnGround)
        {
            //m_VerticalSpeed = m_JumpSpeed;
            touchingGround = 0f;
        }
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
        if (Physics.Raycast(l_ray, out l_RaycastHit, 5.0f, m_AttachLayer.value))
        {
            //if (l_RaycastHit.collider.CompareTag("Object"))
            if (!l_RaycastHit.collider.GetComponent<InteractableObject>().Placed)
            {
                StartAttachObject(l_RaycastHit.collider.gameObject);
                GameController.GetGameController().GetAreasLight().CloseHelp();
            }
               
            //catching = true;
        }
        else if (Physics.Raycast(l_ray, out l_RaycastHit, 5.0f))
        {

            if (l_RaycastHit.collider.CompareTag("Comoda") && !GameController.GetGameController().GetComoda().Opened)
                l_RaycastHit.collider.GetComponent<ComodaMother>().OpenComoda();
        }
    }
    void StartAttachObject(GameObject AttachObject)
    {
        if (m_AttachObject == null)
        {
            m_AttachObject = AttachObject;
            m_AttachObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    public bool CanAttach()
    {
        return m_AttachObject == null;
    }
}
