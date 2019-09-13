using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DebugMeow.Games.Tetris
{
    public partial class TetrisForm : Form
    {
        private const int Height = 20;
        private const int Width = 10;
        private const int PixelWidth = 30;

        private const int UpdateSpeedInMilliseconds = 500;
        private const int MoveSpeedInMilliseconds = 100;
        private const int RotateSpeedInMilliseconds = 300;

        public const int KEY_PRESSED = 0x8000;

        private readonly Point _startPosition = new Point(5, 0);
        private readonly Timer _timer;
        private readonly Brush _backgroundBrush;

        private readonly Brush[,] _grid;
        private Brush[] _palette;
        private Block[] _blocks;

        private Block _currentBlock;
        private Point _currentPosition;

        private int _score;

        private Random _rnd = new Random();

        private Stopwatch _lastUpdate = new Stopwatch();
        private Stopwatch _lastMove = new Stopwatch();
        private Stopwatch _lastRotate = new Stopwatch();

        public TetrisForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            _grid = new Brush[Width, Height];

            _timer = new Timer { Interval = 16 };
            _timer.Tick += TimerTick;

            _backgroundBrush = new SolidBrush(BackColor);

            InitPalette();
            InitShapes();

            ClearGrid();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int keyCode);

        public static bool IsKeyDown(Keys key)
        {
            return Convert.ToBoolean(GetKeyState((int)key) & KEY_PRESSED);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            UpdateGame();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            ClearGrid();
            UpdateScore(0);
            _lastUpdate.Restart();
            _lastMove.Restart();
            _lastRotate.Restart();
            _currentBlock = null;
            UpdateGame();
            _timer.Start();
        }

        private void UpdateScore(int score)
        {
            _score = score;
            LabelScore.Text = score.ToString();
        }

        private bool MoveBlock(int offsetX, int offsetY)
        {
            // Bug: don't start the _lastMove stopwatch
            // It demonstrates the use of a conditional breakpoint on offset

            if (offsetX == 0 && offsetY == 0)
            {
                return false;
            }

            if (_lastMove.ElapsedMilliseconds < MoveSpeedInMilliseconds)
            {
                return false;
            }

            _lastMove.Restart();

            _currentPosition.X += offsetX;
            _currentPosition.Y += offsetY;

            foreach (var coordinate in _currentBlock.GetCoordinates())
            {
                if (IntersectsWithSomething(_currentPosition + coordinate))
                {
                    _currentPosition.X -= offsetX;
                    _currentPosition.Y -= offsetY;
                    return false;
                }
            }

            return true;
        }

        private bool RotateBlock()
        {
            if (_lastRotate.ElapsedMilliseconds < RotateSpeedInMilliseconds)
            {
                return false;
            }

            _lastRotate.Restart();

            int newRotation = _currentBlock.Rotation + 1 % 4;

            foreach (var point in _currentBlock.GetCoordinates(newRotation))
            {
                if (IntersectsWithSomething(_currentPosition + point))
                {
                    return false;
                }
            }

            _currentBlock.Rotation = newRotation;

            return true;
        }

        private void UpdateGame()
        {
            bool needRefresh = false;

            if (_currentBlock != null)
            {
                int offsetX = 0;
                int offsetY = 0;

                if (IsKeyDown(Keys.Left))
                {
                    offsetX = -1;
                }
                else if (IsKeyDown(Keys.Right))
                {
                    offsetX = 1;
                }
                else if (IsKeyDown(Keys.Down))
                {
                    offsetY = 1;
                }
                else if (IsKeyDown(Keys.Up))
                {
                    needRefresh = RotateBlock();
                }

                needRefresh = needRefresh || MoveBlock(offsetX, offsetY);
            }

            if (_lastUpdate.ElapsedMilliseconds > UpdateSpeedInMilliseconds)
            {
                if (_currentBlock == null)
                {
                    _currentBlock = _blocks[_rnd.Next(0, _blocks.Length)];
                    _currentPosition = _startPosition - (0, _currentBlock.MaxHeight);
                }
                else
                {
                    _currentPosition.Y++;

                    // Check for collisions
                    foreach (var coordinate in _currentBlock.GetCoordinates())
                    {
                        // Bug: remove the sum
                        // Interesting use of tracepoints
                        if (IntersectsWithSomething(coordinate + _currentPosition))
                        {
                            if (!DropBlock(_currentPosition + (0, -1), _currentBlock))
                            {
                                GameOver();
                                return;
                            }

                            _currentBlock = null;
                            break;
                        }
                    }
                }

                needRefresh = true;
                _lastUpdate.Restart();
            }

            if (needRefresh)
            {
                GameArea.Invalidate();
            }
        }

        private void GameOver()
        {
            _timer.Stop();
            MessageBox.Show("Game over");
        }

        private bool IntersectsWithSomething(Point point)
        {
            if (point.X < 0 || point.X >= Width || point.Y >= Height)
            {
                return true;
            }

            if (point.Y >= 0 && _grid[point.X, point.Y] != _backgroundBrush)
            {
                return true;
            }

            return false;
        }

        private bool DropBlock(Point position, Block block)
        {
            foreach (var point in block.GetCoordinates())
            {
                if (point.Y + position.Y < 0)
                {
                    // Not enough room
                    return false;
                }

                _grid[point.X + position.X, point.Y + position.Y] = block.Brush;
            }

            // Check if there's a line
            var linesToClear = new Stack<int>();

            for (int y = 0; y < Height; y++)
            {
                bool line = true;

                for (int x = 0; x < Width; x++)
                {
                    if (_grid[x, y] == _backgroundBrush)
                    {
                        line = false;
                        break;
                    }
                }

                if (line)
                {
                    linesToClear.Push(y);
                }
            }

            if (linesToClear.Count > 0)
            {
                UpdateScore(_score + linesToClear.Count * linesToClear.Count * 100);
                ClearLines(linesToClear);
            }

            return true;
        }

        private void ClearLines(Stack<int> lines)
        {
            int offset = 0;

            // Start from the bottom
            for (int y = Height - 1; y >= 0; y--)
            {
                while (lines.Count > 0 && lines.Peek() == y - offset)
                {
                    offset++;
                    lines.Pop();
                }

                if (offset == 0)
                {
                    continue;
                }

                // Push the top lines down
                for (int x = 0; x < Width; x++)
                {
                    _grid[x, y] = y - offset < 0 ? _backgroundBrush : _grid[x, y - offset];
                }
            }
        }

        private void FillRectangle(Graphics graphics, Brush brush, int x, int y)
        {
            graphics.FillRectangle(brush, x * PixelWidth, y * PixelWidth, PixelWidth, PixelWidth);

            if (brush != _backgroundBrush)
            {
                graphics.DrawRectangle(Pens.Black, x * PixelWidth, y * PixelWidth, PixelWidth, PixelWidth);
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _grid[i, j] = _palette[0];
                }
            }
        }

        private void TetrisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
        }

        private void GameArea_Paint(object sender, PaintEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            e.Graphics.Clear(BackColor);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (_grid[i, j] != _backgroundBrush)
                    {
                        FillRectangle(e.Graphics, _grid[i, j], i, j);
                    }
                }
            }

            if (_currentBlock != null)
            {
                foreach (var point in _currentBlock.GetCoordinates())
                {
                    if (_currentPosition.Y + point.Y < 0)
                    {
                        // Block is partially outside screen
                        continue;
                    }

                    FillRectangle(e.Graphics, _currentBlock.Brush, _currentPosition.X + point.X, _currentPosition.Y + point.Y);
                }
            }
        }

        private void InitPalette()
        {
            _palette = new[]
            {
                _backgroundBrush,
                Brushes.LightBlue,
                Brushes.LightSalmon,
                Brushes.LightGreen,
                Brushes.MediumPurple,
                Brushes.Coral,
                Brushes.Pink,
                Brushes.Magenta
            };
        }

        private void InitShapes()
        {
            _blocks = new Block[7];

            _blocks[0] = new Block( // L
                _palette[1],
                new[] { (0, 0), (1, 0), (0, 1), (0, 2) });

            _blocks[1] = new Block( // S
                _palette[2],
                new[] { (1, 0), (0, 1), (1, 1), (0, 2) });

            _blocks[2] = new Block( // Mirrored L
                _palette[3],
                new[] { (0, 0), (1, 0), (1, 1), (1, 2) });

            _blocks[3] = new Block( // Z
                _palette[4],
                new[] { (0, 0), (0, 1), (1, 1), (1, 2) });

            _blocks[4] = new Block( // Line
                _palette[5],
                new[] { (0, 0), (0, 1), (0, 2), (0, 3) });

            _blocks[5] = new Block( // Square
                _palette[6],
                new[] { (0, 0), (0, 1), (1, 0), (1, 1) });

            _blocks[6] = new Block( // T
                _palette[7],
                new[] { (1, 0), (0, 1), (1, 1), (2, 1) });
        }
    }
}
