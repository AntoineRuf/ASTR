using UnityEngine;
using System.Collections;

public class CalculsCube : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        int row = 2;
        int col = 1;
        int x = 2;
        int y = -2;
        int z = 0;
        Vector3 up = new Vector3(0, 1, -1);
        Vector3 down = new Vector3(0, -1, 1);
        Vector3 downR = new Vector3(-1, 0, 1);
        Vector3 downL = new Vector3(1, -1, 0);
        Vector3 upR = new Vector3(-1, 1, 0);
        Vector3 upL = new Vector3(1, 0, -1);



        x = row;
        z = col - (row + (Mathf.Abs(row) % 2)) / 2;
        y = -x - z;
        Vector3 cube = new Vector3(x, y, z);
        Debug.Log(cube);
        Debug.Log("UP" + Convert(cube + up));
        Debug.Log("DOWN" + Convert(cube + down));
        Debug.Log("UP R" + Convert(cube + upR));
        Debug.Log("UP L" + Convert(cube + upL));
        Debug.Log("DOWN R" + Convert(cube + downR));
        Debug.Log("DOWN L" + Convert(cube + downL));

    }

    Vector2 Convert(Vector3 v)
    {
        return new Vector2(v.x, (v.z + (v.x - (Mathf.Abs(v.x) % 2)) / 2));
    }
    // Update is called once per frame
    void Update()
    {

    }
}
