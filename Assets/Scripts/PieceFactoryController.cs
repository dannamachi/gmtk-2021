using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFactoryController : MonoBehaviour
{
    // Pic List
    public List<Sprite> citybros = new List<Sprite>();
    public List<Sprite> evergreens = new List<Sprite>();
    public List<Sprite> gayhearts = new List<Sprite>();
    // Piece Ref
    public GameObject piecePrefab;

    // Connection matrix lmao
    int getConn4(int pos, string dirKey)
    {
        if (dirKey.Equals("trueLeft"))
        {
            if (pos % 4 == 0) return -1;
            return pos - 1;
        }
        else if (dirKey.Equals("trueRight"))
        {
            if (pos % 4 == 3) return -1;
            return pos + 1;
        }
        else if (dirKey.Equals("trueUp"))
        {
            if (pos / 4 == 0) return -1;
            return pos - 4;
        }
        else if (dirKey.Equals("trueDown"))
        {
            if (pos / 4 == 3) return -1;
            return pos + 4;
        }
        return -2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // create pieces
    List<GameObject> createPieceList()
    {
        // select random image
        List<GameObject> pieces = new List<GameObject>();
        List<Sprite> spriteList = new List<Sprite>();
        int ran = Random.Range(0, 3);
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
        }
        
        // create pieces and connections
        for (int i = 0; i < spriteList.Capacity; i++)
            {
                // instantiate w sprite
                GameObject gam = Instantiate(piecePrefab, new Vector2(), Quaternion.identity);
                PieceController piece = gam.GetComponent<PieceController>();
                piece.changeSprite(citybros[i], titlePiece + i.ToString());
                // 4 directions
                if (getConn4(i, "trueLeft") > 0) piece.setTrueConn("trueLeft", titlePiece + getConn4(i, "trueLeft").ToString());
                if (getConn4(i, "trueRight") > 0) piece.setTrueConn("trueRight", titlePiece + getConn4(i, "trueRight").ToString());
                if (getConn4(i, "trueUp") > 0) piece.setTrueConn("trueUp", titlePiece + getConn4(i, "trueUp").ToString());
                if (getConn4(i, "trueDown") > 0) piece.setTrueConn("trueDown", titlePiece + getConn4(i, "trueDown").ToString());
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
