using UnityEngine;
using UnityEngine.SceneManagement;


public class scPress2Start : MonoBehaviour
{
    
    public void Easy()
    {
       
            SceneManager.LoadScene("Stage_1");
            
        
    }

    public void Average()
    {
       
            SceneManager.LoadScene("Stage_2");
            
        
    }

    public void Expert()
    {
       
            SceneManager.LoadScene("Stage_3");
            
        
    }

    

 
   
}
