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
    public GameObject frameObj;
    Dictionary<Vector2, bool> mapSlot;
    Dictionary<Vector2, int[]> slotDict;
    string[,] currentMap;
    string[,] correctMap;

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
        // terminate piece gen if no more
        if (!allEnabled() && isStart)
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
    bool allEnabled()
    {
        foreach (GameObject obj in pieceObjs)
        {
            if (!(obj.GetComponent<PieceController>().isEnabled())) return false;
        }
        return true;
    }

    // game methods
    void restartGame()
    {
        // get controllers
        PieceFactoryController fac = pieceFactoryObj.GetComponent<PieceFactoryController>();
        PlayerController player = playerObj.GetComponent<PlayerController>();

        // get list of picture pieces to be placed
        pieceObjs = fac.createPieceList();

        // get map
        int[,] intMap = fac.getCorrectMap4();
        string picRef = fac.getPicRef();
        correctMap = new string[4,4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                correctMap[i, j] = picRef + intMap[i, j].ToString();
            }
        }

        // set up map slots
        setupMap4();

        // timer stuff
        releaseTime = intervalTime;
        isStart = true;
    }
    public void checkSlotSnapping(PieceController piece)
    {        
        foreach (KeyValuePair<Vector2, bool> entry in mapSlot)
        {
            // distance to each slotted point
            if (entry.Value)
            {
                if (Vector2.Distance(entry.Key, piece.getMyPos()) >= 0.4)
                {
                    // remove slot entry
                    mapSlot[entry.Key] = false;
                    unfillCurrentMap(entry.Key);
                }
            }
            // distance to each unslotted slot point
            else if (Vector2.Distance(entry.Key, piece.getMyPos()) <= 0.4)
            {
                // snap!
                piece.setLocation(entry.Key);
                // fill up slot
                mapSlot[entry.Key] = true;
                fillCurrentMap(entry.Key, piece.getName());
            }
        }
        Debug.Log(currentMap.ToString());
    }
    void fillCurrentMap(Vector2 slotPos, string content)
    {
        int[] matrixCoord = slotDict[slotPos];
        currentMap[matrixCoord[0], matrixCoord[1]] = content;
    }
    void unfillCurrentMap(Vector2 slotPos)
    {
        int[] matrixCoord = slotDict[slotPos];
        currentMap[matrixCoord[0], matrixCoord[1]] = "NA";
    }
    void setupMap4()
    {
        // create slot positions & current map
        Vector2 framePos = frameObj.transform.position;
        Vector2 basePos = new Vector2(framePos.x - 1.5f, framePos.y + 1.5f);
        List<Vector2> slots = new List<Vector2>();
        currentMap = new string[4,4];
        slotDict = new Dictionary<Vector2, int[]>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 slotPos = new Vector2(basePos.x + 1.0f * i, basePos.y - 1.0f * j);
                slots.Add(slotPos);
                currentMap[i, j] = "NA";
                slotDict.Add(slotPos, new int[] {i, j});
            }
        }
        // create slot dictionary to check snap/unsnap
        mapSlot = new Dictionary<Vector2, bool>();
        foreach (Vector2 slot in slots)
        {
            mapSlot.Add(slot, false);
        }
    }
    bool isPictureFinished()
    {
        // compare current and correct map
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (!(currentMap[i,j].Equals(correctMap[i,j]))) return false;
            }
        }
        return true;
    }
}
