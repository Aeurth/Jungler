using UnityEngine;
public class SkillShotTargeting
{
    public void AcquireTarget(AbilityContext context)
    {
        Camera cam = GameManager.Instance.GetMainCamera();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        int groundLayer = LayerMask.GetMask("Ground");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, groundLayer))
        {
            context.position = new Vector3(hit.point.x, 0, hit.point.z);
            UnityEngine.Debug.Log($"SkillShotTargeting: Ground hit at {hit.point}");
        }
        else
        {
            UnityEngine.Debug.Log("SkillShotTargeting: No ground hit.");
        }
    }
}
