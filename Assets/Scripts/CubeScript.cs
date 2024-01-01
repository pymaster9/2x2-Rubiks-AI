using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public List<GameObject> pieces;
    public GameObject pieceprefab;
    public List<Material> stickers;
    public int minScrambleMoves;
    public Texture main;
    // Start is called before the first frame update
    public void RebuildCube()
    {
        foreach(GameObject piece in pieces)
        {
            Destroy(piece);
        }
        pieces = new List<GameObject>();
        for(float x = -0.5f; x <1; x++)
        {
            for (float y = -0.5f; y < 1; y++)
            {
                for (float z = -0.5f; z < 1; z++)
                {
                    GameObject currentpiece = Instantiate(pieceprefab);
                    currentpiece.transform.parent = transform;
                    currentpiece.transform.localPosition = new Vector3(x, -y, z);
                    currentpiece.name = x.ToString() + "," + (-y).ToString() + "," + z.ToString();
                    currentpiece.GetComponent<RubiksCubePiece>().idealPosition = new Vector3(x, -y, z);
                    pieces.Add(currentpiece);
                    for (int sticker = 1; sticker < 7; sticker++)
                    {
                        currentpiece.transform.Find("Sticker" + sticker.ToString()).GetComponent<Renderer>().material = stickers[sticker-1];
                        if(currentpiece.transform.Find("Sticker" + sticker.ToString()).transform.position.x == 0 || currentpiece.transform.Find("Sticker" + sticker.ToString()).transform.position.y == 0 || currentpiece.transform.Find("Sticker" + sticker.ToString()).transform.position.z == 0) { Destroy(currentpiece.transform.Find("Sticker" + sticker.ToString()).gameObject); }
                    }
                }
            }
        }
    }
    public void LeftTurn(bool Prime)
    {
        GameObject hinge = new GameObject();
        hinge.transform.parent = transform;
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        foreach(GameObject currentpiece in pieces)
        {
            if(currentpiece.transform.localPosition.x <= 0)
            {
                currentpiece.transform.parent = hinge.transform;
            }
        }
        if (Prime)
        {
            hinge.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            hinge.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
        Reparent();
        Destroy(hinge);
    }
    public void RightTurn(bool Prime)
    {
        GameObject hinge = new GameObject();
        hinge.transform.parent = transform;
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        foreach (GameObject currentpiece in pieces)
        {
            if (currentpiece.transform.localPosition.x >= 0)
            {
                currentpiece.transform.parent = hinge.transform;
            }
        }
        if (Prime)
        {
            hinge.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            hinge.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        Reparent();
        Destroy(hinge);
    }
    public void DownTurn(bool Prime)
    {
        GameObject hinge = new GameObject();
        hinge.transform.parent = transform;
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        foreach (GameObject currentpiece in pieces)
        {
            if (currentpiece.transform.localPosition.y <= 0)
            {
                currentpiece.transform.parent = hinge.transform;
            }
        }
        if (Prime)
        {
            hinge.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            hinge.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        Reparent();
        Destroy(hinge);
    }
    public void UpTurn(bool Prime)
    {
        GameObject hinge = new GameObject();
        hinge.transform.parent = transform;
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        foreach (GameObject currentpiece in pieces)
        {
            if (currentpiece.transform.localPosition.y >= 0)
            {
                currentpiece.transform.parent = hinge.transform;
            }
        }
        if (Prime)
        {
            hinge.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            hinge.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        Reparent();
        Destroy(hinge);
    }
    public void FrontTurn(bool Prime)
    {
        GameObject hinge = new GameObject();
        hinge.transform.parent = transform;
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        foreach (GameObject currentpiece in pieces)
        {
            if (currentpiece.transform.localPosition.z <= 0)
            {
                currentpiece.transform.parent = hinge.transform;
            }
        }
        if (Prime)
        {
            hinge.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            hinge.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        Reparent();
        Destroy(hinge);
    }
    public void BackTurn(bool Prime)
    {
        GameObject hinge = new GameObject();
        hinge.transform.parent = transform;
        hinge.transform.localPosition = new Vector3(0, 0, 0);
        foreach (GameObject currentpiece in pieces)
        {
            if (currentpiece.transform.localPosition.z >= 0)
            {
                currentpiece.transform.parent = hinge.transform;
            }
        }
        if (Prime)
        {
            hinge.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            hinge.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        Reparent();
        Destroy(hinge);
    }
    void Reparent()
    {
        foreach(GameObject curr in pieces)
        {
            curr.transform.parent = transform;
        }
    }
    public List<float> TakeInput()
    {
        List<float> output = new List<float>();
        int[,] left = new int[2,2];
        int[,] right = new int[2, 2];
        int[,] down = new int[2, 2];
        int[,] up = new int[2, 2];
        int[,] front = new int[2, 2];
        int[,] back = new int[2, 2];

        foreach (GameObject piece in pieces)
        {
            foreach (Transform stickerTransform in piece.transform.GetComponentsInChildren<Transform>())
            {
                stickerTransform.parent = transform;
                GameObject sticker = stickerTransform.gameObject;
                int materialIndex = FindMaterial(sticker);
                if(Mathf.Abs((stickerTransform.localPosition.z) + 1) <= 0.05f)
                {
                    left[Mathf.RoundToInt(piece.transform.localPosition.x + 0.5f), Mathf.RoundToInt(piece.transform.localPosition.y + 0.5f)] = materialIndex;
                }
                else if (Mathf.Abs((stickerTransform.localPosition.z) - 1) <= 0.05f)
                {
                    right[Mathf.RoundToInt(piece.transform.localPosition.x + 0.5f), Mathf.RoundToInt(piece.transform.localPosition.y + 0.5f)] = materialIndex;
                }
                else if (Mathf.Abs((stickerTransform.localPosition.y) + 1) <= 0.05f)
                {
                    down[Mathf.RoundToInt(piece.transform.localPosition.x + 0.5f), Mathf.RoundToInt(piece.transform.localPosition.z + 0.5f)] = materialIndex;
                }
                else if (Mathf.Abs((stickerTransform.localPosition.y) - 1) <= 0.05f)
                {
                    up[Mathf.RoundToInt(piece.transform.localPosition.x + 0.5f), Mathf.RoundToInt(piece.transform.localPosition.z + 0.5f)] = materialIndex;
                }
                else if (Mathf.Abs((stickerTransform.localPosition.x) + 1) <= 0.05f)
                {
                    back[Mathf.RoundToInt(piece.transform.localPosition.y + 0.5f), Mathf.RoundToInt(piece.transform.localPosition.z + 0.5f)] = materialIndex;
                }
                else if (Mathf.Abs((stickerTransform.localPosition.x) - 1) <= 0.05f)
                {
                    front[Mathf.RoundToInt(piece.transform.localPosition.y + 0.5f), Mathf.RoundToInt(piece.transform.localPosition.z + 0.5f)] = materialIndex;
                }
                stickerTransform.parent = piece.transform;
            }
        }
        foreach (int x in left) { output.Add((float)x); }
        foreach (int x in right) { output.Add((float)x); }
        foreach (int x in down) { output.Add((float)x); }
        foreach (int x in up) { output.Add((float)x); }
        foreach (int x in back) { output.Add((float)x); }
        foreach (int x in front) { output.Add((float)x); }
        return output;
    }
    public void Scramble()
    {
        for(int x = 0; x < minScrambleMoves; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                int choice = Random.Range(0, 4);
                if (choice == 0)
                {
                    RightTurn(Random.Range(0, 3) == 0);
                }
                else if (choice == 1)
                {
                    UpTurn(Random.Range(0, 3) == 0);
                }
                else
                {
                    FrontTurn(Random.Range(0, 3) == 0);
                }
            }
        }
    }
    int FindMaterial(GameObject sticker)
    {
        return sticker.name[7]-'0'-1;
    }
}
