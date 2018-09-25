using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotGizmo : MonoBehaviour
{
  public Color gizmoColor = Color.blue;
  public float gizmoSize = .5f;

  void OnDrawGizmos()
  {
    Gizmos.color = gizmoColor;
    Gizmos.DrawWireSphere(transform.position, gizmoSize);
  }
}
