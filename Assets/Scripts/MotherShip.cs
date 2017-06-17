using SocketIO;
using UnityEngine;

public class MotherShip : MonoBehaviour 
{
    public SocketIOComponent socket;

    public int collectedEnergy = 0;
	public int neededEnergy;

	public GameObject[] energy;
	public int totalEnergy;

	public float difficultyPercentage = 0.5f;
	
	private PlayerInventory playerInventory;

	private Animator anim;

	public float restartDelay = 5f;
	private float restartTimer;
	
	void Awake()
	{
		playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
		energy = GameObject.FindGameObjectsWithTag("Energy");
		totalEnergy = energy.Length;
		neededEnergy = Mathf.RoundToInt (totalEnergy * difficultyPercentage);
		anim = GameObject.Find ("HUDCanvas").GetComponent<Animator>();
	}

	void Update()
	{
		if(totalEnergy < neededEnergy || collectedEnergy >= neededEnergy)
		{
			//print ("Game Over!");
			anim.SetTrigger("IsGameOver");

            socket.Emit("gameover");

			restartTimer+= Time.deltaTime;

			if(restartTimer >= restartDelay)
			{
				Application.LoadLevel(Application.loadedLevel);
			}

		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(playerInventory.collectedEnergy != 0)
			{
                var collected = playerInventory.collectedEnergy;

                collectedEnergy += collected;
				playerInventory.collectedEnergy = 0;

                socket.Emit("mothership", new JSONObject(string.Format(@"{{""q"":{0}}}", collected)));
            }
		}
	}
}
