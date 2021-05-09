using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGameTest
{
    public partial class Form1 : Form
    {
        int mouseX, mouseY, score = 0;

        private bool isGameRunning = false;

        private Shapes.Shape[] shapes = new Shapes.Shape[3];

        public Form1()
        {
            InitializeComponent();
            shapes[0] = new Shapes.Circle();
            shapes[1] = new Shapes.Rectangle();
            shapes[2] = new Shapes.Square();
            resetGame();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

        }

        private void resetGame()
        {
            foreach (Shapes.Shape shape in shapes)
            {
                shape.Init(this.ClientSize);
            }
            backMenuButton.Hide();
            mouseX = 0;
            mouseY = 0;
            label2.Text = "";
            scoreLable.Text = "";
        }

        private void ScoreCount(object sender, EventArgs e)
        {
            if (isGameRunning)
            {
                score += 1;
                label2.Text = "SCORE: " + score;
                // gameDifficulty();
                foreach (Shapes.Shape shape in shapes)
                {
                    shape.UpdateSpeed(score);
                }
            }
        }

        private void btn_start_MouseHover(object sender, EventArgs e)
        {
            btn_start.Image = Properties.Resources.start_hover;
        }

        private void btn_option_MouseHover(object sender, EventArgs e)
        {
            btn_option.Image = Properties.Resources.option_hover;
        }

        private void btn_exit_MouseHover(object sender, EventArgs e)
        {
            btn_exit.Image = Properties.Resources.exit_hover;
            // System.Media.SoundPlayer sound = new System.Media.SoundPlayer(@"C:\Users\Tal\source\repos\MiniGameTest\MiniGameTest\Resources\button_sound.wav");
            // sound.Play();
        }
        private void btn_start_MouseLeave(object sender, EventArgs e)
        {
            btn_start.Image = Properties.Resources.start_normal;
        }
        private void btn_option_MouseLeave(object sender, EventArgs e)
        {
            btn_option.Image = Properties.Resources.option_normal;
        }

        private void backToMainMenu(object sender, EventArgs e)
        {
            menu.Show();
            backMenuButton.Hide();
            scoreLable.Text = "";
            resetGame();
            this.Refresh();
        }

        private void btn_exit_MouseLeave(object sender, EventArgs e)
        {
            btn_exit.Image = Properties.Resources.exit_normal;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            menu.Hide();
            // Start Game
            isGameRunning = true;
            timer2.Start();
            // gameManager = new GameManager();
        }

        private void endGame()
        {
            // menu.Show();
            // Start Game
            backMenuButton.Show();
            scoreLable.Text = "TOTAL SCORE: " + score;
            isGameRunning = false;

            score = 0;
            label2.Text = "";
            this.Refresh();
        }

        private void btn_option_Click(object sender, EventArgs e)
        {
            option_page option = new option_page();
            option.ShowDialog();
        }
        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void PaintShape(object sender, PaintEventArgs e)
        {
            if (isGameRunning)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.Clear(this.BackColor);
                foreach (Shapes.Shape shape in shapes)
                {
                    shape.PaintShape(e);
                }
            }
        }

        private void HitTarget(Shapes.ShapeType type)
        {
            switch(type)
            {
                case Shapes.ShapeType.escape:
                    AddScore();
                    break;

                case Shapes.ShapeType.random:
                    endGame();
                    break;

                case Shapes.ShapeType.chase:
                    endGame();
                    break;
            }
        }

        private void UpdateObjects(object sender, EventArgs e)
        {
            // Update Cordinate
            if (isGameRunning)
            {
                foreach (Shapes.Shape shape in shapes)
                {
                    shape.UpdatePosition(ClientSize, mouseX, mouseY);
                    // allow one second before hit take place
                    if (shape.IsHitTarget(mouseX, mouseY) && score >= 1)
                    {
                        if (shape.type == Shapes.ShapeType.escape)
                        {
                            ((Shapes.Square)shape).RerollPosition(ClientSize);
                        }
                        HitTarget(shape.type);
                    }
                }
                // Update Painting
                this.Refresh(); // this will refresh the window
            }
        }

        private void AddScore()
        {
            score += 5;
            label2.Text = "SCORE: " + score;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }
    }

    public class Button
    {
        public EventHandler ClickEvent;
        public EventHandler MouseHover;
        public void OnClick()
        {
            ClickEvent.Invoke(this, EventArgs.Empty);
        }
        public void OnHover()
        {
            MouseHover.Invoke(this, EventArgs.Empty);
        }
    }
}
