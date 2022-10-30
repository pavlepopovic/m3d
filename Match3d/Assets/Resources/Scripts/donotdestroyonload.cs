using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour
{
	public static DoNotDestroyOnLoad s_Instance = null;

	void Awake()
	{	
		if (s_Instance == null)		
		{
			s_Instance = gameObject.GetComponent<DoNotDestroyOnLoad>();
			DontDestroyOnLoad (gameObject);
		} 
		else
		{
			if (this != s_Instance)
			{
				Destroy (this.gameObject);
			}
		}
	}
}
