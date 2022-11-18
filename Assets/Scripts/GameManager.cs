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
    public int wrongGuesses = 0;

    private bool drawingComplete = false; //if drawing is complete, player lose
    private bool wordComplete = false; //if word is complete, player wins

    public void NewGame() //starts a new game, resets everything from last game
    {
        wrongGuesses = 0; //number of wrong guess now 0
        drawingComplete = false; //drawing is not complete
        wordComplete = false; //word is not complete

        chosenWord.Clear(); //clears the chosen word, so it is set as new random word
        wordBoardCharacters.Clear(); //clears the word board, so it is set with new number of characters

        foreach (Transform child in wordBoard) //destroys all character holders in the word board panel
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < keyBoardKeys.Length; i++) //re enables interactability of all keyboard keys
        {
            keyBoardKeys[i].interactable = true;
        }

        for (int i = 0; i < drawings.Length; i++) //hides all drawing sections
        {
            drawings[i].SetActive(false);
        }

        StartCoroutine(ChooseRandomWord()); //chooses a new word for every new game
    }

    IEnumerator ChooseRandomWord()
    {
        int index = Random.Range(0, wordList.Length); //chooses a random number from 0 to length of wordlist array

        string chooseWord = wordList[index]; //chooses the word which corresponds to that index value

        fullWord = chooseWord; //sets the completed/full word as the random word

        string[] characters = new string[chooseWord.Length]; //creates a new string which is the same length as chooseword word count

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
            if (keyBoardKeys[i].name == letterGuess) //checks all keyboard keys if its name is the same as the letter guess
            {
                keyBoardKeys[i].interactable = false; //then disables its interaction
            }
        }

        StartCoroutine(CheckLetter(letterGuess)); //checks if the guessed letter is within the chosen word
    }

    public void InputWord()
    {
        string word = guessWord.text; //sets the word from the input field to a string variable

        StartCoroutine(CheckWord(word)); //checks if the guessed word == the random word
    }

    IEnumerator CheckLetter(string letter)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < chosenWord.Count; i++) //checks each individual character in chosen word if is the same as guessed letter
        {
            if (chosenWord[i] == letter)
            {
                wordBoardCharacters[i].text = letter; //if the guessed letter is in the random word, then it shows up on the word board.
            }
        }

        if (chosenWord.Contains(letter)) //checks if the word board has the guessed letter
        {

        }
        else
        {
            wrongGuesses++; //if not, increases number of wrong guesses
            drawings[wrongGuesses - 1].SetActive(true); //then shows a drawing section corresponding to wrong guess number.
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator CheckWord(string word)
    {
        if (word == fullWord)
        {

        }
        else
        {

        }

        yield return null;
    }


}
