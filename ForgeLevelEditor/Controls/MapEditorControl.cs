﻿using System;
using System.Drawing;
using System.Windows.Forms;

using ForgeLevelEditor.map;
using ForgeLevelEditor.map.Component;
using ForgeLevelEditor.mapCollection;
using ForgeLevelEditor.Tools;
using ForgeLevelEditor.TextureHandler;

namespace ForgeLevelEditor.Controls
{
    public class MapEditorControl : Control
    {
        public event EventHandler<TileChangedEventArgs> TileChanged;
        public event EventHandler<TileSelectedEventArgs> TileSelected;
        public event EventHandler<SpriteAddedEventArgs> SpriteAdded;
        public event EventHandler<SingleActionEventArgs> SingleActionComplete;

        private MapCollection mapCollection = new MapCollection();
        private ITool<MapEditorControl> tool = null;
        private Point? tileCursor = null;
        private int selectedTileId = -1;
        private int selectedSpriteId = -1;

        public MapEditorControl()
        {
            this.DoubleBuffered = true;
        }

        public MapCollection MapCollection
        {
            get => this.mapCollection;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("MapCollection");

                this.mapCollection = value;
            }
        }

        public Map CurrentMap
        {
            get => this.mapCollection.CurrentMap;
            set => this.mapCollection.CurrentMap = value;
        }

        public ITool<MapEditorControl> Tool
        {
            get => this.tool;
            set
            {
                if (this.tool != null)
                    this.tool.Detach(this);

                this.tool = value;

                if (this.tool != null)
                    this.tool.Attach(this);
            }
        }

        public int SelectedTileId
        {
            get => this.selectedTileId;
            set
            {
                this.selectedTileId = value;
                this.Invalidate();
            }
        }

        public int SelectedSpriteId
        {
            get => selectedSpriteId;
            set
            {
                this.selectedSpriteId = value;
                this.Invalidate();
            }
        }

        public Point? TileCursor
        {
            get => this.tileCursor;
            set
            {
                this.tileCursor = value;
                this.Invalidate();
            }
        }

        private void DrawSelectedTile(Graphics graphics)
        {
            var textureId = TileManager.GetTile(this.SelectedTileId).TextureID;
            var bitmap = TextureManager.GetTexture(textureId);

            // This colour seems arbitrary < not really, its the pink colour in the Pico8 palette but should probably be specified somewhere.
            bitmap.MakeTransparent(Color.FromArgb(255, 119, 168));

            graphics.DrawImage(bitmap, 8, 8, 16, 16);
        }

        private void DrawTileCursor(Graphics graphics)
        {
            if (this.TileCursor.HasValue)
                graphics.DrawRectangle(Pens.Red, this.TileCursor.Value.X - 1, this.TileCursor.Value.Y - 1, this.CurrentMap.TileSize.Width, this.CurrentMap.TileSize.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.mapCollection?.Draw(e.Graphics);

            this.DrawSelectedTile(e.Graphics);
            this.DrawTileCursor(e.Graphics);

            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var map = this.MapCollection?.CurrentMap;

            if (map != null)
                this.TileCursor = map.ToScreenSpace(map.ToTileSpace(e.Location));
        }

        private const int scrollScale = 2;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0)
            {
                var map = this.MapCollection?.CurrentMap;

                if (map != null)
                {
                    var delta = (Math.Sign(e.Delta) * scrollScale);

                    var width = Math.Min(Math.Max(4, (map.TileSize.Width + delta)), 64);
                    var height = Math.Min(Math.Max(4, (map.TileSize.Height + delta)), 64);
                    map.TileSize = new Size(width, height);

                    this.TileCursor = map.ToScreenSpace(map.ToTileSpace(e.Location));

                    this.Invalidate();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this.Focus();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.TileCursor = null;
        }

        public void ChangeTile(Point location) {

            Point point = new Point();
            point.X = this.CurrentMap.ToTileSpaceX(location.X);
            point.Y = this.CurrentMap.ToTileSpaceY(location.Y);

            int origTileId = this.CurrentMap.GetTile(point).tileID;
            Tile origTile = TileManager.GetTile(origTileId);
            Tile newTile = TileManager.GetTile(this.SelectedTileId);

            TileChangedEventArgs eventArgs = new TileChangedEventArgs();
            eventArgs.OrigTile = origTile;
            eventArgs.NewTile = newTile;
            eventArgs.Location = point;

            EventHandler<TileChangedEventArgs> handler = TileChanged;
            handler?.Invoke(this, eventArgs);

        }


        public void SelectTile(Point location) {

            Point point = new Point();
            point.X = this.CurrentMap.ToTileSpaceX(location.X);
            point.Y = this.CurrentMap.ToTileSpaceY(location.Y);

            Tile newTile = TileManager.GetTile(this.SelectedTileId);

            TileSelectedEventArgs eventArgs = new TileSelectedEventArgs();
            eventArgs.Tile = newTile;
            eventArgs.Location = point;

            EventHandler<TileSelectedEventArgs> handler = TileSelected;
            handler?.Invoke(this, eventArgs);

        }


        public void AddSprite(Point location, Sprite sprite) {

            Point point = new Point();
            point.X = this.CurrentMap.ToTileSpaceX(location.X);
            point.Y = this.CurrentMap.ToTileSpaceY(location.Y);

            SpriteAddedEventArgs eventArgs = new SpriteAddedEventArgs();
            eventArgs.Sprite = sprite;
            eventArgs.Location = point;

            EventHandler<SpriteAddedEventArgs> handler = SpriteAdded;
            handler?.Invoke(this, eventArgs);

        }


        public void CompleteSingleAction(object tool) {
            
            SingleActionEventArgs eventArgs = new SingleActionEventArgs();
            eventArgs.Tool = tool;

            EventHandler<SingleActionEventArgs> handler = SingleActionComplete;
            handler?.Invoke(this, eventArgs);

        }

    }

}
