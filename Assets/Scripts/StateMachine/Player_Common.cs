using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public partial class PlayerStateMachine : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    int m_moveSpeed = default;
    [SerializeField, Header("振り向く速度")]
    int m_turnSpeed = default;
    [SerializeField, Header("ジャンプ力")]
    int m_jumpPower = default;
    [SerializeField, Header("自身の中央(Pivot)から、真下に線を伸ばしオブジェクトが当たっていたら接地しているとみなす。その際に使用する線の長さ")]
    float m_lineLength = default;

    InputAction m_move,m_jump;
    Rigidbody m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_move = GetComponent<PlayerInput>().currentActionMap["Move"];
        m_jump = GetComponent<PlayerInput>().currentActionMap["Jump"];
        OnStart();
    }

    void Update()
    {
        Move();
        OnUpdate();
    }

    void Move()
    {
        Vector2 v2 = m_move.ReadValue<Vector2>();
        Vector3 dir = Vector3.right * v2.x + Vector3.forward * v2.y;
        if (dir == Vector3.zero)
        {
            if (IsGrounded() == true)
            {
                m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
            }
            else
            {
                m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, m_rb.velocity.z);
            }
        }
        else
        {
            if (IsGrounded())
            {
                dir = Camera.main.transform.TransformDirection(dir);
                dir.y = 0;

                //  入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);

                Vector3 velo = dir * m_moveSpeed;
                velo.y = m_rb.velocity.y;
                m_rb.velocity = velo;
            }
        }
    }

    /// <summary>接地判定</summary>
    bool IsGrounded()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * m_lineLength;
        Debug.DrawLine(start, end);
        bool isGrounded = Physics.Linecast(start, end);
        return isGrounded;
    }
}
