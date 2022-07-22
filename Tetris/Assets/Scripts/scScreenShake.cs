using UnityEngine;

public class scScreenShake : MonoBehaviour
{
  public Animator camAnim;


  public void CamShake()
  {
      camAnim.SetTrigger("Shake");
  }

}
