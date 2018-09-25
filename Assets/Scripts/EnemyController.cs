using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{

  public enum EnemyBehavior
  {
    rotating,
    looking
  }

  public bool alive = true;
  public AudioClip[] deathClips;
  public GameObject weapon;
  public GameObject weaponSlot;
  public EnemyBehavior behavior;


  private GameObject lookAtTarget;
  Renderer _renderer;
  AudioSource audioSource;
  Rigidbody _rigidbody;
  ParticleSystem _particleSystem;

  void Awake()
  {
    _rigidbody = gameObject.GetComponent<Rigidbody>();
    _renderer = gameObject.GetComponent<Renderer>();
    _particleSystem = gameObject.GetComponent<ParticleSystem>();
    audioSource = gameObject.GetComponent<AudioSource>();
    lookAtTarget = GameObject.FindGameObjectWithTag("PlayerBody");
    Instantiate(weapon, weaponSlot.transform.position, weapon.transform.rotation, transform);
  }

  void Update()
  {
    if (alive)
    {
      _rigidbody.velocity = Vector3.zero;
      if (behavior == EnemyBehavior.looking)
      {
        Vector3 relativePosition = new Vector3(lookAtTarget.transform.position.x, transform.position.y, lookAtTarget.transform.position.z);
        transform.LookAt(relativePosition);
      }
      else if (behavior == EnemyBehavior.rotating)
      {
        transform.Rotate(new Vector3(0, 100f * Time.deltaTime, 0));
      }
    }
  }

  public void Kill()
  {
    alive = false;
    _rigidbody.AddRelativeForce(new Vector3(0, 2f, -2f), ForceMode.Impulse);
    _particleSystem.Play();
    _renderer.material.color = Color.gray;
    playRandomDeathSound();
    Invoke("DisposeBody", 5);
  }

  void DisposeBody()
  {
    transform.parent.gameObject.SetActive(false);
  }

  void playRandomDeathSound()
  {
    int randomInt = Mathf.RoundToInt(Random.Range(0, deathClips.Length));
    AudioClip randomClip = deathClips[randomInt];
    audioSource.clip = randomClip;
    audioSource.Play();
  }
}
