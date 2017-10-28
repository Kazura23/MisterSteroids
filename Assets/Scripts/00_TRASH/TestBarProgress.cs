using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBarProgress : MonoBehaviour {

    public Texture2D textEmpty, textProgress;
    public float heightMax = 25, widthMax = 200;
    public float delay = 10;
    private float timer;
    public Vector2 pos;
    private Rect empty, progress;
    private bool active;

	// Use this for initialization
	void Start () {
        SaveData.LoadData();
        timer = 0;
        empty = new Rect(pos.x, pos.y, widthMax, heightMax);
        progress = new Rect(pos.x, pos.y, 0.01f, heightMax);
        Debug.Log("nb piece = "+AllPlayerPrefs.piece);
	}

    private void OnApplicationQuit()
    {
        SaveData.SaveDataS();
    }

    // Update is called once per frame
    void Update () {
        if (!active)
        {
            active = true;
        }
		if(timer < delay)
        {
            timer += Time.deltaTime;
            if(timer > delay)
            {
                timer = delay;
            }
            progress.width = ((timer / delay)) * widthMax;
        }
	}

    private void OnGUI()
    {
        if (active)
        {
            GUI.DrawTexture(empty, textEmpty);
            GUI.DrawTexture(progress, textProgress);
        }
    }

}
