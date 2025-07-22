using UnityEngine;

public class CameraEdgePan : MonoBehaviour
{
    [Header("Settings")]
    public float panSpeed = 10f;
    public float edgeThickness = 10f;
    public float edgeOffset = 0f;

    [Header("Movement Bounds")]
    public Vector2 xBounds = new Vector2(-50, 50);
    public Vector2 zBounds = new Vector2(-50, 50);

    [Header("Follow Settings")]
    public Transform player; // Assign in inspector or dynamically
    public Vector3 followOffset = new Vector3(0, 10f, -10f);

    private bool isFollowingPlayer = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            isFollowingPlayer = !isFollowingPlayer;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFollowingPlayer = true;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isFollowingPlayer = false;
        }

        if (isFollowingPlayer)
        {
            transform.position = player.position + followOffset;
        }
        else
        {
            HandleEdgePan();
        }
    }

    private void HandleEdgePan()
    {
        Vector3 pos = transform.position;
        Vector3 moveDir = Vector3.zero;

        Vector3 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Horizontal movement
        if (mousePos.x <= 0f - edgeOffset || mousePos.x < edgeThickness)
            moveDir.x = -1;
        else if (mousePos.x >= screenWidth + edgeOffset || mousePos.x > screenWidth - edgeThickness)
            moveDir.x = 1;

        // Vertical movement (Z-axis in top-down)
        if (mousePos.y <= 0f - edgeOffset || mousePos.y < edgeThickness)
            moveDir.z = -1;
        else if (mousePos.y >= screenHeight + edgeOffset || mousePos.y > screenHeight - edgeThickness)
            moveDir.z = 1;

        // Apply movement
        pos += moveDir.normalized * panSpeed * Time.deltaTime;

        // Clamp bounds
        pos.x = Mathf.Clamp(pos.x, xBounds.x, xBounds.y);
        pos.z = Mathf.Clamp(pos.z, zBounds.x, zBounds.y);

        transform.position = pos;
    }
}
