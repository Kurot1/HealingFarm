using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    private string currentAnimation = "Player_WalkDown";
    private bool wasMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 게임 시작 시 아래쪽 첫 프레임에서 정지
        animator.Play(currentAnimation, 0, 0f);
        animator.Update(0f);
        animator.speed = 0f;
    }

    private void Update()
    {
        ReadMovementInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void ReadMovementInput()
    {
        if (Keyboard.current == null)
            return;

        float horizontal =
            (Keyboard.current.dKey.isPressed ||
             Keyboard.current.rightArrowKey.isPressed ? 1f : 0f)
            -
            (Keyboard.current.aKey.isPressed ||
             Keyboard.current.leftArrowKey.isPressed ? 1f : 0f);

        float vertical =
            (Keyboard.current.wKey.isPressed ||
             Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
            -
            (Keyboard.current.sKey.isPressed ||
             Keyboard.current.downArrowKey.isPressed ? 1f : 0f);

        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    private void UpdateAnimation()
    {
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            animator.speed = 1f;

            // 대각선 입력 시 더 강한 방향을 사용
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                if (moveInput.x > 0)
                    PlayAnimation("Player_WalkRight");
                else
                    PlayAnimation("Player_WalkLeft");
            }
            else
            {
                if (moveInput.y > 0)
                    PlayAnimation("Player_WalkUp");
                else
                    PlayAnimation("Player_WalkDown");
            }
        }
        else if (wasMoving)
        {
            // 멈추면 현재 방향의 첫 프레임으로 복귀
            animator.Play(currentAnimation, 0, 0f);
            animator.Update(0f);
            animator.speed = 0f;
        }

        wasMoving = isMoving;
    }

    private void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
            return;

        currentAnimation = animationName;
        animator.Play(currentAnimation);
    }
}