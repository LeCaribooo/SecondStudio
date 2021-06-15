using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reinitialiseposition : MonoBehaviour
{
    public Parallaxing Parallaxing;
    private Transform[] backgrounds;
    public bool reinit = false;
    private Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        backgrounds = Parallaxing.backgrounds;
        positions = new Vector3[backgrounds.Length];
        int i = 0;
        foreach(var background in backgrounds)
        {
            positions[i] = background.position;
            i += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (reinit)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                Vector3 vector= positions[i];

                backgrounds[i].position = vector;
            }
            reinit = false;
        }
    }
}
