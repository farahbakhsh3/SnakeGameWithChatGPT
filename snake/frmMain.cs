using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class frmMain : Form
    {
        private string desc = "Snake Game , Developed with ChatGPT  --- ";
        private static int snakeBlock = 15;
        private int screenWidth = snakeBlock * 40;
        private int screenHeight = snakeBlock * 30;
        private int foodX, foodY;
        private int score = 0;

        private List<Point> snakeList = new List<Point>();
        private string direction = "right";

        public frmMain()
        {
            InitializeComponent();
            ResetGame();
        }

        private void ResetGame()
        {
            snakeList.Clear();
            snakeList.Add(new Point(snakeBlock * 3, snakeBlock));
            snakeList.Add(new Point(snakeBlock * 2, snakeBlock));
            snakeList.Add(new Point(snakeBlock * 1, snakeBlock));

            direction = "right";

            GenerateFood();
            score = 0;

            this.Text = this.desc + " Score: " + score;
            tmrGame.Interval = 100;
            tmrGame.Start();
        }

        private void GenerateFood()
        {
            Random random = new Random();
            foodX = random.Next(0, screenWidth / snakeBlock) * snakeBlock;
            foodY = random.Next(0, screenHeight / snakeBlock) * snakeBlock;
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && direction != "left")
                direction = "right";
            else if (e.KeyCode == Keys.Left && direction != "right")
                direction = "left";
            else if (e.KeyCode == Keys.Up && direction != "down")
                direction = "up";
            else if (e.KeyCode == Keys.Down && direction != "up")
                direction = "down";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
            this.Invalidate();
        }

        private void MoveSnake()
        {
            Point head = snakeList.First();

            if (direction == "right")
                head.X += snakeBlock;
            else if (direction == "left")
                head.X -= snakeBlock;
            else if (direction == "up")
                head.Y -= snakeBlock;
            else if (direction == "down")
                head.Y += snakeBlock;

            if (head.X >= screenWidth)
                head.X = 0;
            else if (head.X < 0)
                head.X = screenWidth - snakeBlock;

            if (head.Y >= screenHeight)
                head.Y = 0;
            else if (head.Y < 0)
                head.Y = screenHeight - snakeBlock;

            snakeList.Insert(0, head);

            if (head.X == foodX && head.Y == foodY)
            {
                score += 10;
                this.Text = this.desc + " Score: " + score;
                tmrGame.Interval = Math.Max(tmrGame.Interval-2, 35);

                GenerateFood();
            }
            else
                snakeList.RemoveAt(snakeList.Count - 1);
        }

        private void CheckCollision()
        {
            Point head = snakeList.First();
            for (int i = 1; i < snakeList.Count; i++)
            {
                if (snakeList[i].X == head.X && snakeList[i].Y == head.Y)
                {
                    tmrGame.Stop();
                    MessageBox.Show("Game Over!");
                    ResetGame();
                    break;
                }
            }
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush boardBrush = Brushes.White;
            Brush snakeBrush = Brushes.Blue;
            Brush foodBrush = Brushes.Red;

            canvas.FillRectangle(boardBrush, 0, 0, screenWidth, screenHeight);
            foreach (Point point in snakeList)
                canvas.FillRectangle(snakeBrush, point.X, point.Y, snakeBlock, snakeBlock);
            canvas.FillRectangle(foodBrush, foodX, foodY, snakeBlock, snakeBlock);
        }
    }
}