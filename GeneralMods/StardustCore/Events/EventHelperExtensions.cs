using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace StardustCore.Events
{
    public static class EventHelperExtensions
    {
        /// <summary>
        /// Creates the event string to add the object to the player's inventory.
        /// </summary>
        /// <returns></returns>
        public static void addObjectToPlayersInventory(this EventHelper EventHelper,int ParentSheetIndex, int Amount=1, bool MakeActiveObject=false)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.EventFramework.AddObjectToPlayersInventory ");
            b.Append(ParentSheetIndex);
            b.Append(" ");
            b.Append(Amount);
            b.Append(" ");
            b.Append(MakeActiveObject);
            EventHelper.add(b);
        }

        public static void ViewportLerp(this EventHelper EventHelper, Point NewPosition, double Speed)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.EventFramework.ViewportLerp ");
            b.Append(NewPosition.X);
            b.Append(" ");
            b.Append(NewPosition.Y);
            b.Append(" ");
            b.Append(Speed);
            EventHelper.add(b);

        }

        public static void ViewportLerpTile(this EventHelper EventHelper, Point NewTilePosition, double Speed)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.EventFramework.ViewportLerp ");
            b.Append(NewTilePosition.X*Game1.tileSize);
            b.Append(" ");
            b.Append(NewTilePosition.Y*Game1.tileSize);
            b.Append(" ");
            b.Append(Speed);
            EventHelper.add(b);

        }

        /// <summary>
        /// Lerps the camera an offset tile amount.
        /// </summary>
        /// <param name="EventHelper"></param>
        /// <param name="NewTilePositionOffset"></param>
        /// <param name="Speed">How many frames (aka update ticks) it takes to finish. Aka 60~=1 second</param>
        public static void ViewportLerpTileOffset(this EventHelper EventHelper, Point NewTilePositionOffset, int Frames=60)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Omegasis.EventFramework.ViewportLerp ");
            b.Append((NewTilePositionOffset.X * Game1.tileSize));
            b.Append(" ");
            b.Append((NewTilePositionOffset.Y * Game1.tileSize));
            b.Append(" ");
            b.Append(Frames);
            EventHelper.add(b);

        }


    }
}
