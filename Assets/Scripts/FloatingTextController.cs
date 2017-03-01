using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour {

    public float moveSpeed;
    public string textToDisplay;
    public Enums.FloatingTextType type;
    public Text textControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), transform.position.z);
        if (type == Enums.FloatingTextType.Damage)
        {
            textControl.text = "-" + textToDisplay;
            textControl.color = Color.red;
        } else if (type == Enums.FloatingTextType.Heal)
        {
            textControl.text = "+" + textToDisplay;
            textControl.color = Color.cyan;
        } else
        {
            textControl.text = textToDisplay;
            textControl.color = Color.white;
        }
	}
}
