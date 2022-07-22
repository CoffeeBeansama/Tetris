using UnityEngine;

public class scLoopingBackground : MonoBehaviour
{
    public float backgoundSpeed;
    public Renderer backgroundRenderer;

  
    // Update is called once per frame
    void Update()
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(backgoundSpeed * Time.deltaTime, 0f);
    }
}
