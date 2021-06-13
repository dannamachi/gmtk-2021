using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFactoryController : MonoBehaviour
{
    // Pic List
    public List<Sprite> citybros = new List<Sprite>();
    public List<Sprite> evergreens = new List<Sprite>();
    public List<Sprite> gayhearts = new List<Sprite>();
    public List<Sprite> foxrest = new List<Sprite>();
    public List<Sprite> lifegony = new List<Sprite>();
    public List<Sprite> braziliant = new List<Sprite>();
    public List<Sprite> reddersky = new List<Sprite>();
    public Sprite citybro;
    public Sprite evergreen;
    public Sprite gayheart;
    public Sprite foxres;
    public Sprite lifegon;
    public Sprite brazilia;
    public Sprite redder;
    string picRef;
    // Piece Ref
    public GameObject piecePrefab;

    // Correct map
    public int[,] getCorrectMap4()
    {
        int[,] corrMap = new int[4,4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                corrMap[i, j] = j * 4 + i;
            }
        }
        return corrMap;
    }
    public string getPicRef()
    {
        return picRef;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // get display piece
    public Sprite getDisplayPiece()
    {
        if (picRef.Equals("CityBro")) return citybro;
        else if (picRef.Equals("EverGreen")) return evergreen;
        else if (picRef.Equals("GayHeart")) return gayheart;
        else if (picRef.Equals("Foxrest")) return foxres;
        else if (picRef.Equals("Lifegony")) return lifegon;
        else if (picRef.Equals("Braziliant")) return brazilia;
        else if (picRef.Equals("RedderSky")) return redder;
        return null;
    }

    // create pieces
    public List<GameObject> createPieceList()
    {
        // select random image
        List<GameObject> pieces = new List<GameObject>();
        List<Sprite> spriteList = new List<Sprite>();
        int ran = Random.Range(0, 7);
        string titlePiece = "NA";
        switch(ran)
        {
            case 0:
                titlePiece = "CityBro";
                spriteList = citybros;
                break;
            case 1:
                titlePiece = "EverGreen";
                spriteList = evergreens;
                break;
            case 2:
                titlePiece = "GayHeart";
                spriteList = gayhearts;
                break;
            case 3:
                titlePiece = "Foxrest";
                spriteList = foxrest;
                break;
            case 4:
                titlePiece = "Lifegony";
                spriteList = lifegony;
                break;
            case 5:
                titlePiece = "Braziliant";
                spriteList = braziliant;
                break;
            case 6:
                titlePiece = "RedderSky";
                spriteList = reddersky;
                break;
        }
        picRef = titlePiece;
        // create pieces and connections
        for (int i = 0; i < spriteList.Capacity; i++)
            {
                // instantiate w sprite
                GameObject gam = Instantiate(piecePrefab, new Vector2(), Quaternion.identity);
                PieceController piece = gam.GetComponent<PieceController>();
                piece.changeSprite(spriteList[i], titlePiece + i.ToString());
                // add to list
                pieces.Add(gam);
            }        
        
        return pieces;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
