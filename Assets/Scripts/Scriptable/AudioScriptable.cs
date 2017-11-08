using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioScript", menuName = "Scriptable/Audio", order = 4)]
public class AudioScriptable : ScriptableObject 
{
	#region Variable
	public List<MusicFX> AllMF;
	#endregion
	
	#region Mono

	#endregion
		
	#region Public
	#endregion
	
	#region Private
	#endregion
}


[System.Serializable]
public class MusicFX
{
	public AudioType ThisType;
	public List<AllAudio> SetAudio;
}

[System.Serializable]
public class AllAudio
{
	public string AudioName;
	public AudioClip Audio;
}