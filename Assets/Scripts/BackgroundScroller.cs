using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 0.5f;
    private Material myMaterial;
    private Vector2 offSet;

	void Start ()
	{
	    myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0f, backgroundScrollSpeed);
	}
	
	void Update ()
	{
	    myMaterial.mainTextureOffset += offSet * Time.deltaTime;
	}
}
