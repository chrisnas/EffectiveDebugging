using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DebugMeow.Games.Tetris
{
    public class Block
    {
        private readonly Point[] _coordinates;

        private int _maxWidth;

        public Block(Brush brush, (int x, int y)[] coordinates)
        {
            Brush = brush;
            _coordinates = coordinates.Select(p => (Point)p).ToArray();

            MaxHeight = _coordinates.Max(c => c.Y);
            _maxWidth = _coordinates.Max(c => c.X);
        }

        public int Rotation { get; set; }

        public Brush Brush { get; }

        public int MaxHeight { get; }

        public IEnumerable<Point> GetCoordinates() => GetCoordinates(Rotation);

        public IEnumerable<Point> GetCoordinates(int rotation)
        {
            if (rotation == 0)
            {
                foreach (var point in _coordinates)
                {
                    yield return point;
                }

                yield break;
            }

            foreach (var point in GetCoordinates(rotation - 1))
            {
                //yield return (point.Y, _maxWidth - point.X);
                yield return (MaxHeight - point.Y, point.X);
            }
        }
    }
}