using ArenaDeBatalha.GameLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace ArenaDeBatalha.GUI
{
    public partial class FormPrincipal : Form
    {
        DispatcherTimer GameLoopTimer { get; set; }
        DispatcherTimer EnemySpawnTimer { get; set; }
        List<GameObject> GameObjects { get; set; }
        Bitmap ScreenBuffer { get; set; }
        Graphics ScreenPainter { get; set; }
        Background Background { get; set; }
        Player Player { get; set; }
        GameOver GameOver { get; set; }
        public Random Random { get; set; }
        bool CanShoot;

        public FormPrincipal()
        {
            InitializeComponent();
            this.Random = new Random();
            this.ClientSize = Media.Background.Size;
            this.ScreenBuffer = new Bitmap(Media.Background.Width, Media.Background.Height);
            this.ScreenPainter = Graphics.FromImage(this.ScreenBuffer);
            this.GameObjects = new List<GameObject>();
            this.Background = new Background(this.ScreenBuffer.Size, this.ScreenPainter);
            this.Player = new Player(this.ScreenBuffer.Size, this.ScreenPainter);
            this.GameOver = new GameOver(this.ScreenBuffer.Size, this.ScreenPainter);

            this.GameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.GameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66666);
            this.GameLoopTimer.Tick += GameLoop;

            this.EnemySpawnTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.EnemySpawnTimer.Interval = TimeSpan.FromMilliseconds(1000);
            this.EnemySpawnTimer.Tick += SpawnEnemy;

            this.GameObjects.Add(Background);
            this.GameObjects.Add(Player);

            StartGame();
        }
        public void StartGame()
        {
            this.GameObjects.Clear();
            this.GameObjects.Add(Background);
            this.GameObjects.Add(Player);
            this.Player.StartPosition();
            this.Player.Active = true;
            this.GameLoopTimer.Start();
            this.EnemySpawnTimer.Start();
            this.CanShoot = true;
        }
        public void EndGame() 
        {
            this.GameObjects.Clear();
            this.GameLoopTimer.Stop();
            this.EnemySpawnTimer.Stop();
            this.GameObjects.Add(Background);
            this.GameObjects.Add(GameOver);
            this.Background.UpdateObject();
            this.GameOver.UpdateObject();
            Invalidate();
        }
        public void GameLoop(object sender, EventArgs e) 
        {
            this.GameObjects.RemoveAll(x => !x.Active);

            this.ProcessControls();

            foreach (GameObject go in this.GameObjects)
            {
                go.UpdateObject();

                if (go.IsOutOfBounds())
                {
                    go.Destroy();
                }                

                if (go is Enemy)
                {
                    if (go.IsCollidingWith(Player))
                    {
                        Player.Destroy();
                        Player.PlaySound();
                        EndGame();
                        return;
                    }
                    foreach (GameObject bullet in this.GameObjects.Where(x => x is Bullet))
                    {
                        if (go.IsCollidingWith(bullet))
                        {
                            go.Destroy();
                            bullet.Destroy();
                        }
                    }
                }
            }
            this.Invalidate();
        }
        public void SpawnEnemy(object sender, EventArgs e)      
        {
            Point enemyPosition = new Point(this.Random.Next(2, this.ScreenBuffer.Width -64), -62);
            Enemy enemy = new Enemy(this.ScreenBuffer.Size, this.ScreenPainter, enemyPosition);
            this.GameObjects.Add(enemy);
        }
        private void FormPrincipal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(this.ScreenBuffer, 0, 0);
        }

        private void ProcessControls()
        {
            if (Keyboard.IsKeyDown(Key.Left)) Player.MoveLeft();
            if (Keyboard.IsKeyDown(Key.Right)) Player.MoveRight();
            if (Keyboard.IsKeyDown(Key.Up)) Player.MoveUp();
            if (Keyboard.IsKeyDown(Key.Down)) Player.MoveDown();
            if (Keyboard.IsKeyDown(Key.Space) && CanShoot)
            {
                this.GameObjects.Add(Player.Shoot());
                CanShoot = false;
            }
            if (Keyboard.IsKeyUp(Key.Space)) CanShoot = true;
        }

        private void FormPrincipal_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                StartGame();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
