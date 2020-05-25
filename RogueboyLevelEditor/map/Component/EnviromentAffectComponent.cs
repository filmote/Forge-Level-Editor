﻿using System.Drawing;
using Point = System.Drawing.Point;


namespace RogueboyLevelEditor.map.Component
{
    public class EnviromentAffectComponent : DrawComponent
    {
        Map parentMap;
        public Point Start;
        public Point End;
        public bool IsValid;
        public bool Highlight;

        public EnviromentAffectComponent(Point start, Point end, Map parent)
        {
            Start = start;
            End = end;
            parentMap = parent;

            UpdateValid();
        }

        void UpdateValid()
        {
            var p0 = parentMap.GetTile(Start);
            var p1 = parentMap.GetTile(End);
            this.IsValid = (TileManager.GetTile(p0.tileID).IsSender && TileManager.GetTile(p1.tileID).IsReceiver);
        }

        public override void Draw(Graphics graphics, Point Pos)
        {

            UpdateValid();

            Pen pen = new Pen(Color.LawnGreen);
            Point ScreenStart = new Point(parentMap.ToScreenSpaceX(Start.X), parentMap.ToScreenSpaceY(Start.Y));
            Point ScreenEnd = new Point(parentMap.ToScreenSpaceX(End.X), parentMap.ToScreenSpaceY(End.Y));
            graphics.DrawRectangle(pen, ScreenStart.X - 1, ScreenStart.Y - 1, 17, 17);
            pen.Color = Color.Red;
            graphics.DrawRectangle(pen, ScreenEnd.X - 1, ScreenEnd.Y - 1, 17, 17);

            if (this.Highlight)
            {
                pen = new Pen(Color.Red);
                this.Highlight = false;
            }
            else
            {
                pen = new Pen(Color.Blue);
            }

            pen.Width = 2;
            graphics.DrawLine(pen, ScreenStart.X + 8, ScreenStart.Y + 8, ScreenEnd.X + 8, ScreenEnd.Y + 8);

        }

    }

}
