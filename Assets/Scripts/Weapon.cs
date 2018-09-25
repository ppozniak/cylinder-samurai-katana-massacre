using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public float damage = 10.0f;
  public GameObject blade;
  private EnemyController wielder;

  private void Awake()
  {
    wielder = transform.parent.GetComponent<EnemyController>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("PlayerBody") && wielder.alive)
    {
      other.GetComponent<PlayerController>().takeDamage(damage);
    }
  }
}
