using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {


    public Transform MainCanvas;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMultiplayerButtonClicked()
    {
        Transform multiplayerMenu = MainCanvas.FindChild("MultiplayerMenu");
        multiplayerMenu.gameObject.SetActive(!multiplayerMenu.gameObject.activeSelf);
    }

    public void OnExitGameButtonClicked()  {
        Application.Quit();
    }

    public void OnTeamBuilderButtonClicked()
    {
        SceneManager.LoadScene("SquadBuilder2", LoadSceneMode.Single);
    }
}
