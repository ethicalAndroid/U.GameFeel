using UnityEngine;
using AethicalTools;

[RequireComponent(typeof(Touching2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CoyotePlatformerController2D : MonoBehaviour
{
    [SerializeField] private float JUMPED_BUFFER_TIME = 0.1f;
    [SerializeField] private float COYOTE_TIME = 0.1f;
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] Animator _animator;
    [SerializeField] AudioManager.ManagedSound _jumpSound;
    int a_IdleSpeed = Animator.StringToHash("IdleSpeed");
    int a_Jump = Animator.StringToHash("Jump");
    int a_Grounded = Animator.StringToHash("Grounded");
    int a_Land = Animator.StringToHash("Land");
    private float _inputHorizontal = 0f;
    private bool _inputJumpPressed = false;
    bool _strafing = false;
    float _animMomentum;
    float _animSpeed;
    bool _airborne = true;
    [SerializeField] float _animMax, _animMin, _smoothtime;

    private Touching2D _touching;
    private Rigidbody2D _rigidbody; // Default GravityScale -> 2f, and PhysicsMaterial2D -> NoFriction 
    private Ticker _jumpBuffer;
    private Ticker _coyoteTicker;

    public void SetStrafe(bool value)
    {
        _strafing = value;
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _touching = GetComponent<Touching2D>();
        _jumpBuffer = new Ticker(JUMPED_BUFFER_TIME, false);
        _coyoteTicker = new Ticker(COYOTE_TIME, true);
    }

    [ContextMenu("ParameterUpdate")]
    private void ParameterUpdate()
    {
        _jumpBuffer = new Ticker(JUMPED_BUFFER_TIME, false);
        _coyoteTicker = new Ticker(COYOTE_TIME, true);
    }

    void Update()
    {
        // Get input from legacy system.
        _inputJumpPressed = Input.GetButtonDown("Jump");
        _inputHorizontal = Input.GetAxis("Horizontal");
        // Keep Coyote time at MAX or Tick down Coyote time.
        if (_touching.Ground)
        {
            if (_airborne)
            {
                _animator.CrossFade(a_Land, 0f);
                _airborne = false;
            }
            _coyoteTicker.WindUp();
            float aim = (Mathf.Abs(_inputHorizontal) > 0.01f) ? _animMax : _animMin;
            _animSpeed = Mathf.SmoothDamp(_animSpeed, aim, ref _animMomentum, _smoothtime * Time.deltaTime);
        }
        else
        {
            _airborne = true;
            _coyoteTicker.Tick(Time.deltaTime);
        }
        _animator.SetFloat(a_IdleSpeed, _animSpeed);
        // Keep Jump buffer at MAX or Tick down Jump buffer.
        if (_inputJumpPressed)
        {
            _jumpBuffer.WindUp();
        }
        else
        {
            _jumpBuffer.Tick(Time.deltaTime);
        }
        // If Coyote time and Jump buffer are both greater than 0f.
        if (!_coyoteTicker.CheckDone() && !_jumpBuffer.CheckDone())
        {
            // Trigger jump by changing Y-axis of velocity.
            _jumpBuffer.WindDown();
            _coyoteTicker.WindDown();
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
            _animator.SetTrigger(a_Jump);
            AudioManager.Instance.PlaySound(_jumpSound);
        }
    }
    void FixedUpdate()
    {

        // Move horizontally by changing X-axis of velocity.
        Vector2 moveVelocity = (_inputHorizontal * _moveSpeed) * Vector2.right;
        moveVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveVelocity;
        // Flip character using Transform.localScale.
        if (Mathf.Abs(_inputHorizontal) > 0.01f && !_strafing)
        {
            Vector3 scale = transform.localScale;
            scale.x = (_inputHorizontal > 0f) ? 1f : -1f;
            transform.localScale = scale;
        }
    }
}