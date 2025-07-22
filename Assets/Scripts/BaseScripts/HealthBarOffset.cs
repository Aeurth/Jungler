using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarOffset : HealthBar
{
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2.1f, 0);
    private Transform cameraTransform;
    private Transform target;

    public void Initialize(Transform target, float maxHealth)
    {
        this.target = target;
        slider.maxValue = maxHealth;
        cameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        if (target == null || cameraTransform == null) return;

        // Position: follow the target with offset
        transform.position = target.position + worldOffset;

        // Rotation: face the camera
        transform.forward = cameraTransform.forward;
    }
}
