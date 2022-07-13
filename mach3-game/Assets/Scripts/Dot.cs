using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    //MatchFind Script Ref
    private Matchfinder findmatch;
    //Board Script Ref
    private Board board;
    [Header("x,y")]
    public int Column; //x
    public int Row;    //y
    [Header ("target x,y")]
    public int TargetX; //x
    public int Targety; //y
    private GameObject otherDot;
    //vector that need Swipes
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
  [Header ("Swipe direction angle")]
    public float SwipeAngle;
    [Header("Match")]
    public bool MatchChecker=false;
    public int oldX;
    public int oldY;                               
    [Header("SwipeFixer")]
    public float SwipeFix = 0.02f;
    void Start()
    {
        findmatch = FindObjectOfType<Matchfinder>();
        board = FindObjectOfType<Board>();
       
        
    }
    
    
    void Update() 
    {

        if (MatchChecker)
        {
            SpriteRenderer Choice = GetComponent<SpriteRenderer>();
            Choice.color = new Color(1f, 1f, 1f, .2f);
        }

        TargetX = Column;
        Targety = Row;
        if (Mathf.Abs(TargetX - transform.position.x) > .1)
        {
            //move toward the target
            tempPosition = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position,tempPosition,.6f);
            if (board.alldots[Column, Row] != this.gameObject)
            {
                board.alldots[Column, Row] = this.gameObject;
            }
            findmatch.MatchFinderFunc();
        }
        else
        {
            //Direcly set the position
            tempPosition = new Vector2(TargetX, transform.position.y);
            transform.position = tempPosition;
            board.alldots[Column, Row] = this.gameObject;
        }
         if (Mathf.Abs(Targety - transform.position.y) >  .1)
        {
            //move toward the target
            tempPosition = new Vector2(transform.position.x, Targety);
            transform.position = Vector2.Lerp(transform.position,tempPosition,.6f);
            if (board.alldots[Column, Row] != this.gameObject)
            {
                board.alldots[Column, Row] = this.gameObject;
            }
            findmatch.MatchFinderFunc();

        }
        else
        {
            //Direcly set the position
            tempPosition = new Vector2(transform.position.x, Targety);
            transform.position = tempPosition;
            board.alldots[Column, Row] = this.gameObject;
        }

        
    }
    public IEnumerator checkMoves()
    {
       

        yield return new WaitForSeconds(.5f);
        if (otherDot!=null){
           
            
                if (!MatchChecker && !otherDot.GetComponent<Dot>().MatchChecker)
                {
                    otherDot.GetComponent<Dot>().Row = Row;
                    otherDot.GetComponent<Dot>().Column = Column;
                    Row = oldY;
                    Column = oldX;
                    yield return new WaitForSeconds(.4f);
                    board.currentState = GameState.move;
                }
          
            else
            {
                board.machdestroyer();
                board.currentState = GameState.move;
            }
            otherDot = null;
        }
        
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log(firstTouchPosition+"first"); //control
            
        }
      
}
    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(finalTouchPosition+"final"); //control
            calculeteAngle();
            StartCoroutine(waiter());
        }
        else 
        {
            StartCoroutine(waiter());
        }
    }
    void calculeteAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > SwipeFix || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > SwipeFix)
        {
            //board.currentState = GameState.wait;
            SwipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            //Debug.Log(SwipeAngle); //angle.control
            MovePieceLogic();
            
        }
        else
        {
            board.currentState = GameState.move;
        }


    }
    private void moveWithVector(Vector2 MoveVector)
    {
        otherDot = board.alldots[Column + (int)MoveVector.x, Row + (int)MoveVector.y];
        oldY = Row;
        oldX = Column;
        otherDot.GetComponent<Dot>().Column += -1 * (int)MoveVector.x;
        otherDot.GetComponent<Dot>().Row += -1 * (int)MoveVector.y;
        Column += (int)MoveVector.x;
        Row += (int)MoveVector.y;
        StartCoroutine(checkMoves());
    }


    void MovePieceLogic()
    {
        if (SwipeAngle > -45 && SwipeAngle <= 45 && Column<board.width-1)
        {
            
            moveWithVector(Vector2.right);
         
        }
        else if (SwipeAngle > 45 && SwipeAngle <= 145 && Row<board.height-1)
        {
             
            moveWithVector(Vector2.up);
        }
        else if ((SwipeAngle > 135 || SwipeAngle <= -135)&& Column>0)
        {
            
            moveWithVector(-Vector2.right);

        }
        else if (SwipeAngle < -45 && SwipeAngle >= -135 && Row>0)
        {
           
            moveWithVector(-Vector2.up);


        }


        board.currentState = GameState.move;
        
    }

    IEnumerator waiter()
    {
        board.currentState = GameState.wait;
        yield return new WaitForSeconds(0.8f);
        board.currentState = GameState.move;
        
      
    }


}
