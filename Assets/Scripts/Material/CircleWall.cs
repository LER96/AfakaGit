using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleWall : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_position");
    public static int SizeID = Shader.PropertyToID("_size");

    public Material wallMaterial;
    public Camera camera;
    public LayerMask mask;

    // Update is called once per frame
    void Update()
    {
        var dir = camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        if (Physics.Raycast(ray, 3000, mask))
            wallMaterial.SetFloat(SizeID, 0.5f);
        else
            wallMaterial.SetFloat(SizeID, 0);

        var view = camera.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(PosID, view);
    }
}
