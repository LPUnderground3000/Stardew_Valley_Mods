﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardustCore.UIUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.Objects
{
    public class MultiTileObject : CoreObject
    {
        public List<KeyValuePair<Vector2,MultiTileComponent>> objects;
        public Color categoryColor;
        public String categoryName;

        public MultiTileObject(String Name, String Description,Vector2 tile, Texture2DExtended texture, List<KeyValuePair<Vector2, MultiTileComponent>> Objects, Color CategoryColor, String CategoryName) :base(texture,0,tile,0)
        {
            this.objects = Objects;
            this.TextureSheet = texture;
            this.texturePath = texture.path;
            this.categoryColor = CategoryColor;
            this.categoryName = CategoryName;
            this.name = Name;
            this.displayName = Name;
            this.description = Description;

            foreach(var v in this.objects)
            {
                v.Value.containerObject = this;
            }

            InitializeBasics(0, tile);
            this.serializationName = this.GetType().ToString();

            this.defaultSourceRect = new Rectangle(0, 0, 16, 16);
            this.sourceRect = defaultSourceRect;
        }

        public MultiTileObject(String Name, String Description, Vector2 tile, Animations.AnimationManager animationManager, List<KeyValuePair<Vector2, MultiTileComponent>> Objects, Color CategoryColor, String CategoryName)
        {
            this.animationManager = animationManager;
            this.objects = Objects;
            this.TextureSheet =animationManager.getExtendedTexture();
            this.texturePath = animationManager.getExtendedTexture().path;
            this.name = Name;
            this.displayName = Name;
            this.description = Description;
            InitializeBasics(0, tile);
            this.serializationName = this.GetType().ToString();
        }

        public void RemoveAllObjects()
        {
            if (Game1.player.isInventoryFull() == false){
                foreach (var v in this.objects)
                {
                    v.Value.performRemoveAction(v.Value.TileLocation, v.Value.thisLocation);
                }
                Game1.player.addItemToInventory(this);
            }
            return;
        }

        public override Type getCustomType()
        {
            return this.GetType();
        }

        public override string GetSerializationName()
        {
            return typeof(MultiTileObject).ToString();
        }

        public override void InitializeBasics(int InvMaxSize, Vector2 tile)
        {
            base.InitializeBasics(InvMaxSize, tile);
        }
        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            if (canBePlacedHere(location,new Vector2(x/Game1.tileSize,y/Game1.tileSize)))
            {
                foreach (var pair in this.objects)
                {
                    pair.Value.placementAction(location, x + (int)(pair.Key.X * Game1.tileSize), y + (int)(pair.Key.Y * Game1.tileSize), who);
                }
                return true;
            }
            return false;
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            bool canBePlaced = true;
            foreach (var v in this.objects)
            {
                canBePlaced=v.Value.canBePlacedHere(l, tile+v.Key);
                if (canBePlaced == false) return false;
            }
            return true;
        }

        public override bool clicked(Farmer who)
        {
            foreach (var pair in this.objects)
            {
                pair.Value.clicked(who);
            }
            return true;
        }

        public override bool RightClicked(Farmer who)
        {
            foreach (var pair in this.objects)
            {
                pair.Value.RightClicked(who);
            }
            return true;
        }

        /// <summary>
        /// Need to fix this for sure!!!
        /// </summary>
        /// <returns></returns>
        public override Item getOne()
        {
            List<KeyValuePair<Vector2, MultiTileComponent>> items = new List<KeyValuePair<Vector2, MultiTileComponent>>();
            foreach (var pair in this.objects)
            {
                items.Add(new KeyValuePair<Vector2, MultiTileComponent>(pair.Key, (pair.Value.getOne()) as MultiTileComponent));
            }
            if (this.animationManager != null)
            {
                return new MultiTileObject(this.name, this.description, this.TileLocation, this.animationManager, items, this.categoryColor, this.categoryName);
                //throw new NotImplementedException();
            }
            else
            {
                return new MultiTileObject(this.name, this.description, this.TileLocation, this.TextureSheet, items, this.categoryColor, this.categoryName);
            }
            return null;
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            
            foreach(var v in this.objects)
            {
                    v.Value.draw(spriteBatch, (int)x+(int)(v.Key.X), (int)y+(int)(v.Key.Y), alpha);
            }
            
            //base.draw(spriteBatch, x, y, alpha);
        }

        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            foreach (var v in this.objects)
            {
                v.Value.draw(spriteBatch, (int)xNonTile+(int)(v.Key.X*Game1.tileSize), (int)yNonTile+ (int)(v.Key.Y * Game1.tileSize), layerDepth, alpha);
            }
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, StardewValley.Farmer f)
        {
            if (animationManager == null)
            {
                foreach (var v in this.objects)
                {
                    spriteBatch.Draw(v.Value.getExtendedTexture().getTexture(), objectPosition + new Vector2(v.Key.X * Game1.tileSize, v.Key.Y * Game1.tileSize), this.sourceRect, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                }
            }
            else
            {
                foreach (var v in this.objects)
                {
                    spriteBatch.Draw(v.Value.animationManager.getTexture(), objectPosition + new Vector2(v.Key.X * Game1.tileSize, v.Key.Y * Game1.tileSize), this.sourceRect, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                }
            }

            //base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        public override void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1)
        {
            foreach (var v in this.objects)
            {
                v.Value.drawAtNonTileSpot(spriteBatch, location+new Vector2(v.Key.X * Game1.tileSize, v.Key.Y * Game1.tileSize), layerDepth, alpha);
            }
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color c, bool drawShadows)
        {
            if (animationManager == null) spriteBatch.Draw(this.TextureSheet.getTexture(), location,this.defaultSourceRect, Color.White * transparency, 0f, new Vector2(0,0), scaleSize, SpriteEffects.None, layerDepth);
            else
            {
                spriteBatch.Draw(animationManager.getTexture(), location, animationManager.currentAnimation.sourceRectangle, Color.White * transparency, 0f, new Vector2(0,0), scaleSize, SpriteEffects.None, layerDepth);
                //this.modularCrop.drawInMenu(spriteBatch, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), Color.White, 0f,true);
                if (Game1.player.CurrentItem != this) animationManager.tickAnimation();
            }
        }

        public override Color getCategoryColor()
        {
            return this.categoryColor;
        }

        public override string getCategoryName()
        {
            return this.name;
        }


    }
}
