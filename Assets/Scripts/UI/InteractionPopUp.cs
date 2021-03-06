using UnityEngine;
using UnityEngine.UI;
using Player;

public class InteractionPopUp : MonoBehaviour
{
    private PlayerController m_player;

    [SerializeField] private GameObject m_popUpTarget;

    private void Awake()
    {
        Invoke(nameof(Init), 1f);
    }

    private void Init()
    {
        m_player = GameManager.Instance.Player;
        
        m_player.interactionPopUpEvent += PopUpUI;
        m_player.popUpReleaseEvent += ReleasePopUp;
    }

    public void PopUpUI()
    {
        if (!m_popUpTarget || m_popUpTarget.activeSelf) return;

        transform.position = m_player.targetObj.transform.position + Vector3.up * 2f;

        m_popUpTarget.SetActive(true);
    }

    public void ReleasePopUp()
    {
        if (!m_popUpTarget || !m_popUpTarget.activeSelf) return;

        m_popUpTarget.SetActive(false);
    }
}
