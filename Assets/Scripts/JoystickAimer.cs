using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickAimer : MonoBehaviour
{
    [SerializeField]
    private RectTransform joystickBackground;
    [SerializeField]
    private RectTransform aimer;
    [SerializeField]
    private float moveSpeed = 100f;

    private Vector2 joystickCenter;
    private Vector2 currentAimerPosition;

    void Start()
    {
        joystickCenter = joystickBackground.anchoredPosition;
        currentAimerPosition = joystickCenter;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Joystick horizontal");
        float verticalInput = Input.GetAxis("Joystick vertical");

        Vector2 inputVector = new Vector2(horizontalInput, -verticalInput);
        currentAimerPosition += inputVector * moveSpeed * Time.deltaTime;

        float maxX = joystickBackground.rect.width / 2;
        float maxY = joystickBackground.rect.height / 2;
        currentAimerPosition.x = Mathf.Clamp(currentAimerPosition.x, joystickCenter.x - maxX, joystickCenter.x + maxX);
        currentAimerPosition.y = Mathf.Clamp(currentAimerPosition.y, joystickCenter.y - maxY, joystickCenter.y + maxY);

        aimer.anchoredPosition = currentAimerPosition;
    }
}
