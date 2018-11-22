using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    public float speed = 0.1f;
    public Transform leftB;
    public Transform rightB;
    public Transform bottomB;
    public Transform topB;
    public GameObject foodPrefab;
    public GameObject tailPrefab;
    public Text countText;
    public Text cText;
    public Text highText;
    public AudioSource music;
    public AudioSource musicSFX;
    public GameObject panel;
    public GameObject panelGameover;
    public Slider sliderMusic;
    public Slider sliderSFX;
    public UnityEvent OnEat;

    private int score = 0;
    private Vector2 vec = Vector2.up;
    private Vector2 moveVec;
    private List<Transform> tail = new List<Transform>();
    private bool eat = false;
    private bool moveHorizontal = true;
    private bool moveVertical = false;
    //private float musicValue;
    private bool paused = false;
    private float diff;

	// Use this for initialization
	void Start () {
        GetVolume();
        InvokeRepeating("Movement", 0.3f, speed);
        Count();
        SpawnApple();
        highText.text = "High Score: " + PlayerPrefs.GetFloat("Highscore");
        //PlayerPrefs.SetFloat("Volume", music.volume);
        //PlayerPrefs.SetFloat("SFX", musicSFX.volume);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow) && moveHorizontal)
        {
            moveHorizontal = false;
            moveVertical = true;
            vec = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && moveVertical)
        {
            moveVertical = false;
            moveHorizontal = true;
            vec = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && moveVertical)
        {
            moveHorizontal = true;
            moveVertical = false;
            vec = -Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && moveHorizontal)
        {
            moveHorizontal = false;
            moveVertical = true;
            vec = -Vector2.right;
        }
        moveVec = vec / 3f;


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
            {
                Time.timeScale = 0;
                paused = true;
                panel.SetActive(true);
                music.Pause();
            }
            else
            {
                OnBack();
            }  
        }
    }

    void Movement()
    {
        Vector2 ta = transform.position;
        if (eat)
        {
            GameObject g = (GameObject)Instantiate(tailPrefab, ta, Quaternion.identity);
            tail.Insert(0, g.transform);
            eat = false;
        }
        else if (tail.Count > 0) {
            tail.Last().position = ta;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }

        transform.Translate(moveVec);
    }

    public void SpawnApple()
    {
        int x = (int)Random.Range(leftB.position.x, rightB.position.x);
        int y = (int)Random.Range(bottomB.position.y, topB.position.y);  

        foreach(var tails in tail)
        {
            if(tails.position.x == x || tails.position.y == y)
            {
                SpawnApple();
            }
        }

        Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
    }

    void Count()
    {
        countText.text = score.ToString();
        cText.text = "Score: " + score.ToString();
        if(PlayerPrefs.GetFloat("Highscore") < score)
        {
            PlayerPrefs.SetFloat("Highscore", score);
            highText.text = "High Score: " + ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
        }
    }

    /*public void OnReset()
    {
        sliderMusic.value = 0.8f;
        sliderSFX.value = 0.5f;
        music.volume = sliderMusic.value;
        musicSFX.volume = sliderSFX.value;
    }*/

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Replay()
    {
        Time.timeScale = 1;
        paused = false;
        //music.Play();
        //panelGameover.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void OnBack()
    {
        Time.timeScale = 1;
        paused = false;
        panel.SetActive(false);
        music.UnPause();

        music.volume = sliderMusic.value;
        musicSFX.volume = sliderSFX.value;
        PlayerPrefs.SetFloat("Volume", music.volume);
        PlayerPrefs.SetFloat("SFX", musicSFX.volume);
    }

    private void GetVolume()
    {
        music.volume = PlayerPrefs.GetFloat("Volume");
        //music.volume = musicValue;
        sliderMusic.value = music.volume;
        musicSFX.volume = PlayerPrefs.GetFloat("SFX");
        sliderSFX.value = musicSFX.volume;

        diff = PlayerPrefs.GetFloat("Difficulty");
        if(diff == 1)
        {
            speed = 0.05f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "food")
        {
            eat = true;
            Destroy(other.gameObject);
            score++;
            Count();
            SpawnApple();
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
            music.Stop();
            panelGameover.SetActive(true);
            //SceneManager.LoadScene(0);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(OnEat != null)
        {
            OnEat.Invoke();
        }
    }
}
