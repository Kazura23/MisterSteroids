using UnityEngine;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class ImageFilter : MonoBehaviour 
{
	#region Variable
	public Material material;
	#endregion
	
	#region Mono
	void OnRenderImage ( RenderTexture source, RenderTexture destination ) 
	{
		Graphics.Blit (source, destination, material);
	}
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	#endregion
}
