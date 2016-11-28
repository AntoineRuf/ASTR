using UnityEngine;
using System.Collections.Generic;

public static class Directions {

    static Vector3 up = new Vector3(0, 1, -1);
    static Vector3 down = new Vector3(0, -1, 1);
    static Vector3 upR = new Vector3(+1, 0, -1);
    static Vector3 upL = new Vector3(-1, 1, 0);
    static Vector3 downR = new Vector3(+1, -1, 0);
    static Vector3 downL = new Vector3(-1, 0, 1);
    public static List<Vector3> directions = new List<Vector3>();

    public static void Initialize()
    {
        directions.Add(up);
        directions.Add(down);
        directions.Add(upR);
        directions.Add(upL);
        directions.Add(downR);
        directions.Add(downL);
    }

    public static List<Vector3> getDirections()
    {
        return directions;
    }

    public static Vector2 ConvertToOffsetCoord(Vector3 v)
    {
        return new Vector2(v.x, (v.z + (v.x + (Mathf.Abs(v.x) % 2)) / 2));
    }

    /// <summary>
    /// Convert OffsetCoord to Cube
    /// </summary>
    public static Vector3 ConvertToCube(Vector2 v)
    {
        float x = v.x;
        float z = v.y - (v.x + (Mathf.Abs(v.x) % 2)) / 2;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    public static Vector3 NearestNeighbor(Vector3 casterPos, Vector3 receiverPos){
        float x = casterPos.x - receiverPos.x;
        float y = casterPos.y - receiverPos.y;
        float z = casterPos.z - receiverPos.z;
        if (x != 0) x = x / Mathf.Abs(x);
        if (y != 0) y = y / Mathf.Abs(y);
        if (z != 0) z = z / Mathf.Abs(z);
        return new Vector3(receiverPos.x + x, receiverPos.y + y, receiverPos.z + z);
    }

    public static Vector3 NearestNeighborDirection(Vector3 casterPos, Vector3 receiverPos){
        float x = casterPos.x - receiverPos.x;
        float y = casterPos.y - receiverPos.y;
        float z = casterPos.z - receiverPos.z;
        if (x != 0) x = x / Mathf.Abs(x);
        if (y != 0) y = y / Mathf.Abs(y);
        if (z != 0) z = z / Mathf.Abs(z);
        return new Vector3(x, y, z);
    }
}
