using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
  RingManager ringManager;

  // Start is called before the first frame update
  void Start()
  {
    ringManager = RingManager.instance;
    ringManager.AddRing(this);
  }

}
