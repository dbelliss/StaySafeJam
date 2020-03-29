using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
  public static RingManager instance;

  List<Ring> rings = new List<Ring>();

  // Start is called before the first frame update
  void Awake()
  {
    instance = this;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void AddRing(Ring ring)
  {
    rings.Add(ring);
  }

  public void RemoveRing(Ring ring)
  {
    rings.Remove(ring);
  }

  public Ring GetClosestRing(Vector3 origin, float maxDistance)
  {
    Ring closest = null;
    float shortestDist = float.MaxValue;
    foreach (Ring ring in rings)
    {
      float curDist = Vector3.Distance(origin, ring.transform.position + Vector3.down * .5f);
      if (curDist < maxDistance && curDist < shortestDist)
      {
        shortestDist = curDist;
        closest = ring;
      }
    }


    return closest;
  }
}
