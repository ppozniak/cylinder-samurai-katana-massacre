using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlashSide { Left, Middle, Right }

public class SlashTrigger : MonoBehaviour
{
  public SlashSide slashSide;
  public GameObject player;
  private PlayerController _playerController;

  void Start()
  {
    _playerController = player.GetComponent<PlayerController>();
  }


  void OnTriggerEnter(Collider other)
  {
    _playerController.Slash(slashSide, other);
  }
}
