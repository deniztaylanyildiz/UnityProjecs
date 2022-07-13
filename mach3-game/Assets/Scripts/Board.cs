using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    wait,
    move

}
public class Board : MonoBehaviour
{

   [Header ("Board x*y")]
    public int width;
    public int height;
    public int offset;
    [Header("BackGroundMove")]
    public int LeftRight;
    public int updown;
    [Header("TÝle")]
    public GameObject tileprefab;
    [Header("GameObjects")]
    public GameObject[] dots;
    public GameObject[,] alldots;

    [Header("Game State")]
    public GameState currentState = GameState.move;

    //backgroundtile
    public  BackGroundTile[,] allTiles;
    //MatchFinder ref
    private Matchfinder findmatch;
    void Start()
    {
        allTiles = new BackGroundTile[width , height];
        alldots = new GameObject[width, height];
        setup();
        findmatch = FindObjectOfType<Matchfinder>();
    }

   private void setup()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
               GameObject backGroundtile= Instantiate(tileprefab,tempPosition, Quaternion.identity)as GameObject;
                backGroundtile.transform.parent = this.transform;
                backGroundtile.name = "(" + i + "," + j + ")";
                int  DotToUse = Random.Range(0, dots.Length);
                int LoppFixer=0;
                while (machStart(i, j, dots[DotToUse])&&LoppFixer<100)
                {
                    DotToUse = Random.Range(0, dots.Length);
                    LoppFixer++;
                }
                LoppFixer = 0;
                GameObject dot = Instantiate(dots[DotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().Row=j;
                dot.GetComponent<Dot>().Column = i;

                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";

                alldots[i, j] = dot;
            }
        }


    }

    private bool machStart(int column,int row,GameObject GamePiece)
    {
        if (column > 1 && row > 1)
        {
            if (alldots[column - 1, row].tag == GamePiece.tag && alldots[column - 2, row].tag == GamePiece.tag)
                return true;
       
        if (alldots[column, row-1].tag == GamePiece.tag && alldots[column , row-2].tag == GamePiece.tag)
                return true;
        }
        else if(column<=1 || row <= 1)
        {
            if(row > 1)
            {
                if (alldots[column, row - 1].tag == GamePiece.tag && alldots[column, row - 2].tag == GamePiece.tag)
                    return true;
            }
            if (column > 1)
            {
                if (alldots[column - 1, row ].tag == GamePiece.tag && alldots[column - 2, row].tag == GamePiece.tag)
                    return true;
            }
        }
        return false;
    }
    private void machDestroyerspecific(int column,int row)
    {
        if (alldots[column, row].GetComponent<Dot>().MatchChecker)
        {
            findmatch.currentMatches.Remove(alldots[column, row]);
            Destroy(alldots[column, row]);
            alldots[column, row] = null;

        }
    }
    public void machdestroyer()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (alldots[i, j]!=null)
                {
                    machDestroyerspecific(i, j);
                }
            }
        }
        StartCoroutine(RowCalculate());  
    }
    private IEnumerator RowCalculate()
    {
        int nullcounter = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (alldots[i, j] == null)
                    nullcounter++;

                else if (nullcounter > 0)
                {
                    alldots[i, j].GetComponent<Dot>().Row -= nullcounter;
                    alldots[i, j] = null;
                }
            }
            nullcounter = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(fillboard());
    }

    private void RemakeGameP()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                currentState = GameState.wait ;
                if (alldots[i, j] == null&&i!=3)
                {
                    Vector2 tempPosition = new Vector2(i, j + offset);
                    int dotToUse = Random.Range(0, dots.Length);
                    
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    alldots[i, j] = piece;
                    piece.transform.parent = this.transform;
                    piece.name = "( " + i + ", " + j + " )";
                    piece.GetComponent<Dot>().Row = j;
                    piece.GetComponent<Dot>().Column = i;

                    



                }
               
            }
        }
    }
    private bool machesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (alldots[i, j] != null)
                {
                    if (alldots[i, j].GetComponent<Dot>().MatchChecker)
                        return true;
                }
            }
        }

        return false;
    }
    private IEnumerator fillboard()
    {
        RemakeGameP();
        yield return new WaitForSeconds(.4f);
        while (machesOnBoard()){

            yield return new WaitForSeconds(.4f);
            machdestroyer();

        }

        yield return new WaitForSeconds(.4f);
        currentState = GameState.move;

    }
   

}
