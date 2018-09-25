using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public bool paused = false;

  public void togglePause(bool toggle)
  {
    paused = toggle;
  }

  void Update()
  {
    if (Input.GetKeyDown("escape") || Input.GetKeyDown("p"))
    {
      if (paused)
      {
        togglePause(false);
        Cursor.lockState = CursorLockMode.Locked;
      }
      else
      {
        togglePause(true);
        Cursor.lockState = CursorLockMode.None;
      }
    }
  }
}
