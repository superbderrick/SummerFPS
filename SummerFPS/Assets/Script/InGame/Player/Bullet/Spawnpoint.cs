
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
	[SerializeField] GameObject graphics;

	void Awake()
	{
		graphics.SetActive(false);
	}
}
