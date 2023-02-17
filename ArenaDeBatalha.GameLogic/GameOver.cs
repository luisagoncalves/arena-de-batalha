using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaDeBatalha.GameLogic
{
    public class GameOver : GameObject
    {
        public GameOver(Size bounds, Graphics ScreenPainter) : base(bounds, ScreenPainter)
        {
            this.Left = this.Bounds.Width / 2 - this.Width /2;
            this.Top = this.Bounds.Height / 2 - this.Height /2;
            this.Speed = 0;
        }

        public override Bitmap GetSprite()
        {
            return Media.GameOver;
        }
    }
}
