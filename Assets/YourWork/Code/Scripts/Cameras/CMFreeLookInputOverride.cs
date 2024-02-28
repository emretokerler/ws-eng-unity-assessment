using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CMFreeLookInputOverride : MonoBehaviour
{
    private CinemachineFreeLook cam => _cam ??= GetComponent<CinemachineFreeLook>(); private CinemachineFreeLook _cam;

    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(0))
            {
                DOTween.Kill(this);
                return Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(0))
            {
                DOTween.Kill(this);
                return Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return Input.GetAxis(axisName);
    }


    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnregisterEvents();
    void RegisterEvents()
    {
        GameEvents.On<StackHighlightedEvent>(HandleStackHighlighted);
    }
    void UnregisterEvents()
    {
        GameEvents.Off<StackHighlightedEvent>(HandleStackHighlighted);
    }

    void HandleStackHighlighted(StackHighlightedEvent e)
    {
        for (int i = 0; i < 3; i++)
        {
            var rig = cam.GetRig(i);
            rig.LookAt = e.StackTemplate.transform;
        }
        // cam.m_XAxis.Value = 0;

        DOTween.To(() => cam.m_XAxis.Value, (x) => cam.m_XAxis.Value = x, 0, 1f).SetEase(Ease.InOutSine).SetId(this);
        DOTween.To(() => cam.m_YAxis.Value, (x) => cam.m_YAxis.Value = x, 0, 1f).SetEase(Ease.InOutSine).SetId(this);
    }
}