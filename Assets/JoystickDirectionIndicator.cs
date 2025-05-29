using UnityEngine;

public class JoystickDirectionIndicator : MonoBehaviour
{
    [Header("조이스틱 & 스킬 범위 스프라이트")]
    public VariableJoystick joystick;
    public GameObject directionSpritePrefab;

    [Header("스킬 범위 중앙값 설정")]
    public float distanceFromPlayer = 0.0f; // 캐릭터로부터 앞쪽 거리
    public float spriteBackOffset = 0.0f;   // 스프라이트 pivot 중앙 보정용 뒤쪽 거리

    private GameObject indicatorInstance;

    // 조이스틱 터치 상태 체크용
    private bool isTouchingJoystick = false;
    private bool wasTouchingJoystickLastFrame = false;

    void Start()
    {
        if (directionSpritePrefab != null)
        {
            indicatorInstance = Instantiate(directionSpritePrefab, transform.position, Quaternion.identity);
            indicatorInstance.SetActive(false);
        }
    }

    void Update()
    {
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        isTouchingJoystick = input.magnitude > 0.2f;

        if (isTouchingJoystick)
        {
            if (!indicatorInstance.activeSelf)
                indicatorInstance.SetActive(true);

            Vector3 direction = new Vector3(input.x, input.y, 0f).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            indicatorInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Vector3 basePos = transform.position + direction * distanceFromPlayer;
            Vector3 offset = -indicatorInstance.transform.right * spriteBackOffset;

            indicatorInstance.transform.position = basePos + offset;
        }
        else
        {
            if (indicatorInstance.activeSelf)
                indicatorInstance.SetActive(false);
        }

        // 손 뗀 순간 감지
        if (!isTouchingJoystick && wasTouchingJoystickLastFrame)
        {
            OnJoystickReleased();
        }

        wasTouchingJoystickLastFrame = isTouchingJoystick;
    }

    private void OnJoystickReleased()
    {
        Debug.Log("스킬 발사!!");
        if (indicatorInstance != null)
            indicatorInstance.SetActive(false);
    }
}
