using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //allows me to get the name of the button pressed, rather than hardcoding each button to print their respective letter
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MenuHandler menuHandler;

    public string[] wordList; //10 random words I've come up with, the computer chooses 1 of these for each game
    public List<string> chosenWord = new List<string>();
    public string fullWord;

    public GameObject characterHolder; //the prefab for holding the character
    public Transform wordBoard; //the panel which holds the prefab ^^

    public List<Text> wordBoardCharacters = new List<Text>(); 

    public GameObject[] drawings; //each line and head of the hangman drawing
    public Button[] keyBoardKeys; //the keys in the game panel. Just so i can put them into a for loop and disable their interactivity whenever the player chooses a letter, and if that letter has been used already.

    public InputField guessWord; //the input field for if the player can guess the word with the guesses they've made.

    private int wordLength; //the number of characters in the chosen word
    private int wrongGuesses = 0;

    private bool drawingComplete = false; //if drawing is complete, player lose
    private bool wordComplete = false; //if word is complete, player wins

    public void NewGame() //starts a new game
    {
        wrongGuesses = 0;
        drawingComplete = false;
        wordComplete = false;

        chosenWord.Clear();
        wordBoardCharacters.Clear();

        foreach (Transform child in wordBoard)
        {
            GameObject.Destroy(child.gameObject);
        }

        StartCoroutine(ChooseRandomWord()); //chooses a new word for every new game
    }

    IEnumerator ChooseRandomWord()
    {
        int index = Random.Range(0, wordList.Length); //chooses a random number from 0 to length of wordlist array

        string chooseWord = wordList[index]; //chooses the word which corresponds to that index value

        fullWord = chooseWord; //sets the completed/full word as the random word

        string[] characters = new string[chooseWord.Length];

        for (int i = 0; i < chooseWord.Length; i++) 
        {
            characters[i] = chooseWord[i].ToString(); //splits the random word into individual characters
            chosenWord.Add(characters[i]); //adds the splitted word into the list
        }

        for (int i = 0; i < chosenWord.Count; i++)
        {
            GameObject temp = Instantiate(characterHolder, wordBoard, false); //instantiates the characterHolder prefab into the wordBoard panel
            wordBoardCharacters.Add(temp.GetComponent<Text>()); //instantiates the prefab for each letter in the word ( 3 letters = 3 prefabs )
        }

        yield return new WaitForSeconds(2f);
    }

    public void InputButton()
    {
        string letterGuess = EventSystem.current.currentSelectedGameObject.name; //name of button pressed = guessed letter

        for (int i = 0; i < keyBoardKeys.Length; i++)
        {
            if (keyBoardKeys[i].name == letterGuess)
            {
                keyBoardKeys[i].interactable = false;
            }
        }

        StartCoroutine(CheckLetter(letterGuess)); //checks if the guessed letter is within the chosen word
    }

    public void InputWord()
    {
        string word = guessWord.text;

        StartCoroutine(CheckWord(word));
    }

    IEnumerator CheckLetter(string letter)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < chosenWord.Count; i++) //checks each individual character in chosen word if is the same as guessed letter
        {
            if (chosenWord[i] == letter)
            {
                wordBoardCharacters[i].text = letter;
            }
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator CheckWord(string word)
    {
        if (word == fullWord)
        {
            //player wins
        }

        yield return null;
    }


}
