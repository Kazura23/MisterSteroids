using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainbowColor : MonoBehaviour
{
    public enum Type
    {
        Image,
        Material,
        MaterialFont,
        Sprite,
        Text,
    }

	int index = -1;
	public float time = .2f;
	public Color[] colors = new Color[]{ Color.red, Color.yellow, Color.green, Color.blue, Color.cyan };
    // Use this for initialization
    //public string rainbowType = "Image";
    public Type ComponentType;


    void OnEnable ()
	{
		Next ();
	}

	void Next ()
	{
		index = (index + 1) % colors.Length;

        if(ComponentType == Type.Image)
		    GetComponent<Image> ().DOColor (colors [index], time).OnComplete (() => Next ());

        if (ComponentType == Type.Material)
        {
            GetComponent<Renderer>().material.DOColor(colors[index], time).OnComplete(() => Next());
        }

        if (ComponentType == Type.MaterialFont)
            GetComponent<Font>().material.DOColor(colors[index], time).OnComplete(() => Next());

        if (ComponentType == Type.Sprite)
            GetComponent<SpriteRenderer>().DOColor(colors[index], time).OnComplete(() => Next());


        if (ComponentType == Type.Text)
            GetComponent<Text>().DOColor(colors[index], time).OnComplete(() => Next());

    }

    void OnDisable ()
	{
        if (ComponentType == Type.Image) { 
            GetComponent<Image> ().DOKill ();
		    GetComponent<Image> ().DOColor (Color.white, time * 2f);
        }

        if (ComponentType == Type.Material)
        {
            GetComponent<Material>().DOKill();
            GetComponent<Material>().DOColor(Color.white, time * 2f);
        }

        if (ComponentType == Type.MaterialFont)
        {
            GetComponent<Font>().material.DOKill();
            GetComponent<Font>().material.DOColor(Color.white, time * 2f);
        }

        if (ComponentType == Type.Sprite)
        {
            GetComponent<SpriteRenderer>().DOKill();
            GetComponent<SpriteRenderer>().DOColor(Color.white, time * 2f);
        }
        if (ComponentType == Type.Text)
        {
            GetComponent<Text>().DOKill();
            GetComponent<Text>().DOColor(Color.white, time * 2f);
        }

        
    }
}
