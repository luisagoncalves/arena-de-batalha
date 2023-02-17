using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ArenaDeBatalha.GameLogic
{
    public class Enemy : GameObject
    {
        public Enemy(Size bounds, Graphics screenPainter, Point position) : base(bounds, screenPainter)
        {
            this.Left = position.X;
            this.Top = position.Y;
            this.Speed = 5;
            this.Sound = Media.exploshion_short;
        }

        public override Bitmap GetSprite()
        {
            return Media.Enemy;
        }

        public override void UpdateObject()
        {
            this.MoveDown();
            base.UpdateObject();
        }

    }
}
