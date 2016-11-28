using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnController : MonoBehaviour
{


    public Transform MainCanvas;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnReturnToMenuButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);

    }
}
