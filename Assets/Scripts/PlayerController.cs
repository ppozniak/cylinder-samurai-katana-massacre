using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  public float mouseSensitivity = 2f;
  public float playerSpeed = 12f;
  public float jumpPower = 50f;
  public float camMaxAngleUp = -75f;
  public float camMaxAngleDown = 90f;
  public new Camera camera;
  public Text killScoreText;
  public GameController _gameController;
  public int maxSlashAnimations = 3;
  public BoxCollider leftTrigger;
  public BoxCollider rightTrigger;
  public BoxCollider middleTrigger;
  public Slider hpSlider;
  public Text hpCurrText;
  public Text hpMaxText;

  private int killScore = 0;
  private CharacterController _charController;
  private Animator _animator;
  private Vector3 movement = Vector3.zero;
  private float gravity = 20.0f;
  private float health;
  private float maxHealth = 100.0f;

  int lastAttackIndex = 0;

  void Awake()
  {
    Cursor.lockState = CursorLockMode.Locked;
    _charController = GetComponent<CharacterController>();
    _gameController = _gameController.GetComponent<GameController>();
    _animator = transform.parent.gameObject.GetComponent<Animator>();
    health = maxHealth;
    hpSlider.maxValue = maxHealth;
    hpSlider.value = health;
    hpCurrText.text = health.ToString();
    hpMaxText.text = maxHealth.ToString();
  }

  void Update()
  {
    if (!_gameController.paused)
    {
      Move();
      MouseLook();
    }
  }

  void Move()
  {
    if (_charController.isGrounded)
    {
      float horizontalSpeed = Input.GetAxisRaw("Horizontal");
      float verticalSpeed = Input.GetAxisRaw("Vertical");

      movement.x = horizontalSpeed;
      movement.y = 0;
      movement.z = verticalSpeed;
      movement = transform.TransformDirection(movement);
      movement *= playerSpeed;

      if (Input.GetButton("Jump"))
      {
        movement.y = 10f;
      }
    }

    movement.y -= gravity * Time.deltaTime;
    _charController.Move(movement * Time.deltaTime);
  }

  void MouseLook()
  {
    float yRot = Input.GetAxis("Mouse X") * mouseSensitivity;
    float xRot = Input.GetAxis("Mouse Y") * mouseSensitivity;

    Quaternion playerTargetRot = transform.localRotation;
    Quaternion camTargetRot = camera.transform.localRotation;

    playerTargetRot *= Quaternion.Euler(0f, yRot, 0f);
    camTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

    camTargetRot = ClampRotationAroundXAxis(camTargetRot);

    camera.transform.localRotation = camTargetRot;
    transform.localRotation = playerTargetRot;
  }

  Quaternion ClampRotationAroundXAxis(Quaternion q)
  {
    q.x /= q.w;
    q.y /= q.w;
    q.z /= q.w;
    q.w = 1.0f;

    float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

    angleX = Mathf.Clamp(angleX, camMaxAngleUp, camMaxAngleDown);

    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

    return q;
  }

  public void Slash(SlashSide slashSide, Collider other)
  {
    if (other.gameObject.CompareTag("Enemy"))
    {
      EnemyController enemyScript = other.GetComponent<EnemyController>();
      if (enemyScript.alive == true)
      {

        enemyScript.Kill();

        bool leftSlash = false;
        bool rightSlash = false;
        int newAttackIndex;


        if (slashSide == SlashSide.Left)
        {
          leftSlash = true;
        }
        else if (slashSide == SlashSide.Right)
        {
          rightSlash = true;
        }
        else if (slashSide == SlashSide.Middle)
        {
          leftSlash = true;
          rightSlash = true;
        }

        do
        {
          newAttackIndex = Random.Range(0, maxSlashAnimations);
        } while (newAttackIndex == lastAttackIndex);


        lastAttackIndex = newAttackIndex;
        _animator.SetFloat("SlashAnimationIndex", newAttackIndex);

        if (leftSlash)
        {
          _animator.SetTrigger("SlashLeft");
        }

        if (rightSlash)
        {
          _animator.SetTrigger("SlashRight");
        }

        killScore++;
        killScoreText.text = killScore.ToString();
      }
    }
  }

  public void takeDamage(float damage)
  {
    health -= damage;
    hpSlider.value = health;
    hpCurrText.text = health.ToString();
    if (health <= 0)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    foreach (ContactPoint contact in collision.contacts)
    {
      Debug.DrawRay(contact.point, contact.normal, Color.white, 5f);
    }
  }
}
