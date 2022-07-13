using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matchfinder : MonoBehaviour
{
    private Board board; 
    public List<GameObject> currentMatches = new List<GameObject>();
    
    void Start()
        
    {
        board = FindObjectOfType<Board>();
    }
    public void MatchFinderFunc()
    {
        StartCoroutine(MachFinderNuma());

    }

    private IEnumerator MachFinderNuma()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject curentdott = board.alldots[i, j];
                if(curentdott != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.alldots[i - 1, j];
                        GameObject rightDot = board.alldots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            if (leftDot.tag == curentdott.tag && rightDot.tag == curentdott.tag)
                            {

                                if (!currentMatches.Contains(leftDot))
                                {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().MatchChecker = true;
                                if (!currentMatches.Contains(rightDot))
                                {
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().MatchChecker = true;
                                if (!currentMatches.Contains(curentdott))
                                {
                                    currentMatches.Add(curentdott);
                                }
                                curentdott.GetComponent<Dot>().MatchChecker = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.alldots[i , j + 1];
                        GameObject downDot = board.alldots[i , j - 1];
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == curentdott.tag && downDot.tag == curentdott.tag)
                            {
                                if (!currentMatches.Contains(upDot))
                                {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().MatchChecker = true;
                                if (!currentMatches.Contains(downDot))
                                {
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<Dot>().MatchChecker = true;
                                if (!currentMatches.Contains(curentdott))
                                {
                                    currentMatches.Add(curentdott);
                                }
                                curentdott.GetComponent<Dot>().MatchChecker = true;
                            }
                        }
                    }
                }
            }
        }


    }
}
