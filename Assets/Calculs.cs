using UnityEngine;
using System.Collections;

public class Calculs : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int row = 2;
        int col = 1;
        int x = 2;
        int y = -2;
        int z = 0;

        

        x = row;
        z = col - (row + (Mathf.Abs(row) % 2)) / 2;
        y = -x - z;
        Vector3 cube = new Vector3(x, y, z);
        Debug.Log("CUBE : " + x + ',' + y + ',' + z);
        Debug.Log(cube);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
