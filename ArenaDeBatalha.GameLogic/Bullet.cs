using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaDeBatalha.GameLogic
{
    public class Bullet : GameObject
    {
        public Bullet(Size bounds, Graphics screenPainter, Point position) : base(bounds, screenPainter)
        {
            this.Speed = 20;
            this.Sound = Media.Missile;
            this.Left = position.X;
            this.Top = position.Y;
            this.PlaySound();
        }

        public override Bitmap GetSprite()
        {
            return Media.Bullet;
        }

        public override void UpdateObject()
        {
            this.MoveUp();
            base.UpdateObject();
        }
    }
}
