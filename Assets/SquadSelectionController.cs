using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SquadSelectionController : MonoBehaviour {

    public Transform chosenBuildJ1;
    public Transform chosenBuildJ2;

    public Transform PanelJ1;
    public Transform PanelJ2;

    private List<PlayerData> playerData;
	// Use this for initialization
	void Start () {
        GameControl.Load();
        if (GameControl.playerData == null)
            return;
        playerData = GameControl.playerData;

        int i = 0;
        foreach(var data in playerData)
        {
            i += 20;
            Transform newBuild = Instantiate(Resources.Load<Transform>("Prefabs/ASTR/BuildButton"));
            newBuild.SetParent(PanelJ1);
            newBuild.localPosition = new Vector3(0, 115 -2*i, 0);
            newBuild.localScale = new Vector3(1, 1, 1);
            newBuild.FindChild("Text").GetComponent<Text>().text = data.squadName;
            newBuild.GetComponent<Button>().onClick.AddListener(() => { OnP1BuildButtonClicker(newBuild.FindChild("Text").GetComponent<Text>().text); });

            Transform newBuild2 = Instantiate(Resources.Load<Transform>("Prefabs/ASTR/BuildButton"));
            newBuild2.SetParent(PanelJ2);
            newBuild2.localPosition = new Vector3(0, 115 -2 * i, 0);
            newBuild2.localScale = new Vector3(1, 1, 1);
            newBuild2.FindChild("Text").GetComponent<Text>().text = data.squadName;
            newBuild2.GetComponent<Button>().onClick.AddListener(() => { OnP2BuildButtonClicker(newBuild.FindChild("Text").GetComponent<Text>().text); });
        }

        if(playerData.Count != 0)
        {
            OnP1BuildButtonClicker(playerData[0].squadName);
            OnP2BuildButtonClicker(playerData[0].squadName);
        }
            
        
	}
	
	public void OnP1BuildButtonClicker(string buildName)
    {
        chosenBuildJ1.GetComponentInChildren<Text>().text = buildName;
    }

    public void OnP2BuildButtonClicker(string buildName)
    {
        chosenBuildJ2.GetComponentInChildren<Text>().text = buildName;
    }

    public void onDeleteBuildsButtonClicked()
    {
        GameControl.Delete();
        Start();
    }

    public void onFightButtonClicked()
    {
        PlayerData squad1 = playerData.Find(p => p.squadName == chosenBuildJ1.GetComponentInChildren<Text>().text);
        PlayerData squad2 = playerData.Find(p => p.squadName == chosenBuildJ2.GetComponentInChildren<Text>().text);

        GameObject go = new GameObject();
        DontDestroyOnLoad(go);
        go.AddComponent<SquadSelectionToFightScene>();
        go.GetComponent<SquadSelectionToFightScene>().squad1 = squad1;
        go.GetComponent<SquadSelectionToFightScene>().squad2 = squad2;

        SceneManager.LoadScene("Example Scene 4", LoadSceneMode.Single);
    }

    public void onExitButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }
}
