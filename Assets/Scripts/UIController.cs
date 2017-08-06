using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class UIController : MonoBehaviour {
    //References to UI
    Image characterImage;
    Image tipImage;
    Text tipText;
    InputField answerInput;
    Text solutionCheck;
    Text correctSolution;
    GameObject solution;
    //Source Image
    public Sprite[] hiriganaSprite;
    public List<AudioClip> aClips;
    AudioSource aSource;

    string userSolution;
    Dictionary<string, hiriganaCharacter> dictionary;
    List<string> keys;
    int currentIndex;
    //List<hiriganaCharacter> dictionary;

	// Use this for initialization
	void Start () {
        characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
        tipImage = GameObject.Find("TipImage").GetComponent<Image>();
        answerInput = GameObject.Find("Answer").GetComponent<InputField>();
        solution = GameObject.Find("Solution");
        solutionCheck = solution.GetComponentsInChildren<Text>()[0];
        correctSolution = solution.GetComponentsInChildren<Text>()[1];
        tipText = GameObject.Find("TipText").GetComponent<Text>();

        aSource = GetComponent<AudioSource>();

        setupDictionary();

        solution.SetActive(false);
        updateUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void submitButton()
    {
        //Debug.Log("Submit Button");     
        userSolution = answerInput.text;
        userSolution = userSolution.ToLower();

        if (userSolution.Equals(dictionary[keys[currentIndex]].getSolution()))
            solutionCheck.text = "Correct!";
        else
            solutionCheck.text = "Incorrect!";
        correctSolution.text = dictionary[keys[currentIndex]].getSolution();
        solution.SetActive(true);
    }
    public void nextButton()
    {
        solution.SetActive(false);
        currentIndex++;
        if (currentIndex > keys.Count)
        {
            currentIndex = 0;
        }
        updateUI();
    }
    public void playAudioButton()
    {
        aSource.PlayOneShot(dictionary[keys[currentIndex]].getAClip());
    }

    void setupDictionary()
    {
        string path = "Assets/SolutionSheet.txt";
        StreamReader reader = new StreamReader(path);

        dictionary = new Dictionary<string, hiriganaCharacter>();
        
        for (int x = 0; x < hiriganaSprite.Length-1; x+=2) {
            //dictionary.Add(new hiriganaCharacter(hiriganaSprite[x], hiriganaSprite[x+1], reader.ReadLine(), reader.ReadLine()));
            string key = reader.ReadLine();
            Debug.Log(key);
            string tipText = reader.ReadLine();
            dictionary.Add(key, new hiriganaCharacter(hiriganaSprite[x], hiriganaSprite[x + 1], key, tipText));
        }
        reader.Close();
        for (int x = 0; x < aClips.Count; x++)
        {
            if(!dictionary.ContainsKey(aClips[x].name))
                Debug.Log(aClips[x].name);
            dictionary[aClips[x].name].addAClip(aClips[x]);
        }

        keys = new List<string>(dictionary.Keys);
        currentIndex = 0;
    }

    void updateUI()
    {
        characterImage.sprite = dictionary[keys[currentIndex]].getCharacter();
        tipImage.sprite = dictionary[keys[currentIndex]].getTip();
        tipText.text = dictionary[keys[currentIndex]].getTipText();
        answerInput.text = "";
    }
}

public class hiriganaCharacter{
    Sprite characteImage;
    Sprite tipImage;
    string solution;
    string tipText;
    AudioClip aClip;

    public hiriganaCharacter(Sprite _characterImage, Sprite _tipImage, string _solution, string _tipText)
    {
        characteImage = _characterImage;
        tipImage = _tipImage;
        solution = _solution;
        tipText = _tipText;      
    }
    public void addAClip(AudioClip _aClip)
    {
        aClip = _aClip;
    }

    public Sprite getCharacter()
    {
        return characteImage;
    }
    public Sprite getTip()
    {
        return tipImage;
    }
    public string getSolution()
    {
        return solution;
    }
    public string getTipText()
    {
        return tipText;
    }
    public AudioClip getAClip()
    {
        return aClip;
    }
}
