using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFramer : MonoBehaviour
{
    [SerializeField]
    private int ScreenEdgeWorldX;
    void Start()
    {
        Frame();
    }

    public void Frame()
    {
        float camZ = -20;
        float camY = transform.position.y;
        Camera.main.transform.position = new Vector3(0, 0, camZ);
        Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, camZ));
        vec = new Vector3(vec.x, camY, camZ);
        vec += new Vector3(ScreenEdgeWorldX, 0, 0);
        Camera.main.transform.position = vec;
    }
}
