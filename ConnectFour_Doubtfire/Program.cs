using System;
using System.Collections.Generic;
using System.Text;
using SplashKitSDK;

namespace ConnectFour_Doubtfire
{
    public class Board
    {
        // specifying standard size of Connect Four Board and other properties
        private int __rows = 6;
        private int __columns = 7;

        public int Rows { get { return __rows; } }
        public int Columns { get { return __columns; } }

        BoardRenderer graphics = new BoardRenderer();

        private int BoardWidth = 700;
        public int Board_Width { get { return BoardWidth; } }

        private int BoardHeight = 600;
        public int Board_Height { get { return BoardHeight; } }

        private double BoardX = 150;
        public double Board_X { get { return BoardX; } }

        private double BoardY = 100;
        public double Board_Y { get { return BoardY; } }

        private Piece[,] gameBoardPieces;
        public Piece[,] GameBoardPieces
        {
            get
            {
                return gameBoardPieces;
            }
        }
        private Piece[,] winningSpots;

        private GameState _CurrentGameState;
        public GameState CurrentGameState
        {
            get
            {
                return _CurrentGameState;
            }
            set
            {
                _CurrentGameState = value;
            }
        }

        private double[] _currentCircle = new double[] { 0, 0 };

        public double[] getCurrentCircle()
        {
            return _currentCircle;
        }

        private string _GameMode;

        // initialising the board
        public Board()
        {
            InitialiseBoard();
            gameBoardPieces = new Piece[Rows + 1, Columns + 1]; // array holding all the pieces
            winningSpots = new Piece[Rows + 1, Columns + 1];
        }

        public void StartNewGame()
        {
            InitialiseBoard();
            Array.Clear(gameBoardPieces, 0, gameBoardPieces.Length);
            Array.Clear(winningSpots, 0, winningSpots.Length);
        }

        public void InitialiseBoard()
        {
            graphics.DrawBoard(Rows, Columns, BoardWidth, BoardHeight, BoardX, BoardY);
            _CurrentGameState = GameState.Playing;
        }

