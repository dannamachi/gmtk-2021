using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameController : MonoBehaviour
{
    // game variables
    public GameObject pieceFactoryObj;
    public GameObject playerObj;
    List<GameObject> pieceObjs;
    public GameObject timerTextObj;

    // game states
    bool isStart;

    // timer stuff
    float releaseTime;
    public float intervalTime = 5.0f;
    string timerText;

    // Start is called before the first frame update
    void Start()
    {
        isStart = false;
        // call when game start, this is for testing
        restartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            // piece generation timer
            timerTextObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Time: " + ((int) releaseTime).ToString();
            if (releaseTime > 0.0f)
            {
                releaseTime -= Time.deltaTime;
            }
            else
            {
                releaseTime = intervalTime;
                // display a new sprite, 4x4
                int newOther = Random.Range(0, 16);
                PieceController newPiece = pieceObjs[newOther].GetComponent<PieceController>();
                while (newPiece.isEnabled())
                {
                    newOther = Random.Range(0, 16);
                    newPiece = pieceObjs[newOther].GetComponent<PieceController>();
                }
                newPiece.enable();

                // randomize location on screen
                float newX = Random.Range(-4.0f, 4.0f);
                float newY;
                if (newX > 3.0f) newY = Random.Range(-3.0f, 4.0f);
                else newY = Random.Range(-4.0f, 4.0f);

                newPiece.setLocation(new Vector2(newX, newY));
            }
        }
        else 
        {
            timerTextObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Game Not Started";
        }
    }

    // game methods
    void restartGame()
    {
        // get controllers
        PieceFactoryController fac = pieceFactoryObj.GetComponent<PieceFactoryController>();
        PlayerController player = playerObj.GetComponent<PlayerController>();

        // get list of picture pieces to be placed
        pieceObjs = fac.createPieceList();

        // timer stuff
        releaseTime = intervalTime;
        isStart = true;
    }
}
