using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable] public class PlayerCamera
{
    private Vector3 positionNormal = new Vector3(0,0,-10);
    private Vector3 positionMoved = new Vector3(1.5f, 0, -10);

    [SerializeField] private GameObject m_playerCamera;
    [SerializeField] private float Duration = 2;

    public void CheckReference()
    {
        if (!m_playerCamera) Debug.LogError("Player has no camera");
    }

    async public UniTask MoveCamera(bool Moved)
    {
        float progress = 0;
        Vector3 targetPosition = Moved ? positionMoved : positionNormal;
        while (progress < Duration)
        {
            m_playerCamera.transform.localPosition = Vector3.Lerp(m_playerCamera.transform.localPosition, targetPosition, progress/2);
            progress += Time.deltaTime;
            await UniTask.Yield();
            if (m_playerCamera.transform.localPosition == targetPosition) break;
        }
        //Debug.Log("Camera is done Moved");
        m_playerCamera.transform.localPosition = targetPosition;
    }
}