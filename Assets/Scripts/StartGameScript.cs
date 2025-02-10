using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    
    public void StartGameButton()
    {
        SceneManager.LoadScene("InGame");
    }



}
