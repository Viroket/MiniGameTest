using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MiniGameTest.Shapes
{
    enum ShapeType
    {
        chase,
        escape,
        random,
    }

    abstract class Shape
    {
        public ShapeType type;
        protected int width;
        protected int height;
        protected int x;
        protected int y;
        protected int xSpeed;
        protected int ySpeed;
        protected System.Drawing.SolidBrush color;
        protected Random random = new Random();

        protected int xSpeedInit;
        protected int ySpeedInit;

        public abstract void UpdateSpeed(int score);
 
        public abstract void UpdatePosition(System.Drawing.Size clientSize, int mouseX, int mouseY);

        public virtual void Init(System.Drawing.Size clientSize)
        {
            width = random.Next(70, 100);
            height = random.Next(50, 60);
            x = random.Next(width, clientSize.Width - width);
            y = random.Next(height, clientSize.Height - height);
            xSpeed = random.Next(1, 3);
            ySpeed = random.Next(1, 3);
            xSpeedInit = xSpeed;
            ySpeedInit = ySpeed;
        }
        public abstract bool IsHitTarget(int mouseX, int mouseY);
        public abstract void PaintShape(PaintEventArgs e);

    }

    class Circle: Shape
    {
        int radius;
        public override void UpdateSpeed(int score) { }
        public override void Init(System.Drawing.Size clientSize)
        {
            base.Init(clientSize);
            type = ShapeType.random;
            height = width;
            radius = width / 2;
            x = random.Next(width, clientSize.Width - width);
            y = random.Next(height, clientSize.Height - height);
            color = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
        }

        public override void UpdatePosition(System.Drawing.Size clientSize, int mouseX, int mouseY)
        {
            x += xSpeed;
            y += ySpeed;
            int ballRandomMoveside = random.Next(0, 100);
            if (ballRandomMoveside <= 50)
            {
                ballRandomMoveside = -1;
            }
            else
            {
               ballRandomMoveside = 1;
            }
            if (x < 0 || x + width > clientSize.Width)
            {
                xSpeed = -xSpeed;
                ySpeed = ballRandomMoveside * ySpeed;
            }
            if (y < 0 || y + height > clientSize.Height)
            {
                ySpeed = -ySpeed;
                xSpeed = ballRandomMoveside * xSpeed;
            }
        }

        public override bool IsHitTarget(int mouseX, int mouseY)
        {
            return ((mouseX - (this.x + radius)) * (mouseX - (this.x + radius))) + ((mouseY - (this.y + radius)) * (mouseY - (this.y + radius))) <= (radius * radius);
        }

        public override void PaintShape(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(color, x, y, width, height);
            e.Graphics.DrawEllipse(System.Drawing.Pens.Black, x, y, width, height);
        }
    }

    class Rectangle: Shape
    {
        public override void UpdateSpeed(int score)
        {
            if (score >= 100)
            {
                xSpeed = xSpeedInit* score / 100;
                ySpeed = ySpeedInit* score / 100;
            }

        }

        public override void Init(System.Drawing.Size clientSize)
        {
            base.Init(clientSize);
            type = ShapeType.chase;
            color = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
        }

        public override void UpdatePosition(System.Drawing.Size clientSize, int mouseX, int mouseY)
        {
            if (mouseX < (x + (width / 2)) && (x + (width / 2)) > (width / 2))
            {
                x -= xSpeed;
            }
            if (mouseX > (x + (width / 2)) && (x + (width / 2)) < clientSize.Width - (width / 2))
            {
                x += xSpeed;
            }
            if (mouseY < (y + (height / 2)) && (y + (height / 2)) > (height / 2))
            {
                y -= ySpeed;
            }
            if (mouseY > (y + (height / 2)) && (y + (height / 2)) < clientSize.Height - (height / 2))
            {
                y += ySpeed;
            }
        }


        public override bool IsHitTarget(int mouseX, int mouseY)
        {
            return mouseX > this.x && mouseX < this.x + this.width && mouseY > this.y && mouseY < this.y + this.height;
        }


        public override void PaintShape(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(color, x, y, width, height);
            e.Graphics.DrawRectangle(System.Drawing.Pens.Black, x, y, width, height);
        }
    }

    class Square: Rectangle
    {
       
        public void RerollPosition(System.Drawing.Size clientSize)
        {
            x = random.Next(width, clientSize.Width - width);
            y = random.Next(width, clientSize.Height - width);
        }
       
        public override void Init(System.Drawing.Size clientSize)
        {
            base.Init(clientSize);
            type = ShapeType.escape;
            color = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            height = width;
        }

        public override void UpdatePosition(System.Drawing.Size clientSize, int mouseX, int mouseY)
        {
            if (mouseX < (x + (width / 2)) && (x + width) < clientSize.Width)
            {
                x += xSpeed;
            }
            if (mouseX > (x + (width / 2)) && (x) > 0)
            {
                x -= xSpeed;
            }
            if (mouseY < (y + (width / 2)) && (y + width) < clientSize.Height)
            {
                y += ySpeed;
            }
            if (mouseY > (y + (width / 2)) && (y) > 0)
            {
                y -= ySpeed;
            }
        }
    }
}
