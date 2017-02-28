using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumberController : MonoBehaviour {

    public float moveSpeed;
    public int displayNumber;
    public Enums.FloatingNumberType type;
    public Text textControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), transform.position.z);
        if (type == Enums.FloatingNumberType.Damage)
        {
            textControl.text = "-" + displayNumber.ToString();
            textControl.color = Color.red;
        } else if (type == Enums.FloatingNumberType.Heal)
        {
            textControl.text = "+" + displayNumber.ToString();
            textControl.color = Color.cyan;
        }        
	}
}
