using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceTag : MonoBehaviour {

	public Transform target;
	public Vector3 offset = new Vector3(0,0,-4.5f);
	private int price;
	private bool affordable;
	public TextMeshProUGUI label;
	public TMP_ColorGradient goldGradient;
	public TMP_ColorGradient orangeGradient;

	private void Update() {
		if(target == null || Headless.instance == null)
			return;

		if (affordable != IsAffordable()) {
			label.colorGradientPreset = IsAffordable() ? goldGradient : orangeGradient;
			affordable = IsAffordable();
		}
		
		UpdatePos();
	}

	private bool IsAffordable() {
		return Headless.instance.gold >= price;
	}
	
	public void SetValue(int price) {
		this.price = price;
		label.text = price.ToString();
	}

	public void UpdatePos() {
		offset.x = -0.05f * label.text.Length;
		GetComponent<RectTransform>().position = target.position + offset;
	}
}
