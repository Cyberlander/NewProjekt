using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour 
{	
	[SerializeField]
	private ObjectPool op;

	[SerializeField]
	private GameObject[] enemyTypes;

	[SerializeField]
	private GameObject player;

	private Wave currentWave;
	private GameObject[] waveEnemys;

	void Update()
	{
		if (currentWave == null) 
		{	
			waveEnemys = new GameObject[1];
			waveEnemys[0] = enemyTypes[0];
			currentWave = new Wave (op, waveEnemys, new int[] {1}, 1, player);
		}
		else
		{
			if (!currentWave.IsWaveActive()) 
			{
					switch(currentWave.GetWaveNr())
					{
						case 1:	
						
							currentWave = new Wave (op, waveEnemys, new int[] {3}, 2, player); 
							break;
						case 2:
							currentWave = new Wave (op, waveEnemys, new int[] {6}, 3, player); 
							break;
						case 3:
							waveEnemys = new GameObject[1];
							waveEnemys[0] = enemyTypes[1];
							currentWave = new Wave (op, waveEnemys, new int[] {1}, 4, player);
							break;
						case 4:
							currentWave = new Wave (op, waveEnemys, new int[] {3}, 5, player); 
							break;
						default:
							Debug.Log ("THIS WAS LAST WAVE. STOP PLAY NOW!");
							break;
					}
			
				
			}
		}
}

public class Wave
{
	private ObjectPool op;
	private GameObject[] enemyTypes;
	private GameObject player;
	private int[] enemyCount;
	private int waveNr;
	private bool isActive;

	public Wave(ObjectPool o, GameObject[] enemys, int[] enemyAmount, int waveNo, GameObject p)
	{
		op = o;
		enemyTypes = enemys;
		enemyCount = enemyAmount;
		waveNr = waveNo;
		player = p;
		isActive = true;
		SpawnWave ();
	}

	public bool IsWaveActive()
	{
		if (op.GetSpawnedObjectCount () > 0)
				return true;
		else if (isActive) 
		{
			player.GetComponent<PlayerControllerScript> ().Talk ();
			isActive = false;
			return true;
		} 
		else if (!isActive && !player.GetComponent<PlayerControllerScript> ().IsTalking ()) 
			return false;
		else
			return true;

	}

	public int GetWaveNr()
	{
		return waveNr;
	}

	private void SpawnWave()
	{
		for (int i = 0; i < enemyTypes.Length; i++)
		{
			for(int n = 1; n <= enemyCount[i]; n++)
			{	
				op.Spawn(getSpawnPos(), enemyTypes[i]);
			}
		}
	}

	private Vector3 getSpawnPos()
	{
		Vector3 pos = new Vector3 (Random.Range (-9, 9), Random.Range (-4.85f, 4.85f), 0);

		while (Vector3.Distance(pos, player.transform.position) < 4) 
		{
			pos = new Vector3 (Random.Range (-9, 9), Random.Range (-4.85f, 4.85f), 0);
		}
		return pos;	
	}


	}
}