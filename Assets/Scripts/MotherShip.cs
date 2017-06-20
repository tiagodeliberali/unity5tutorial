using UnityEngine;
using UnitySocketIO;

public class MotherShip : MonoBehaviour 
{
    public SocketIOController socket;

    public int collectedEnergy = 0;
	public int neededEnergy;

	public GameObject[] energy;
	public int totalEnergy;

	public float difficultyPercentage = 0.5f;
	
	private PlayerInventory playerInventory;

	private Animator anim;
    private bool disconnected = false;

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
            if (!disconnected)
            {
                socket.Emit("gameover");
                socket.Close();

                disconnected = true;
            }

			//print ("Game Over!");
			anim.SetTrigger("IsGameOver");

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

                socket.Emit("mothership", string.Format(@"{{""EnergyCount"":{0}}}", collected));
            }
		}
	}
}
