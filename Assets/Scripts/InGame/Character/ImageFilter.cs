using UnityEngine;

[ImageEffectAllowedInSceneView]
public class ImageFilter : MonoBehaviour 
{
	#region Variable
	public Material SpeedEffect;
	#endregion
	
	#region Mono
	void OnRenderImage ( RenderTexture source, RenderTexture destination ) 
	{
		Graphics.Blit (source, destination, SpeedEffect);
	}
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	#endregion
}
