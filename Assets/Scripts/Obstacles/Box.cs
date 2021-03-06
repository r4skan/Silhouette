using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Obstacles
{
    private Outline m_outline;

    private void Awake()
    {
        obstacleType = EObstacleType.Movable;

        m_obstacleRigidbody = GetComponent<Rigidbody>();

        m_outline = gameObject.AddComponent<Outline>();

        m_outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        m_outline.OutlineWidth = 2f;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.gameObject.CompareTag("Player") &&
    //        !collision.gameObject.CompareTag("Enemy")) return;

    //    m_obstacleRigidbody.isKinematic = true;
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    m_obstacleRigidbody.isKinematic = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (!collision.gameObject.CompareTag("Player") &&
    //        !collision.gameObject.CompareTag("Enemy")) return;

    //    m_obstacleRigidbody.isKinematic = false;
    //}
}