        // Checks if it is within the circles of the grid
        public bool IsAt(Point2D pt)
        {
            int columnLines = BoardWidth / __columns; // 100
            int rowLines = BoardHeight / __rows; // 100 

            for (int i = 0; i < __columns; i++)
            {
                for (int j = 0; j < __rows; j++)
                {
                    double X = BoardX + columnLines * (i + 0.5);
                    double Y = BoardY + rowLines * (j + 0.5);
                    if ((pt.X > X - graphics.Radius && pt.X < X + graphics.Radius) && (pt.Y > Y - graphics.Radius && pt.Y < Y + graphics.Radius))
                    {
                        _currentCircle = new double[] { X, Y };

                        if (ValidMove(j + 1, i + 1))
                        {
                            gameBoardPieces[j + 1, i + 1] = Piece.Red;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckForVictory(Piece[,] allthePieces)
        {
            // horizontal wins
            for (int row = 0; row < allthePieces.GetLength(0); row++)
            {
                for (int column = 0; column < allthePieces.GetLength(1) - 3; column++)
                {
                    if (allthePieces[row, column] != Piece.None)
                    {
                        if (allthePieces[row, column] == allthePieces[row, column + 1])
                        {
                            if (allthePieces[row, column + 1] == allthePieces[row, column + 2])
                            {
                                if (allthePieces[row, column + 2] == allthePieces[row, column + 3])
                                {
                                    for (int i = 0; i <= 3; i++)
                                    {
                                        winningSpots[row, column + i] = allthePieces[row, column + i];
                                    }
                                    GameIsWon(winningSpots);
                                    return true;
                                }
                            }

                        }
                    }
                }
            }

            // vertical wins
            for (int row = 0; row < allthePieces.GetLength(0) - 3; row++)
            {
                for (int column = 0; column < allthePieces.GetLength(1); column++)
                {
                    if (allthePieces[row, column] != Piece.None)
                    {
                        if (allthePieces[row, column] == allthePieces[row + 1, column])
                        {
                            if (allthePieces[row + 1, column] == allthePieces[row + 2, column])
                            {
                                if (allthePieces[row + 2, column] == allthePieces[row + 3, column])
                                {
                                    for (int i = 0; i <= 3; i++)
                                    {
                                        winningSpots[row + i, column] = allthePieces[row + i, column];
                                    }
                                    GameIsWon(winningSpots);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            // diagonal wins (negative gradient)
            for (int row = 0; row < allthePieces.GetLength(0) - 3; row++)
            {
                for (int column = 0; column < allthePieces.GetLength(1) - 3; column++)
                {
                    if (allthePieces[row, column] != Piece.None)
                    {
                        if (allthePieces[row, column] == allthePieces[row + 1, column + 1])
                        {
                            if (allthePieces[row + 1, column + 1] == allthePieces[row + 2, column + 2])
                            {
                                if (allthePieces[row + 2, column + 2] == allthePieces[row + 3, column + 3])
                                {
                                    for (int i = 0; i <= 3; i++)
                                    {
                                        winningSpots[row + i, column + i] = allthePieces[row + i, column + i];
                                    }
                                    GameIsWon(winningSpots);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            // diagonal wins (positive gradient)
            for (int row = 0; row < allthePieces.GetLength(0) - 3; row++)
            {
                for (int column = allthePieces.GetLength(1) - 1; column >= 3; column--)
                {
                    if (allthePieces[row, column] != Piece.None)
                    {
                        if (allthePieces[row, column] == allthePieces[row + 1, column - 1])
                        {
                            if (allthePieces[row + 1, column - 1] == allthePieces[row + 2, column - 2])
                            {
                                if (allthePieces[row + 2, column - 2] == allthePieces[row + 3, column - 3])
                                {
                                    for (int i = 0; i <= 3; i++)
                                    {
                                        winningSpots[row + i, column - i] = allthePieces[row + i, column - i];
                                    }
                                    GameIsWon(winningSpots);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void GameIsWon(Piece[,] winningSpots)
        {
            for (int i = 0; i < winningSpots.GetLength(0); i++)
            {
                for (int j = 0; j < winningSpots.GetLength(1); j++)
                {
                    if (winningSpots[i, j] != Piece.None)
                    {
                        double X = BoardX + BoardWidth / __columns * (j - 0.5);
                        double Y = BoardY + BoardHeight / __rows * (i - 0.5);
                        SplashKit.FillRectangle(Color.BrightGreen, BoardX + BoardWidth / __columns * (j - 1), BoardY + BoardHeight / __rows * (i - 1), BoardWidth / __columns, BoardHeight / __rows);

                        graphics.DrawPiece(winningSpots[i, j], X, Y);
                        if (winningSpots[i, j] == Piece.Red)
                        {
                            _CurrentGameState = GameState.PlayerWon;
                        }
                        else if (winningSpots[i, j] == Piece.Yellow)
                        {
                            _CurrentGameState = GameState.PCWon;
                        }
                    }
                }
            }
        }

        public bool ValidMove(int row, int column)
        {
            if (row > 6 || row <= 0 || column > 7 || column <= 0)
            {
                return false;
            }
            if (row == 6) // bottom row
            {
                if (gameBoardPieces[row, column] == Piece.None) // spot is available
                {
                    return true;
                }
            }
            else if (gameBoardPieces[row + 1, column] != Piece.None && gameBoardPieces[row, column] == Piece.None) // checks spot below it is filled
            {
                return true;
            }
            return false;
        }

        // checks when the game board is full 
        public bool IsFull()
        {
            for (int i = 0; i < __rows; i++)
            {
                for (int j = 0; j < __columns; j++)
                {
                    if (gameBoardPieces[i, j] == Piece.None)
                    {
                        return false;
                    }
                }
            }
            _CurrentGameState = GameState.Tie;
            return true;
        }

        public void DisplayScores(HumanPlayer Reecey, BotPlayer PC)
        {
            SplashKit.DrawText("Player: " + Reecey.Score + "     ", Color.Black, GameResources.GetFont("Arial"), 26, BoardX + BoardWidth / 4, BoardY * 1.5 + BoardHeight);
            SplashKit.DrawText("PC: " + PC.Score + "     ", Color.Black, GameResources.GetFont("Arial"), 26, BoardX + (3 * BoardWidth) / 4, BoardY * 1.5 + BoardHeight);
        }

        public bool GameStarted(Point2D pt, double[] Check)
        {
            double MenuX = Check[0];
            double MenuY = Check[1];
            double MenuWidth = Check[2];
            double MenuHeight = Check[3];
            double separator = Check[4];

            if (pt.X >= MenuX && pt.X <= MenuX + MenuWidth)
            {
                if (pt.Y >= MenuY && pt.Y <= MenuY + MenuHeight)
                {
                    _GameMode = "Easy";
                    return true; // Easy Mode
                }
                else if (pt.Y >= MenuY + separator && pt.Y <= MenuY + separator + MenuHeight)
                {
                    _GameMode = "Medium";
                    return true; // Medium Mode
                }
                else if (pt.Y >= MenuY + 2 * separator && pt.Y <= MenuY + 2 * separator + MenuHeight)
                {
                    _GameMode = "Hard";
                    return true; // Hard Mode
                }
            }
            return false;
        }

        public string GetGameMode
        {
            get
            {
                return _GameMode;
            }
        }
    }

    public class BoardController // controls the operation and logic of the game/board 
    {
        HumanPlayer Reece = new HumanPlayer();
        BotPlayer PC;
        bool firstGame = true; // used to see if it the first game or not

        public BoardController(Board b, BoardRenderer g)
        {
            b.InitialiseBoard();
        }

        public bool WaitForMove(Board ConnectFour, BoardRenderer g, BotPlayer b)
        {
            if (b.PlayPiece(ConnectFour, g) || ConnectFour.IsFull())
            {
                SplashKit.PlaySoundEffect(GameResources.GetSound("PlayPiece"));
                return true;
            }
            return false;
        }

        public void OperateGame(Board ConnectFour, BoardRenderer g, string difficulty)
        {

            g.DrawGameMode(difficulty);

            switch (ConnectFour.CurrentGameState)
            {
                case GameState.Playing:
                    if (firstGame)
                    {   // creating AI
                        if (difficulty == "Easy")
                        {
                            PC = new EasyBotPlayer();
                        }
                        else if (difficulty == "Medium")
                        {
                            PC = new MediumBotPlayer();
                        }
                        else if (difficulty == "Hard")
                        {
                            PC = new HardBotPlayer();
                        }
                        firstGame = false;
                    }

                    ConnectFour.DisplayScores(Reece, PC);

                    if (Reece.PlayPiece(ConnectFour, g))
                    {
                        SplashKit.PlaySoundEffect(GameResources.GetSound("PlayPiece"));
                        for (; ; )
                        {
                            if (WaitForMove(ConnectFour, g, PC))
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

        }
    }

    // this class is primarily responsible for the graphical rendering of the board and pieces
    public class BoardRenderer
    {
        private int _radius = 40; // standard defined piece size

        public int Radius { get { return _radius; } }

        public BoardRenderer() { }
        public void DrawBoard(int rows, int cols, int width, int height, double hor_offset, double vert_offset)
        {
            SplashKit.ClearScreen(Color.DarkGray);
            SplashKit.DrawText("Connect Four by Reece Hodges", Color.Black, GameResources.GetFont("Arial"), 24, width / 2, vert_offset / 2);
            SplashKit.FillRectangle(Color.Black, hor_offset - 2, vert_offset - 2, width + 4, height + 4); // black outline

            int columnLines = width / cols; // 100
            int rowLines = height / rows; // 100

            for (int i = 0; i < cols; i++)
            {
                // draw grid in individual squares
                for (int j = 0; j < rows; j++)
                {
                    SplashKit.FillRectangle(Color.Blue, hor_offset + columnLines * i, vert_offset + rowLines * j, columnLines, rowLines);
                    SplashKit.FillCircle(Color.White, (2 * hor_offset + columnLines * (2 * i + 1)) / 2, (2 * vert_offset + rowLines * (2 * j + 1)) / 2, _radius);
                }
            }

            // divides the grid 
            for (int i = 1; i < cols; i++)
            {
                SplashKit.FillRectangle(Color.Black, hor_offset + columnLines * i, vert_offset, 2, height);
                for (int j = 1; j < rows; j++)
                {
                    SplashKit.FillRectangle(Color.Black, hor_offset, vert_offset + rowLines * i, width, 2);
                }
            }
        }


        public void DrawPiece(Piece p, double X, double Y)
        {
            if (p == Piece.Red)
            {
                SplashKit.FillCircle(Color.Red, X, Y, _radius);
            }
            else if (p == Piece.Yellow)
            {
                SplashKit.FillCircle(Color.Yellow, X, Y, _radius);
            }
        }

        public double[] DrawMenu()
        {
            SplashKit.ClearScreen(Color.DarkGray);

            // variables to keep track of screen layout/scale. Convenient to change one value and have it dynamically update. 
            double MenuX = 300;
            double MenuY = 50;
            double separator = 250;
            double MenuWidth = 400;
            double MenuHeight = 200;
            double hor_offset = 150;
            double ver_offset = 35;

            double[] gameStartedArray = new double[] { MenuX, MenuY, MenuWidth, MenuHeight, separator };

            SplashKit.FillRectangle(Color.Blue, MenuX, MenuY, MenuWidth, MenuHeight);
            SplashKit.DrawText("Easy Mode", Color.White, GameResources.GetFont("Arial"), 60, MenuX + (MenuWidth / 2) - hor_offset, MenuY + MenuHeight / 2 - ver_offset);

            SplashKit.FillRectangle(Color.Blue, MenuX, MenuY + separator, MenuWidth, MenuHeight);
            SplashKit.DrawText("Medium Mode", Color.White, GameResources.GetFont("Arial"), 60, MenuX + MenuWidth / 2 - hor_offset - 40, MenuY + separator + MenuHeight / 2 - ver_offset);

            SplashKit.FillRectangle(Color.Blue, MenuX, MenuY + separator * 2, MenuWidth, MenuHeight);
            SplashKit.DrawText("Hard Mode", Color.White, GameResources.GetFont("Arial"), 60, MenuX + MenuWidth / 2 - hor_offset, MenuY + separator * 2 + MenuHeight / 2 - ver_offset);

            return gameStartedArray;
        }

        public void DrawGameMode(string GameMode)
        {
            SplashKit.DrawText(GameMode, Color.Black, GameResources.GetFont("Arial"), 25, 800, 35);
        }
    }

    public class BotPlayer : Player
    {

        public BotPlayer() : base(Piece.Yellow)
        {

        }

        public override bool PlayPiece(Board ConnectFour, BoardRenderer graphics)
        {
            return false; // Let the children handle this... 
        }
    }

    public class EasyBotPlayer : BotPlayer, Strategy
    {
        public EasyBotPlayer() : base() { }

        public override bool PlayPiece(Board ConnectFour, BoardRenderer graphics)
        {
            if (PlayRandomly(ConnectFour, graphics))
            {
                return CheckForWin(ConnectFour, this);
            }
            return false;
        }

        public bool PlayRandomly(Board ConnectFour, BoardRenderer graphics)
        {
            return StrategyHelper.PlayRandomly(ConnectFour, graphics);
        }
        public bool CheckForWin(Board ConnectFour, Player p)
        {
            return StrategyHelper.CheckForWin(ConnectFour, this);
        }

        /*
            The following checks are not used for Easy Mode
        */

        public bool CheckDiagonal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            throw new NotImplementedException();
        }

        public bool CheckHorizontal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            throw new NotImplementedException();
        }

        public bool CheckVertical(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            throw new NotImplementedException();
        }
    }

    public class GameManager
    {
        Board ConnectFour = new Board();
        BoardRenderer graphics = new BoardRenderer();
        BoardController Operator;
        ProgramState _currentState = new ProgramState();


        private static GameManager _instance;
        public static GameManager Instance
        {
            get { return _instance; } // Singleton pattern here
        }

        public GameManager()
        {
            if (_instance == null)
                _instance = this;
            else
                throw new Exception("Cannot have more than one instance of GameManager");
            Operator = new BoardController(ConnectFour, graphics);
            _currentState = ProgramState.Menu;
        }

        public void RunGame()
        {
            SplashKit.ProcessEvents();
            switch (_currentState)
            {
                case ProgramState.Menu:
                    double[] Check = graphics.DrawMenu();
                    if (SplashKit.MouseClicked(MouseButton.LeftButton))
                    {
                        if (ConnectFour.GameStarted(SplashKit.MousePosition(), Check))
                        {
                            _currentState = ProgramState.Playing;
                            ConnectFour.InitialiseBoard();
                        }
                    }
                    break;
                case ProgramState.Playing:
                    string GameMode = ConnectFour.GetGameMode;
                    Operator.OperateGame(ConnectFour, graphics, GameMode);
                    if ((ConnectFour.CurrentGameState != GameState.Playing) || ConnectFour.IsFull())
                    {
                        _currentState = ProgramState.Results;
                        if (ConnectFour.CurrentGameState == GameState.PlayerWon)
                        {
                            SplashKit.PlaySoundEffect(GameResources.GetSound("PlayerWon"));
                        }
                        else if (ConnectFour.CurrentGameState == GameState.PCWon)
                        {
                            SplashKit.PlaySoundEffect(GameResources.GetSound("PCWon"));
                        }
                    }
                    break;
                case ProgramState.Results:
                    SplashKit.DrawText("Press Y to start a new game :)", Color.Black, GameResources.GetFont("Arial"), 16, 50, 50);
                    if (SplashKit.KeyDown(KeyCode.YKey))
                    {
                        ConnectFour.StartNewGame();
                        _currentState = ProgramState.Playing;
                    }
                    break;
            }
            SplashKit.RefreshScreen();
        }
    }

    public static class GameResources
    {
        private static Dictionary<string, Font> _fonts;
        private static Dictionary<string, SoundEffect> _sounds;

        private static Font _standardFont;
        private static SoundEffect _playPiece;
        private static SoundEffect _playerWon;
        private static SoundEffect _pcWon;

        static GameResources()
        {
            _fonts = new Dictionary<string, Font>();
            _sounds = new Dictionary<string, SoundEffect>();
            _standardFont = new Font("Arial", "/arial.ttf");
            _fonts.Add("Arial", _standardFont);
            _playPiece = new SoundEffect("PlayPiece", "place_piece.mp3");
            _playerWon = new SoundEffect("PlayerWon", "PlayerWon.mp3");
            _pcWon = new SoundEffect("PCWon", "PCWon.mp3");
            _sounds.Add("PlayerWon", _playerWon);
            _sounds.Add("PlayPiece", _playPiece);
            _sounds.Add("PCWon", _pcWon);
        }
        public static Font GetFont(string fontName)
        {
            try
            {
                return _fonts[fontName];
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Cannot find font: " + fontName);
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public static SoundEffect GetSound(string soundName)
        {
            try
            {
                return _sounds[soundName];
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Cannot find sound: " + soundName);
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }

    public enum GameState
    {
        Playing,
        PlayerWon,
        PCWon,
        Tie
    }

    public class HardBotPlayer : BotPlayer, Strategy
    {
        /* 
            Hard Mode will check if it has any three-a-rows in any direction first and then play in the fourth spot to win.
            If not, it will check if the player has gotten any three-in-a-rows diagonally, horizontally or vertically and try to stop it. 
            Otherwise, it will play randomly.  
        */
        public HardBotPlayer() : base() { }

        public override bool PlayPiece(Board ConnectFour, BoardRenderer graphics)
        {

            if (CheckDiagonal(ConnectFour, graphics, Piece.Yellow))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckHorizontal(ConnectFour, graphics, Piece.Yellow))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckVertical(ConnectFour, graphics, Piece.Yellow))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckDiagonal(ConnectFour, graphics, Piece.Red))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckHorizontal(ConnectFour, graphics, Piece.Red))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckVertical(ConnectFour, graphics, Piece.Red))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (PlayRandomly(ConnectFour, graphics))
            {
                return CheckForWin(ConnectFour, this);
            }
            return false;
        }

        public bool PlayRandomly(Board ConnectFour, BoardRenderer graphics)
        {
            return StrategyHelper.PlayRandomly(ConnectFour, graphics);
        }

        public bool CheckForWin(Board ConnectFour, Player p)
        {
            return StrategyHelper.CheckForWin(ConnectFour, this);
        }

        public bool CheckDiagonal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            return StrategyHelper.CheckDiagonal(ConnectFour, graphics, p);
        }

        public bool CheckHorizontal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            return StrategyHelper.CheckHorizontal(ConnectFour, graphics, p);
        }

        public bool CheckVertical(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            return StrategyHelper.CheckVertical(ConnectFour, graphics, p);
        }
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer() : base(Piece.Red) { }

        public override bool PlayPiece(Board ConnectFour, BoardRenderer graphics)
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (ConnectFour.IsAt(SplashKit.MousePosition()))
                {
                    double[] pos = ConnectFour.getCurrentCircle(); // get the position
                    graphics.DrawPiece(Piece.Red, pos[0], pos[1]);

                    if (ConnectFour.CheckForVictory(ConnectFour.GameBoardPieces))
                    {
                        IncrementScore();
                    }
                    else
                    {
                        return !ConnectFour.IsFull();
                    }
                }
            }
            return false;
        }
    }

    public class MediumBotPlayer : BotPlayer, Strategy
    {
        /* 
            Medium Mode will check if it has any three-in-a-rows horizontally and vertically and play in the fourth spot to win.
            If not, then it will check if the player has three in a row horizontally or vertically and try to stop it.
            If the player does not have three in a row horizontally or vertically, the AI will play randomly. 
        */
        public MediumBotPlayer() : base() { }

        public override bool PlayPiece(Board ConnectFour, BoardRenderer graphics)
        {
            if (CheckHorizontal(ConnectFour, graphics, Piece.Yellow))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckVertical(ConnectFour, graphics, Piece.Yellow))
            {
                return CheckForWin(ConnectFour, this);
            }
            if (CheckHorizontal(ConnectFour, graphics, Piece.Red))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (CheckVertical(ConnectFour, graphics, Piece.Red))
            {
                return CheckForWin(ConnectFour, this);
            }
            else if (PlayRandomly(ConnectFour, graphics))
            {
                return CheckForWin(ConnectFour, this);
            }
            return false;
        }

        public bool PlayRandomly(Board ConnectFour, BoardRenderer graphics)
        {
            return StrategyHelper.PlayRandomly(ConnectFour, graphics);
        }

        public bool CheckForWin(Board ConnectFour, Player p)
        {
            return StrategyHelper.CheckForWin(ConnectFour, this);
        }

        public bool CheckHorizontal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            return StrategyHelper.CheckHorizontal(ConnectFour, graphics, Piece.Yellow);
        }

        public bool CheckVertical(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            return StrategyHelper.CheckVertical(ConnectFour, graphics, Piece.Yellow);
        }

        // CheckDiagonal is not used for Medium Mode 
        public bool CheckDiagonal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            throw new NotImplementedException();
        }
    }

    public enum Piece
    {
        None,
        Red,
        Yellow
    }

    public abstract class Player
    {
        private Piece _p;
        private int _score;
        public Player(Piece p)
        {
            _p = p;
            _score = 0;
        }

        public int Score
        {
            get
            {
                return _score;
            }
        }

        public void IncrementScore()
        {
            _score++;
        }

        public abstract bool PlayPiece(Board ConnectFour, BoardRenderer graphics);
    }

    public class Program
    {

        public static void Main()
        {
            new Window("Connect Four: Reece Hodges", 1000, 800);
            new GameManager();

            do
            {
                GameManager.Instance.RunGame();
            } while (!SplashKit.WindowCloseRequested("Connect Four: Reece Hodges"));

        }
    }

    public enum ProgramState
    {
        Menu,
        Playing,
        Results
    }

    public interface Strategy
    {
        bool PlayPiece(Board ConnectFour, BoardRenderer graphics);
        bool PlayRandomly(Board ConnectFour, BoardRenderer graphics);
        bool CheckHorizontal(Board ConnectFour, BoardRenderer graphics, Piece p);
        bool CheckVertical(Board ConnectFour, BoardRenderer graphics, Piece p);
        bool CheckDiagonal(Board ConnectFour, BoardRenderer graphics, Piece p);

        bool CheckForWin(Board ConnectFour, Player p);
    }

    public static class StrategyHelper
    {
        public static bool PlayRandomly(Board ConnectFour, BoardRenderer graphics)
        {
            Random r1 = new Random(); // _rows
            Random r2 = new Random(); // _columns

            int randomRow = r1.Next(1, ConnectFour.Rows + 1);
            int randomColumn = r2.Next(1, ConnectFour.Columns + 1);

            if (ConnectFour.ValidMove(randomRow, randomColumn))
            {
                double X = ConnectFour.Board_X + ConnectFour.Board_Width / ConnectFour.Columns * (randomColumn - 0.5); // x pos.
                double Y = ConnectFour.Board_Y + ConnectFour.Board_Height / ConnectFour.Rows * (randomRow - 0.5); // y pos.

                graphics.DrawPiece(Piece.Yellow, X, Y);
                ConnectFour.GameBoardPieces[randomRow, randomColumn] = Piece.Yellow;

                return true;
            }

            return false;
        }

        public static bool CheckHorizontal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            for (int row = 0; row < ConnectFour.GameBoardPieces.GetLength(0); row++)
            {
                for (int column = 0; column < ConnectFour.GameBoardPieces.GetLength(1) - 3; column++)
                {
                    if (ConnectFour.GameBoardPieces[row, column] == p)
                    {
                        if (ConnectFour.GameBoardPieces[row, column] == ConnectFour.GameBoardPieces[row, column + 1])
                        {
                            if (ConnectFour.GameBoardPieces[row, column + 1] == ConnectFour.GameBoardPieces[row, column + 2])
                            {
                                if (ConnectFour.ValidMove(row, column + 3))
                                {
                                    double X = ConnectFour.Board_X + ConnectFour.Board_Width / ConnectFour.Columns * (column + 3 - 0.5); // x pos.
                                    double Y = ConnectFour.Board_Y + ConnectFour.Board_Height / ConnectFour.Rows * (row - 0.5); // y pos.

                                    graphics.DrawPiece(Piece.Yellow, X, Y);
                                    ConnectFour.GameBoardPieces[row, column + 3] = Piece.Yellow;

                                    return true;
                                }
                                else if (ConnectFour.ValidMove(row, column - 1))
                                {
                                    double X = ConnectFour.Board_X + ConnectFour.Board_Width / ConnectFour.Columns * (column - 1 - 0.5); // x pos.
                                    double Y = ConnectFour.Board_Y + ConnectFour.Board_Height / ConnectFour.Rows * (row - 0.5); // y pos.

                                    graphics.DrawPiece(Piece.Yellow, X, Y);
                                    ConnectFour.GameBoardPieces[row, column - 1] = Piece.Yellow;

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckVertical(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            for (int row = 0; row < ConnectFour.GameBoardPieces.GetLength(0) - 3; row++)
            {
                for (int column = 0; column < ConnectFour.GameBoardPieces.GetLength(1); column++)
                {
                    if (ConnectFour.GameBoardPieces[row, column] == p)
                    {
                        if (ConnectFour.GameBoardPieces[row, column] == ConnectFour.GameBoardPieces[row + 1, column])
                        {
                            if (ConnectFour.GameBoardPieces[row + 1, column] == ConnectFour.GameBoardPieces[row + 2, column])
                            {
                                if (ConnectFour.ValidMove(row + 3, column))
                                {
                                    double X = ConnectFour.Board_X + ConnectFour.Board_Width / ConnectFour.Columns * (column - 0.5); // x pos.
                                    double Y = ConnectFour.Board_Y + ConnectFour.Board_Height / ConnectFour.Rows * (row + 3 - 0.5); // y pos.

                                    graphics.DrawPiece(Piece.Yellow, X, Y);
                                    ConnectFour.GameBoardPieces[row + 3, column] = Piece.Yellow;

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckDiagonal(Board ConnectFour, BoardRenderer graphics, Piece p)
        {
            // negative slope
            for (int row = 0; row < ConnectFour.GameBoardPieces.GetLength(0) - 3; row++)
            {
                for (int column = 0; column < ConnectFour.GameBoardPieces.GetLength(1) - 3; column++)
                {
                    if (ConnectFour.GameBoardPieces[row, column] == p)
                    {
                        if (ConnectFour.GameBoardPieces[row, column] == ConnectFour.GameBoardPieces[row + 1, column + 1])
                        {
                            if (ConnectFour.GameBoardPieces[row + 1, column + 1] == ConnectFour.GameBoardPieces[row + 2, column + 2])
                            {
                                if (ConnectFour.ValidMove(row + 3, column + 3))
                                {
                                    double X = ConnectFour.Board_X + ConnectFour.Board_Width / ConnectFour.Columns * (column + 3 - 0.5); // x pos.
                                    double Y = ConnectFour.Board_Y + ConnectFour.Board_Height / ConnectFour.Rows * (row + 3 - 0.5); // y pos.

                                    graphics.DrawPiece(Piece.Yellow, X, Y);
                                    ConnectFour.GameBoardPieces[row + 3, column + 3] = Piece.Yellow;

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            // positive slope
            for (int row = 0; row < ConnectFour.GameBoardPieces.GetLength(0) - 3; row++)
            {
                for (int column = ConnectFour.GameBoardPieces.GetLength(1) - 1; column >= 3; column--)
                {
                    if (ConnectFour.GameBoardPieces[row, column] == p)
                    {
                        if (ConnectFour.GameBoardPieces[row, column] == ConnectFour.GameBoardPieces[row + 1, column - 1])
                        {
                            if (ConnectFour.GameBoardPieces[row + 1, column - 1] == ConnectFour.GameBoardPieces[row + 2, column - 2])
                            {
                                if (ConnectFour.ValidMove(row + 3, column - 3))
                                {
                                    double X = ConnectFour.Board_X + ConnectFour.Board_Width / ConnectFour.Columns * (column - 3 - 0.5); // x pos.
                                    double Y = ConnectFour.Board_Y + ConnectFour.Board_Height / ConnectFour.Rows * (row + 3 - 0.5); // y pos.

                                    graphics.DrawPiece(Piece.Yellow, X, Y);
                                    ConnectFour.GameBoardPieces[row + 3, column - 3] = Piece.Yellow;

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static bool CheckForWin(Board ConnectFour, BotPlayer p)
        {
            if (ConnectFour.CheckForVictory(ConnectFour.GameBoardPieces))
            {
                p.IncrementScore();
            }

            return !ConnectFour.IsFull();
        }
    }

}