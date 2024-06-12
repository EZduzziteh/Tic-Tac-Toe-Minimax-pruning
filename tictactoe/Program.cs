using System;
using System.Collections.Generic;

class Tile
{
    public int x;
    public int y;
    public Tile up;
    public Tile down;
    private Tile upLeft;
    private Tile upRight;
    private Tile downLeft;
    public Tile left;
    public Tile right;
    public string value = " ";
    private Tile downRight;

    public void PrintCoordinates()
    {
        Console.Write(x + ", " + y);
    }

    internal void PrintValues()
    {
        Console.Write(value);
    }

    internal void PrintAdjacent()
    {
        Console.WriteLine("Adjacencies for Tile " + value + ":");

        if (up != null)
        {
            Console.WriteLine("     Up: " + up.value);
        }
        else
        {
            Console.WriteLine("     Up: None");
        }

        if (down != null)
        {
            Console.WriteLine("     Down: " + down.value);
        }
        else
        {
            Console.WriteLine("     Down: None");
        }

        if (left != null)
        {
            Console.WriteLine("     Left: " + left.value);
        }
        else
        {
            Console.WriteLine("     Left: None");
        }

        if (right != null)
        {
            Console.WriteLine("     Right: " + right.value);
        }
        else
        {
            Console.WriteLine("     Right: None");
        }

        if (upLeft != null)
        {
            Console.WriteLine("     UpLeft: " + upLeft.value);
        }
        else
        {
            Console.WriteLine("     UpLeft: None");
        }

        if (downLeft != null)
        {
            Console.WriteLine("     DownLeft: " + downLeft.value);
        }
        else
        {
            Console.WriteLine("     DownLeft: None");
        }

        if (upRight != null)
        {
            Console.WriteLine("     UpRight: " + upRight.value);
        }
        else
        {
            Console.WriteLine("     UpRight: None");
        }

        if (downRight != null)
        {
            Console.WriteLine("     DownRight: " + downRight.value);
        }
        else
        {
            Console.WriteLine("     DownRight: None");
        }
    }


    internal void CalculateAdjacents(int boardSize, List<List<Tile>> puzzleBoard)
    {

            
       //// 0 1 2
      //  0
        //1
       // 2

            left = x > 0 ? puzzleBoard[x-1][y] : null;
            right = x < boardSize - 1 ? puzzleBoard[x+1][y] : null;
            up = y > 0 ? puzzleBoard[x][y-1] : null;
            down = y < boardSize - 1 ? puzzleBoard[x][y+1] : null;




            upLeft = (x > 0 && y > 0) ? puzzleBoard[x - 1][y - 1] : null;


            
            upRight = (x < boardSize - 1 && y > 0) ? puzzleBoard[x + 1][y - 1] : null;


            
            downLeft = (y < boardSize - 1 && x > 0) ? puzzleBoard[x - 1][y + 1] : null;
            
        
        
            downRight = (x < boardSize - 1 && y < boardSize - 1) ? puzzleBoard[x + 1][y + 1] : null;
        

    }
    
    internal bool CanWin()
    {
        if(value == "-")
        {
            return false;
        }

        //check left - right
        if (left != null && right !=null)
        {
            if (left.value == value && right.value == value)
            {
                return true;
            }
        }


        //check up - down
        if(up!=null && down != null)
        {
            if (up.value == value && down.value == value)
            {
                return true;
            }
        }

        //check diagonal upleft - downright
        if(upLeft != null && downRight != null)
        {
            if(upLeft.value == value && downRight.value == value)
            {
                return true;
            }
        }


            //diagolan downleft - upright

        if(downLeft!=null && upRight != null)
        {
            if (downLeft.value == value && upRight.value == value)
            {
                return true;
            } 
        }

        return false;

    }
}


