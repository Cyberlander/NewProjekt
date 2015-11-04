using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGeneratorScript : MonoBehaviour 
{	
	private const float MAP_WIDTH = 30;
	private const float MAP_HEIGHT = 20;

	[SerializeField]
	private GameObject _obstacle;
	[SerializeField]
	private GameObject _fence;
	[SerializeField]
	private GameObject _deco;
	[SerializeField]
	private float _minObstacleDistance;
	[SerializeField]
	private int _obstacleCount;
	[SerializeField]
	private int _decoCount;

	GameObject _obstacleContainer;
	GameObject _fenceContainer;
	List<GameObject> _existingObstacles;

	void Start () 
	{
		_obstacleContainer = new GameObject ("Obstacles");
		_fenceContainer = new GameObject ("Fence");
		_fenceContainer.transform.SetParent (gameObject.transform);
		_obstacleContainer.transform.SetParent (gameObject.transform);
		_existingObstacles = GenerateFence (_fence);
		_existingObstacles = PlaceObstacles (_obstacle, _obstacleCount, _existingObstacles);
		PlaceObstacles (_deco, _decoCount, _existingObstacles);
	}


	private List<GameObject> PlaceObstacles(GameObject obs, int count,List<GameObject> existingObstacles)
	{
		List<GameObject> obstacles = existingObstacles;

		for (int i = 0; i < count; i++) 
		{
			GameObject go = (GameObject) Instantiate(obs, generateObstaclePosition(obstacles), Quaternion.identity);
			go.transform.SetParent(_obstacleContainer.transform);
			obstacles.Add(go);
		}

		return obstacles;
	}

	private List<GameObject> GenerateFence(GameObject fence)
	{
		List<GameObject> completeFence = new List<GameObject> ();
		for (int i = 1; i < MAP_HEIGHT - 1; i+=2) 
		{
			GameObject f = (GameObject) Instantiate (fence, new Vector3(-MAP_WIDTH/2 + 1, -MAP_HEIGHT/2 + i, 0), Quaternion.identity);
			f.transform.SetParent(_fenceContainer.transform);
			completeFence.Add(f);
		}
		for (int i = 1; i < MAP_HEIGHT - 1; i+=2) 
		{
			GameObject f = (GameObject) Instantiate (fence, new Vector3(MAP_WIDTH/2 - 1, -MAP_HEIGHT/2 + i, 0), Quaternion.identity);
			f.transform.SetParent(_fenceContainer.transform);
			completeFence.Add(f);
		}
		for (int i = 1; i < MAP_WIDTH - 1; i+=2) 
		{
			GameObject f = (GameObject) Instantiate (fence, new Vector3(-MAP_WIDTH/2 + i,MAP_HEIGHT/2 - 1, 0), Quaternion.Euler(new Vector3(0,0,270)));
			f.transform.SetParent(_fenceContainer.transform);
			completeFence.Add(f);
		}
		for (int i = 1; i < MAP_WIDTH - 1; i+=2) 
		{
			GameObject f = (GameObject) Instantiate (fence, new Vector3(-MAP_WIDTH/2 + i,-MAP_HEIGHT/2 + 1, 0), Quaternion.Euler(new Vector3(0,0,270)));
			f.transform.SetParent(_fenceContainer.transform);
			completeFence.Add(f);
		}
		return completeFence;
	}


	private Vector3 generateObstaclePosition(List<GameObject> existingObstacles)
	{
		Vector3 pos = new Vector3 (
			Random.Range (-MAP_WIDTH / 2, MAP_WIDTH / 2),
			Random.Range (-MAP_HEIGHT / 2, MAP_HEIGHT / 2),
			0);
		foreach (GameObject o in existingObstacles) 
		{
			if(Vector2.Distance(pos, o.transform.position) < _minObstacleDistance)
				pos = generateObstaclePosition(existingObstacles);
		}


		return pos;
	}
}
