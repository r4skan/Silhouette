using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

[System.Serializable]
public struct tPlayerCommand
{
    public KeyCode playerWalk;
    public KeyCode playerCrouch;
    public KeyCode playerInteraction;
    public KeyCode playerThrowReady;
    public KeyCode playerThrowSomething;
}

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance
    {
        get { return s_instance; }
    }
    private static PlayerInput s_instance;

    [HideInInspector] public bool playerControllerInputBlocked;

    [SerializeField]
    // 마우스 입력
    private Camera m_camera;
    private RaycastHit m_mouseHit;
    private bool m_isMouseClickedGround;

    private tPlayerCommand m_command;
    
    // 플레이어 움직임 관련 입력
    private bool m_walk;
    private bool m_crouch;
    private bool m_throwReady;
    private bool m_throwSomething;

    // 상호작용 입력
    private bool m_interaction;

    // getter / setter
    public RaycastHit MouseHit
    {
        get
        {
            if (playerControllerInputBlocked)
                return new RaycastHit();

            return m_mouseHit;
        }
    }

    public bool MouseInput
    {
        get
        {
            return m_isMouseClickedGround && !playerControllerInputBlocked;
        }
    }

    public bool WalkInput
    {
        get
        {
            return m_walk && !playerControllerInputBlocked;
        }
    }
    
    public bool CrouchInput
    {
        get
        {
            return m_crouch && !playerControllerInputBlocked;
        }
    }
    
    public bool ReadyToThrowInput
    {
        get
        {
            return m_throwReady && !playerControllerInputBlocked;
        }
    }
    
    public bool ThrowInput
    {
        get
        {
            return m_throwSomething && !playerControllerInputBlocked;
        }
    }

    public bool InteractionInput
    {
        get { return m_interaction && !playerControllerInputBlocked; }
    }
    

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Debug.Log("플레이어 Input은 오직 하나만 존재할 수 있습니다.");
            Destroy(gameObject);
        }

        // 마우스 입력을 감지할 카메라 설정
        if (m_camera == null)
        {
            m_camera = Camera.main;
        }
    }

    private void Update()
    {
        CalcMouseHit();
        
        m_walk = Input.GetKeyDown(m_command.playerWalk);
        m_crouch = Input.GetKeyDown(m_command.playerCrouch);
        m_throwReady = Input.GetKeyDown(m_command.playerThrowReady);
        m_throwSomething = Input.GetKeyDown(m_command.playerThrowSomething);
        
        m_interaction = Input.GetKeyDown(m_command.playerInteraction);
    }

    private void CalcMouseHit()
    {
        Ray _ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out var _hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            m_mouseHit = _hit;
            m_isMouseClickedGround = true;
            return;
        }

        m_isMouseClickedGround = false;
    }
}
