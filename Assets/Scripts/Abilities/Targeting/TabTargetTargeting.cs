using UnityEngine;

public class TabTargetTargeting
{
    private Camera mainCamera;
    private LayerMask enemyLayer;

    public TabTargetTargeting()
    {
        // Do not call LayerMask.GetMask here
    }

    public void Initialize()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public void AcquireTarget(AbilityContext context)
    {
        if (enemyLayer == 0)
            Initialize();
        if (mainCamera == null)
            mainCamera = GameManager.Instance.GetMainCamera();
        if (mainCamera == null)
        {
            UnityEngine.Debug.LogWarning("TabTargetTargeting: No main camera found.");
            return;
        }
        // Automatically raycast to mouse position (no click required)
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, enemyLayer))
        {
            context.target = hit.collider.gameObject;
            UnityEngine.Debug.Log($"TabTargetTargeting: Target acquired: {context.target.name}");
        }
        else
        {
            UnityEngine.Debug.Log("TabTargetTargeting: No enemy hit.");
        }
    }
}