class Program
{
    public static bool isGameOver = false;
    public static bool checkIsGameOver(List<List<Tile>> boardState, bool isXTurn)
    {
        if (!isXTurn)
        {
            foreach (List<Tile> row in boardState)
            {
                foreach (Tile tile in row)
                {
                    if (tile.value == "X")
                    {
                        if (tile.CanWin())
                        {
                            Console.WriteLine("X has won!");
                          
                            isGameOver = true;
                            return true;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (List<Tile> row in boardState)
            {
                foreach (Tile tile in row)
                {
                    if (tile.value == "O")
                    {
                        if (tile.CanWin())
                        {
                            Console.WriteLine("O has won!");
                            
                            isGameOver = true;
                            return true;
                        }
                    }
                }
            }
        }


        return false;
    }
    public static List<List<Tile>> miniMax(List<List<Tile>> currentBoard, int depth, bool maximize, int a, int b)
    {
        if (depth == 0 || isGameOver)
        {
                
            return currentBoard;
        }

        //maximize is x turn
        if (maximize)
        {
            //get highest
            List<List<Tile>> maxBoardState = currentBoard;
            //foreach possible move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<List<Tile>> newBoard = new List<List<Tile>>();

                    //generate a copy of our input board and put it into new board
                    for (int p = 0; p < 3; p++)
                    {
                        List<Tile> tempList = new List<Tile>();
                        for (int q = 0; q < 3; q++)
                        {
                            Tile temp = new Tile();
                            temp.x = currentBoard[p][q].x;
                            temp.y = currentBoard[p][q].y;
                            temp.value = currentBoard[p][q].value;
                            tempList.Add(temp);
                        }
                        newBoard.Add(tempList);
                    }

                    ///calculate adjacents for new board
                    foreach (var row in newBoard)
                    {
                        foreach (var tile in row)
                        {
                            tile.CalculateAdjacents(3, newBoard);
                        }
                    }




                    //we can only place on empty tiles **marked with - **
                    if (newBoard[i][j].value == "-")
                    {
                        newBoard[i][j].value = "X";


                        //int x = getBoardScore(maxBoardState, true);

                        int boardScore = getBoardScore(miniMax(newBoard, depth - 1, false, a, b), true);

                        if(boardScore > a)
                        {
                            a = boardScore;
                            maxBoardState = newBoard;
                        }
                       
                        if(a >= b)
                        {
                            break;
                        }

                    }
                }

                if(a >= b)
                {
                    break;
                }
            }
            if(!isGameOver)
            if(checkIsGameOver(maxBoardState, maximize))
            {
                    Console.WriteLine("Max State:");
                    Console.WriteLine(" 123");
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            Console.Write("a");
                        }
                        else if (i == 1)
                        {
                            Console.Write("b");
                        }
                        else if (i == 2)
                        {
                            Console.Write("c");
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            maxBoardState[i][j].PrintValues();
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                }


            return maxBoardState;
        }
        //minimize is o turn
        else
        {
            //get lowest
            List<List<Tile>> minBoardState = currentBoard;
            //foreach possible move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<List<Tile>> newBoard = new List<List<Tile>>();

                    //generate a copy of our input board and put it into new board
                    for (int p = 0; p < 3; p++)
                    {
                        List<Tile> tempList = new List<Tile>();
                        for (int q = 0; q < 3; q++)
                        {
                            Tile temp = new Tile();
                            temp.x = currentBoard[p][q].x;
                            temp.y = currentBoard[p][q].y;
                            temp.value = currentBoard[p][q].value;
                            tempList.Add(temp);
                        }
                        newBoard.Add(tempList);
                    }

                    ///calculate adjacents for new board
                    foreach (var row in newBoard)
                    {
                        foreach (var tile in row)
                        {
                            tile.CalculateAdjacents(3, newBoard);
                        }
                    }


                    //we can only place on empty tiles **marked with - **
                    if (newBoard[i][j].value == "-")
                    {
                        newBoard[i][j].value = "O";

                     
                        int boardScore = getBoardScore(miniMax(newBoard, depth - 1, true, a, b), false);

                        if (boardScore < b)
                        {
                            b = boardScore;
                            minBoardState = newBoard;
                        }

                        if(a >= b)
                        {
                            break;
                        }
                    }
                }
                if(a >= b)
                {
                    break;
                }
            }

            if (!isGameOver)
            {
                if (checkIsGameOver(minBoardState, maximize))
                {
                    Console.WriteLine("Min State:");
                    Console.WriteLine(" 123");
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            Console.Write("a");
                        }
                        else if (i == 1)
                        {
                            Console.Write("b");
                        }
                        else if (i == 2)
                        {
                            Console.Write("c");
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            minBoardState[i][j].PrintValues();
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
            
            return minBoardState;



        }

    }

    private static int getBoardScore(List<List<Tile>> board, bool isXTurn)
    {
        // X prefers psoitive, o prefers negative.


        //check for win
        if (isXTurn)
        {
            //if there is a way for x to win, give board score maxValue, this is the best condition we dont need to check any more.
            foreach (List<Tile> b in board)
            {
                foreach (Tile tile in b)
                {
                    if (tile.value == "X")
                    {
                        if (tile.CanWin())
                        {
                            return int.MaxValue;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (List<Tile> b in board)
            {
                foreach (Tile tile in b)
                {
                    if (tile.value == "O")
                    {
                        if (tile.CanWin())
                        {
                            return int.MinValue;
                        }
                    }
                }
            }
            //if there is a way for o to win, give board score minValue, this is the best condition and we dont need to check anymore
        }

        //if no immediate win, 


        //thoughts for expansion
        

        /*
        if (isXTurn)
        {
            //for each board:

            //check if there is a way x can make a set

       
            //if there is, +100 points

            //if there isn't, check if there is a way that O can make a set
            //if there is, +50 points to block that off

            //otherwise, for spaces adjacent to where we already have an x, give +10 points

            //**maybe will implement maybe not: -50 points for places that will never result in a set for us

        }
        else
        {
            //for each board:

            //check if there is a way o can make a set
            //if there is, -100 points

            //if there isn't, check if there is a way that x can make a set
            //if there is, -50 points to block that off

            //otherwise, for spaces adjacent to where we already have an x, give -10 points

            //**maybe will implement maybe not: +50 points for places that will never result in a set for us

        }*/

        return -1;
    }

    public static void Main(string[] args)
    {
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int depth = 8;
        bool isXTurn = true;
        List<List<Tile>> board = new List<List<Tile>>();
        int boardSize = 3;
        int count = 0;
        for (int i = 0; i < boardSize; i++)
        {
            List<Tile> tempList = new List<Tile>();
            for (int j = 0; j < boardSize; j++)
            {
                Tile temp = new Tile();
                temp.x = i;
                temp.y = j;
                temp.value = "-";
                tempList.Add(temp);
                count++;
            }
            board.Add(tempList);
        }


        Console.WriteLine("Welcome to Tic Tac Toe minimax with alpha beta pruning by Sasha Greene");
        Console.WriteLine("Enter 2 characters to place your mark in this format: a1 to place a mark (starts with X)");
        Console.WriteLine("Enter m to run minimax from whatever the current board state is and find the first victory state");
        Console.WriteLine("Enter q to quit");

        while (!isGameOver)
        {

            Console.WriteLine("Tic Tac Toe:");
            Console.WriteLine(" 123");
            for (int i = 0; i < boardSize; i++)
            {
                if (i == 0)
                {
                    Console.Write("a");
                }
                else if (i == 1)
                {
                    Console.Write("b");
                }
                else if (i == 2)
                {
                    Console.Write("c");
                }

                for (int j = 0; j < boardSize; j++)
                {
                    board[i][j].PrintValues();
                }
                Console.WriteLine();
            }

            string input = Console.ReadLine().ToLower();

            if (input == "q")
            {
                isGameOver = true;
                Console.WriteLine("Quitting");
            }
            else if (input == "m")
            {
                Console.WriteLine("Minimax!");
                board = miniMax(board, depth, isXTurn, alpha, beta);

                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        board[i][j].PrintValues();
                        
                    }
                }
            }
            else
            {
                if (input.Length == 2)
                {
                    int x = 0;
                    int y = 0;


                    switch (input[0])
                    {
                        default:
                            Console.WriteLine("No such coordinate");
                            break;
                        case 'a':
                            x = 0;
                            break;
                        case 'b':
                            x = 1;
                            break;
                        case 'c':
                            x = 2;
                            break;
                    }

                    switch (input[1])
                    {
                        default:
                            Console.WriteLine("No such coordinate");
                            break;
                        case '1':
                            y = 0;
                            break;
                        case '2':
                            y = 1;
                            break;
                        case '3':
                            y = 2;
                            break;
                    }

                    //check if empty space
                    if (board[x][y].value == "-")
                    {
                        if (isXTurn)
                        {
                            board[x][y].value = "X";
                            isXTurn = false;
                        }
                        else
                        {
                            board[x][y].value = "O";
                            isXTurn = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Can't place at that location!");
                    }

                }
                else
                {
                    Console.WriteLine("Enter 2 characters to place your mark in this format: a1");
                }

            }

        }
        Console.WriteLine("gameover");
        Console.ReadLine();
    }

}
