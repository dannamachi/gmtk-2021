using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGameController : MonoBehaviour
{
    // game variables
    public GameObject pieceFactoryObj;
    public GameObject playerObj;
    List<GameObject> pieceObjs = new List<GameObject>();
    public GameObject timerTextObj;
    public GameObject frameObj;
    public GameObject overlayObj;
    public GameObject safeZoneObj;

    // buttons & overlays
    public Sprite mainSp;
    public Sprite inSp;
    public Sprite winSp;
    public Sprite loseSp;
    public Sprite startSp;
    public Sprite restartSp;
    public Sprite exitSp;
    public GameObject button1Obj;
    public GameObject button2Obj;
    public GameObject picBG;

    // map stuff
    Dictionary<Vector2, bool> mapSlot;
    Dictionary<Vector2, int[]> slotDict;
    string[,] currentMap;
    string[,] correctMap;

    // game states
    bool isStart;
    bool isWin;
    string gameState;

    // game timer
    float gameTime;
    public float gameMaxTime = 300.0f;
    
    // bomb timer
    float bombTime;
    public float bombInterval = 15.0f;

    // piece timer stuff
    float releaseTime;
    public float intervalTime = 5.0f;
    string timerText;

    // Start is called before the first frame update
    void Start()
    {
        gameState = "MAINPAGE";
        isStart = false;
        // hide pic background
        picBG.GetComponent<SpriteRenderer>().enabled = false;
        // hide safe zone
        SafeZoneController szone = safeZoneObj.GetComponent<SafeZoneController>();
        szone.setDisplay(false);
        szone.setEnabled(false);
        // display main
        displayOverlay("Main");
        displayButton("Start");
        displayButton("Exit");
    }

    // display overlay
    void displayOverlay(string key)
    {
        SpriteRenderer ren = overlayObj.GetComponent<SpriteRenderer>();
        if (key.Equals("Success"))
        {
            ren.sprite = winSp;
            // add completed pic behind
            picBG.GetComponent<SpriteRenderer>().sprite = pieceFactoryObj.GetComponent<PieceFactoryController>().getDisplayPiece();
            picBG.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (key.Equals("GameOver")) ren.sprite = loseSp;
        else if (key.Equals("Main")) ren.sprite = mainSp;
        else if (key.Equals("Instruct")) ren.sprite = inSp;
        ren.enabled = true;
    }
    void hideOverlay()
    {
        SpriteRenderer ren = overlayObj.GetComponent<SpriteRenderer>();
        ren.enabled = false;
    }
    void displayButton(string key)
    {
        Image ren1 = button1Obj.GetComponent<Image>();
        Image ren2 = button2Obj.GetComponent<Image>();
        if (key.Equals("Start")) ren1.sprite = startSp;
        else if (key.Equals("Restart")) ren1.sprite = restartSp;
        else if (key.Equals("Exit")) ren2.sprite = exitSp;
        ren1.enabled = true;
        ren2.enabled = true;
    }
    void hideButtons()
    {
        Image ren1 = button1Obj.GetComponent<Image>();
        Image ren2 = button2Obj.GetComponent<Image>();
        ren1.enabled = false;
        ren2.enabled = false;
    }

    // button press
    public void onButton1Press()
    {
        // start button different
        if (gameState.Equals("MAINPAGE"))
        {
            gameState = "INSTRUCT";
            displayOverlay("Instruct");
        }
        else
        {
            // hide uis
            hideOverlay();
            hideButtons();
            picBG.GetComponent<SpriteRenderer>().enabled = false;
            // start/restart game
            gameState = "GAMEPLAY";
            destroyFinished();
            restartGame();
        }
    }
    public void onButton2Press()
    {
        // exit code
        // Debug.Log("Exit");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        // game state
        if (isStart)
        {
            timerTextObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Time left: " + ((int) gameTime).ToString();
            // check win
            isWin = isPictureFinished();
            
            // game timer
            if (gameTime > 0.0f && !isWin) gameTime -= Time.deltaTime;
            else
            {
                // game ends
                gameTime = gameMaxTime;
                isStart = false;
                // to end state
                gameState = "ENDGAME";
                if (isWin) displayOverlay("Success");
                else displayOverlay("GameOver");
                displayButton("Restart");
                displayButton("Exit");
            }
            
            // piece generation
            if (isStart && !allEnabled())
            {
                // piece generation timer
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
            
            // bomb generation
            if (isStart)
            {
                // give 2s of leeway
                if (bombTime > 0.0f)
                {
                    bombTime -= Time.deltaTime;
                    // warning sound & show safe zone
                    if (bombTime < bombInterval / 2)
                    {
                        SafeZoneController szone = safeZoneObj.GetComponent<SafeZoneController>();
                        szone.setDisplay(true);
                        szone.setEnabled(true);
                    }
                }
                else
                {
                    // check safe, otherwise explode!
                    PlayerController player = playerObj.GetComponent<PlayerController>();
                    if (!player.isSafe())
                    {
                        // play bomb ani
                        // mix up slotted pieces!
                        wreckMap(getFilledInSlots() / 2);
                    }
                    else 
                    {
                        // play safe sound
                    }
                    // hide safe zone
                    SafeZoneController szone = safeZoneObj.GetComponent<SafeZoneController>();
                    szone.setDisplay(false);
                    szone.setEnabled(false);
                    // prepare next random safe zone location (+boundary)
                    float newX = Random.Range(-3.0f, 3.0f);
                    float newY;
                    if (newX > 3.0f) newY = Random.Range(-2.0f, 3.0f);
                    else newY = Random.Range(-2.0f, 3.0f);
                    szone.setLocation(new Vector2(newX, newY));
                    // restart bomb timer
                    bombTime = bombInterval;
                    // reset safety
                    player.setSafe(false);
                }
            }
        }
        // end state
        else 
        {
            if (isWin) timerTextObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Finished!";
            else timerTextObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Time left: 0";
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

    // bomb methods
    int getFilledInSlots()
    {
        // count non NA slot
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (!(currentMap[i, j].Equals("NA"))) count++;
            }
        }
        return count;
    }
    void wreckMap(int num)
    {
        for (int i = 0; i < num; i++)
        {
            // get 2 random slots
            int rand1 = Random.Range(0, 16);
            int rand2 = Random.Range(0, 16);
            List<Vector2> slots = new List<Vector2>(mapSlot.Keys);
            Vector2 slot1 = slots[rand1];
            Vector2 slot2 = slots[rand2];
            // if not NA, swap them
            bool not1 = isPieceSlottedHere(slot1, "NA");
            bool not2 = isPieceSlottedHere(slot2, "NA");
            // filled slot 1, empty slot 2
            if (!not1 && not2)
            {
                // fill slot 2
                mapSlot[slot2] = true;
                fillCurrentMap(slot2, whatIsSlottedHere(slot1));
                // empty slot 1
                mapSlot[slot1] = false;
                unfillCurrentMap(slot1);
                // relocate piece
                getPieceWithName(whatIsSlottedHere(slot2)).setLocation(slot2);
            }
            // empty slot 1, filled slot 2
            else if (not1 && !not2)
            {
                // fill slot 1
                mapSlot[slot1] = true;
                fillCurrentMap(slot1, whatIsSlottedHere(slot2));
                // empty slot 2
                mapSlot[slot2] = false;
                unfillCurrentMap(slot2);
                // relocate piece
                getPieceWithName(whatIsSlottedHere(slot1)).setLocation(slot1);
            }
            // both slots fillted
            else if (!not1 && !not2)
            {
                // temp fill for slot 1
                string tempFill = whatIsSlottedHere(slot1);
                // fill slot 1
                fillCurrentMap(slot1, whatIsSlottedHere(slot2));
                // fill slot 2
                fillCurrentMap(slot2, tempFill);
                // relocate both pieces
                getPieceWithName(whatIsSlottedHere(slot1)).setLocation(slot1);
                getPieceWithName(whatIsSlottedHere(slot2)).setLocation(slot2);
            }
        }
    }
    PieceController getPieceWithName(string pname)
    {
        foreach (GameObject piece in pieceObjs)
        {
            if (piece.GetComponent<PieceController>().isCalled(pname)) return piece.GetComponent<PieceController>();
        }
        return null;
    }

    // game methods
    void destroyFinished()
    {
        foreach (GameObject obj in pieceObjs)
        {
            Destroy(obj);
        }
    }
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
        gameTime = gameMaxTime;
        bombTime = bombInterval;
        isStart = true;
        isWin = false;
    }
    public void postPickUpCheckSlot(PieceController piece)
    {
        List<Vector2> disSlots = new List<Vector2>();
        foreach (KeyValuePair<Vector2, bool> entry in mapSlot)
        {
            // distance to each slotted point
            if (entry.Value)
            {
                if (isPieceSlottedHere(entry.Key, piece.getName()))
                {
                    // remove slot entry
                    disSlots.Add(entry.Key);
                    unfillCurrentMap(entry.Key);
                    break;
                }
            }
        }
        // disable
        foreach (Vector2 vec in disSlots)
        {
            mapSlot[vec] = false;
        }
        // Debug.Log(debugDisplayMap(currentMap));
        // Debug.Log(debugDisplayMap(correctMap));
    }
    public void postPutDownCheckSlot(PieceController piece)
    {        
        List<Vector2> enSlots = new List<Vector2>();
        foreach (KeyValuePair<Vector2, bool> entry in mapSlot)
        {
            // distance to each unslotted slot point
            if (!entry.Value && Vector2.Distance(entry.Key, piece.getMyPos()) <= 0.4)
            {
                // snap!
                piece.setLocation(entry.Key);
                // fill up slot
                enSlots.Add(entry.Key);
                fillCurrentMap(entry.Key, piece.getName());
                break;
            }
        }
        // enable slots
        foreach (Vector2 vecen in enSlots)
        {
            mapSlot[vecen] = true;
        }
        // Debug.Log(debugDisplayMap(currentMap));
        // Debug.Log(debugDisplayMap(correctMap));
    }
    string debugDisplayMap(string[,] map)
    {
        string sth = "Map?:\n";
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                sth += i.ToString() + "-" + j.ToString() + ":" + map[i,j] + "\n";
            }
        }
        return sth;
    }
    bool isPieceSlottedHere(Vector2 slotKey, string pieceName)
    {
        int[] matrixCoord = slotDict[slotKey];
        return currentMap[matrixCoord[0], matrixCoord[1]] == pieceName;
    }
    string whatIsSlottedHere(Vector2 slotKey)
    {
        int[] matrixCoord = slotDict[slotKey];
        return currentMap[matrixCoord[0], matrixCoord[1]];
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
