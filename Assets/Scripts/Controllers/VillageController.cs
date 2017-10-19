using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VillageController : MonoBehaviour
{
	public GameObject difficultyManager;
	public Sprite[] sprites;
	public AudioClip explosion;
	public bool isDestroyed;
	public bool godMode;
	
	private SpriteRenderer spriteRenderer;
	private int latestSeconds;
	private int growthCount;
	private float timer;

	// Use this for initialization
	private void Start()
	{
		//this is what makes it so that the sprite can be changed in run time not via animation, can be swapped
		//to animation later on
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

		growthCount = 0;
		UpdateCollider();

		if (Debug.isDebugBuild)
			Debug.Log("Village Created Succesfully");
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
	}

	/**
	   * This method increases the size of the village sprite by .2 lengthwise for the first 20 seconds, and then
	   * .2 in height for the next 50, after this the village will not grow anymore. This can be altered later on 
	   * by simply changing the vector values and the amount of time in the update() method for this script
	 * 
	 * NOTE: This function is called within the difficulty manager controller to make sure it is called 100% of the time
	 * WILL NEED TO FIGURE OUT WHY IT WON'T CALL WHEN IN THE UPDATES 100% OF THE TIME
	   * */
	public bool IncreaseSize()
	{
		if (Debug.isDebugBuild)
			Debug.Log("Growed?");
		
		if (growthCount < 2)
		{
			if (Debug.isDebugBuild)
				Debug.Log("Village growth triggered!");
			
			spriteRenderer.sprite = sprites[growthCount + 1];
			UpdateCollider();
			return true;
		}
		
		growthCount++;
		return false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Meteorite"))
		{
			if (!godMode)
			{
				MeteoriteController meteorite = collision.gameObject.GetComponent<MeteoriteController>();
				meteorite.BlowUp();
				PlayExplosion();
				isDestroyed = true;
				Invoke("ChangeScene", 0.55f);
			}
		}
	}

	/**
	 * Updates the BoxCollider2D offset and size
	 * depending on the sprite's bounds set
	 * in the sprite editor.
	 */
	private void PlayExplosion()
	{
		AudioSource.PlayClipAtPoint(explosion, transform.position);
	}

	private void ChangeScene()
	{
		if (Debug.isDebugBuild)
			Debug.Log("invoked woo");
		
		gameObject.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadScene("GameEnd");
	}

	/**
	 * Updates the BoxCollider2D offset and size
	 * depending on the sprite's bounds set
	 * in the sprite editor.
	 */
	private void UpdateCollider()
	{
		BoxCollider2D collider = GetComponent<BoxCollider2D>();

		// Gets the size and center of the sprite and applies it to the collider
		collider.size = spriteRenderer.sprite.bounds.size;
		collider.offset = spriteRenderer.sprite.bounds.center;
	}
}