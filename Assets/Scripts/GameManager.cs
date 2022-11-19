using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //allows me to get the name of the button pressed, rather than hardcoding each button to print their respective letter
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public MenuHandler menuHandler;

    public string[] wordList; //10 random words I've come up with, the computer chooses 1 of these for each game
    public List<string> chosenWord = new List<string>(); //a list of string/chars of the word's individual characters
    public string[] correctLetters; //an array of string/chars of every correct guess
    public string fullWord; //the full unsplit chosen word, used for word guesses

    public GameObject characterHolder; //the prefab for holding the character
    public Transform wordBoard; //the panel which holds the prefab ^^

    public List<Text> wordBoardCharacters = new List<Text>(); //a list of text prefabs which corresponds to word length

    public GameObject[] drawings; //each line and head of the hangman drawing
    public Button[] keyBoardKeys; //the keys in the game panel. Just so i can put them into a for loop and disable their interactivity whenever the player chooses a letter, and if that letter has been used already.

    public InputField guessWord; //the input field for if the player can guess the word with the guesses they've made.
    public Text endText; //the text on the play again panel, tells player if they've won or lost the game, then asks if they'll play again
    public Button yes, no; //will enable interaction on game end.

    private int wrongGuesses; //the number of wrong guesses the player has made

    private bool drawingComplete = false; //if drawing is complete, player lose
    private bool wordComplete = false; //if word is complete, player wins
    #endregion

    #region NewGame
    public void NewGame() //starts a new game, resets everything from last game
    {
        wrongGuesses = 0; //number of wrong guess reset to 0
        drawingComplete = false; //drawing is not complete
        wordComplete = false; //word is not complete

        yes.interactable = false; //sets play again button as uninteractable
        no.interactable = false; //set quit button as uninteractable

        guessWord.text = ""; //sets input field empty

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
    #endregion

    #region Choose Random Word from wordlist array
    IEnumerator ChooseRandomWord()
    {
        int index = Random.Range(0, wordList.Length); //chooses a random number from 0 to length of wordlist array

        string chooseWord = wordList[index]; //chooses the word which corresponds to that index value

        fullWord = chooseWord; //sets the completed/full word as the random word

        correctLetters = new string[fullWord.Length]; //sets the length of the array to be as long as the chosen word's length

        string[] characters = new string[chooseWord.Length]; //creates a new string which is the same length as chooseword word length

        for (int i = 0; i < chooseWord.Length; i++) //for every every character inside the word
        {
            characters[i] = chooseWord[i].ToString(); //splits the random word into individual characters
            chosenWord.Add(characters[i]); //adds the splitted word into the list
        }

        for (int i = 0; i < chosenWord.Count; i++) //for every character in the list
        {
            GameObject temp = Instantiate(characterHolder, wordBoard, false); //sets a variable which instantiates the characterHolder prefab into the wordBoard panel
            wordBoardCharacters.Add(temp.GetComponent<Text>()); //instantiates the prefab for each letter in the word ( 3 letters = 3 prefabs )
        }

        yield return new WaitForSeconds(2f);
    }
    #endregion

    #region Keyboard button press
    public void InputButton() //executes code when button is pressed
    {
        string letterGuess = EventSystem.current.currentSelectedGameObject.name; //name of button pressed = guessed letter

        for (int i = 0; i < keyBoardKeys.Length; i++) //for every keyboard key in array
        {
            if (keyBoardKeys[i].name == letterGuess) //checks all keyboard keys if its name is the same as the letter guess
            {
                keyBoardKeys[i].interactable = false; //then disables its interaction
            }
        }

        StartCoroutine(CheckLetter(letterGuess)); //checks if the guessed letter is within the chosen word
    }
    #endregion

    #region Guess Word
    public void InputWord()
    {
        string word = guessWord.text; //sets the word from the input field to a string variable

        StartCoroutine(CheckWord(word)); //checks if the guessed word == the random word
    }
    #endregion

    #region Check if letter is inside the random word
    IEnumerator CheckLetter(string letter)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < chosenWord.Count; i++) //checks each individual character in chosen word if is the same as guessed letter
        {
            if (chosenWord[i] == letter)
            {
                wordBoardCharacters[i].text = letter; //if the guessed letter is in the random word, then it shows up on the word board.
                correctLetters[i] = letter; //adds guesses letter to correct letters list
            }
        }

        if (chosenWord.Contains(letter)) { } //checks if the word board has the guessed letter        
        else
        {
            wrongGuesses++; //if not, increases number of wrong guesses
            drawings[wrongGuesses - 1].SetActive(true); //then shows a drawing section corresponding to wrong guess number.
            StartCoroutine(CheckDrawing()); //check if drawing is complete 
        }

        PlayerWin(); //checks if the player has won

        if (PlayerWin()) //if player has won
        {
            wordComplete = true; //sets word complete bool to true = which sets play again panel to say "you have won the game"

            StartCoroutine(EndGame()); //starts the end game process
        }

        yield return new WaitForSeconds(1f);
    }
    #endregion

    #region Check if the word is complete
    bool PlayerWin()
    {
        for (int i = 0; i < chosenWord.Count; i++) //for every letter in chosen word,...
        {
            if (chosenWord[i] != correctLetters[i]) //check if letter is not in the correct guesses list
            {
                return false; //if letter is not there, returns false
            }
        }

        return true; //otherwise, returns true
    }
    #endregion

    #region Check if guessed word is the chosen word
    IEnumerator CheckWord(string word)
    {
        if (word == fullWord) //checks if the input word is the same as the chosen word
        {
            wordComplete = true; //if true, sets word complete bool to true = which sets play again panel to say "you won the game"

            for (int i = 0; i < chosenWord.Count; i++) //for every letter in the chosen word,...
            {
                correctLetters[i] = chosenWord[i]; //sets all possible guesses as that letter
            }

            for (int i = 0; i < correctLetters.Length; i++) //for every index inside the checking word list,...
            {
                wordBoardCharacters[i].text = correctLetters[i]; //turns all character holders in the word board into the input word's characters 
                                                                 //shows the input word on the word board
            }

            yield return new WaitForSeconds(2f);

            StartCoroutine(EndGame()); //starts the end game process
        }
        else //if the input word is NOT the chosen word
        {
            guessWord.text = ""; //the input field becomes blank

            wrongGuesses++; //increases the wrong guesses by 1
            drawings[wrongGuesses - 1].SetActive(true); //draws the next section of the hangman

            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region Check if drawing is complete
    IEnumerator CheckDrawing()
    {
        if (wrongGuesses == 10) //checks if 10 wrong guesses have been made (all drawings should have shown up by this point)
        {
            drawingComplete = true; //sets drawing complete bool to true = which sets play again panel title to "you lost the game"

            StartCoroutine(EndGame()); //executes the end game coroutine
        }

        yield return null;
    }
    #endregion

    #region Play Again
    IEnumerator EndGame() //whether the player wins or loses, this is executed
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < drawings.Length; i++)
        {
            drawings[i].SetActive(false);
        }

        menuHandler.menuHandlerInstance.ChangePanel(3); //changes panel to the play again menu, where player can choose to play again, or quit.

        if (drawingComplete)
        {
            endText.text = "Game Over:\nYou lost the game!";
        }
        else if (wordComplete)
        {
            endText.text = "Game Over:\nYou won the game!";
        }

        yield return new WaitForSeconds(2f);

        endText.text = "Will you\nplay again?";

        yes.interactable = true;
        no.interactable = true;

        menuHandler.menuHandlerInstance.gameStates = GameStates.MenuState; //sets game state to menu state = stops all game processes
        menuHandler.menuHandlerInstance.NextState(); //activates the game state change
    }
    #endregion
}
