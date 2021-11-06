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

    private bool m_OnGround = false;
    public float m_DotToEnterPortal = 0.5f;

    private GameObject m_AttachObject = null;
    public Transform m_AttachPosition;
    public float m_AttachObjectTime = 1f;
    private float m_CurrentAttachObjectTime = 0f;
    private float smooth = 19f;
    private float smoothRot = 5f;

    private float m_DetachForce = 550f;

    Vector3 m_Direction;

    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_CharacterController = GetComponent<CharacterController>();
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;

        //GameController.GetGameController().SetPlayer(this);
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

        if (m_AttachObject != null && Input.GetKeyDown(m_Attach))
            Detach(0f);
        if (m_AttachObject != null && Input.GetMouseButtonDown(1))
            Detach(m_DetachForce);


        UpdateAttachPosition();
    }

    private void Detach(float ForceToApply)
    {
        if (m_CurrentAttachObjectTime >= m_AttachObjectTime)
        {
            Rigidbody l_Rigid = m_AttachObject.GetComponent<Rigidbody>();
            l_Rigid.isKinematic = false;
            l_Rigid.AddForce(m_Camera.transform.forward * ForceToApply);
            m_AttachObject.transform.SetParent(null);
            m_AttachObject = null;
        }
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

        if (m_AttachObject != null && m_CurrentAttachObjectTime < m_AttachObjectTime)
        {
            m_CurrentAttachObjectTime += Time.deltaTime;
            float l_Pct = Mathf.Min(1.0f, m_CurrentAttachObjectTime / m_AttachObjectTime);
            m_AttachObject.transform.position = Vector3.Lerp(m_AttachObject.transform.position, m_AttachPosition.position, m_CurrentAttachObjectTime / m_AttachObjectTime);
            m_AttachObject.transform.rotation = Quaternion.Lerp(m_AttachObject.transform.rotation, m_AttachPosition.rotation, m_CurrentAttachObjectTime / m_AttachObjectTime);

            if (l_Pct == 1.0f)
            {
                m_AttachObject.transform.SetParent(m_AttachPosition);
            }
        }
    }

    void Attach()
    {
        Ray l_ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_ray, out l_RaycastHit, 20.0f, m_AttachLayer.value))
        {
            if (l_RaycastHit.collider.CompareTag("Object"))
                StartAttachObject(l_RaycastHit.collider.gameObject);
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
