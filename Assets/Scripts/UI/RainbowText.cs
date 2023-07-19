using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RainbowText : MonoBehaviour
{

	//green yellow orange red purple blue cyan
	public List<Color> colors;
	public float lerpSpeed;
	private TMP_Text text;

	private Color currentColor;
	private Color targetColor;
	private float lerpTime;
	private int colorIndex;

	private void Start()
	{
		text = GetComponent<TMP_Text>();
		currentColor = colors[0];
		targetColor = colors[1];
		colorIndex = 2;
	}

	public void FixedUpdate()
	{
		if (lerpTime >= 1f)
		{
			currentColor = targetColor;
			targetColor = colors[colorIndex % colors.Count];
			colorIndex++;
			lerpTime = 0f;
		}
		Color lerpedColor = Color.Lerp(currentColor, targetColor, lerpTime);
		text.color = lerpedColor;

		lerpTime += Time.deltaTime * lerpSpeed;
	}

}
