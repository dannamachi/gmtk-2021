using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Connections
    string myName;
    Dictionary<string, string> connDict = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Check conn
    public bool isConnected(string key = "any") {
        if (!key.Equals("any"))
        {
            return connDict.ContainsKey(key);
        }
        else
        {
            return connDict.ContainsKey("left") || connDict.ContainsKey("right") || connDict.ContainsKey("up") || connDict.ContainsKey("down");
        }
    }
    public bool isPlacedCorrectly()
    {
        if (connDict.ContainsKey("trueLeft"))
        {
            if (connDict.ContainsKey("left"))
            {
                if (!connDict["trueLeft"].Equals(connDict["left"])) return false;
            } else return false;
                
        }
        if (connDict.ContainsKey("trueRight"))
        {
            if (connDict.ContainsKey("right"))
            {
                if (connDict["trueRight"].Equals(connDict["right"])) return false;
            } else return false;
        }
        if (connDict.ContainsKey("trueUp"))
        {
            if (connDict.ContainsKey("up"))
            {
                if (connDict["trueUp"].Equals(connDict["up"])) return false;
            } else return false;
        }
        if (connDict.ContainsKey("trueDown"))
        {
            if (connDict.ContainsKey("down"))
            {
                if (connDict["trueDown"].Equals(connDict["down"])) return false;
            } else return false;
        }
        return true;
    }

    // Set correct conn
    public void setTrueConn(string dirKey, string connName)
    {
        connDict.Add(dirKey, connName);
    }

    // Set conn
    public void setConn(string dirKey, string connName)
    {
        connDict.Add(dirKey, connName);
    }
    public void removeConn(string dirKey)
    {
        if (connDict.ContainsKey(dirKey)) connDict.Remove(dirKey);
    }

    // Change sprite
    public void changeSprite(Sprite sp, string text)
    {
        spriteRenderer.sprite = sp;
        myName = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
