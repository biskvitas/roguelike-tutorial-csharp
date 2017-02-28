﻿using SadConsole;
using Microsoft.Xna.Framework;
using SadConsole.Game;
using SadConsole.Consoles;
using System;
using System.Collections.Generic;
using roguelike.Entities;
using RogueSharp.DiceNotation;
using roguelike.Core;
using roguelike.Systems;
using roguelike.Entities.Monsters;

namespace roguelike.Consoles
{
    public class MapConsole : SadConsole.Consoles.Console
    {
        RogueSharp.FieldOfView rogueFOV;
        MapObjects.MapObjectBase[,] mapData;
        public Player Player { get; private set; }
        public List<Monster> Monsters { get; private set; }

        RogueSharp.Map rogueMap;
        private DungeonMap detailedMap;

        IReadOnlyCollection<RogueSharp.Cell> previousFOV = new List<RogueSharp.Cell>();

        public MapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight): base(mapWidth, mapHeight)
        {
            TextSurface.RenderArea = new Rectangle(0, 0, viewWidth, viewHeight);
            Player = new Player();           
            GenerateMap();
        }

        bool runOnce = true; // TODO: carry on thinking for a proper fix
        public override void Render()
        {
            if(runOnce) { GameWorld.DungeonScreen.StatsConsole.DrawPlayerStats(Player); runOnce = false; }

            base.Render();
            Player.Render();
            Monsters.ForEach(m =>
            {
                if (textSurface.RenderArea.Contains(m.Position)) { m.Render(); }
            });
        }

        public override void Update()
        {
            base.Update();
            Player.Update();
            Monsters.ForEach(m =>
            {
                m.RenderOffset = Position - textSurface.RenderArea.Location;
                m.Update();
            });
        }

        public void MovePlayerBy(Point amount)
        {
            // Get the position the player will be at
            Point newPosition = Player.Position + amount;

            // TODO: might need to add additional check to avoid drawing in inventory
            if (new Rectangle(0, 0, Width, Height).Contains(newPosition) && rogueMap.IsWalkable(newPosition.X, newPosition.Y))
            {
                Player.Position += amount;
                // TODO: fix this possitioning horror
                TextSurface.RenderArea = new Rectangle(Player.Position.X - (TextSurface.RenderArea.Width / 2),
                                                        Player.Position.Y - (TextSurface.RenderArea.Height / 2),
                                                        TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);

                // If he view area moved, we'll keep our entity in sync with it.
                Player.RenderOffset = Position - TextSurface.RenderArea.Location;


                // Erase status on old FOV
                foreach (var cell in previousFOV)
                    mapData[cell.X, cell.Y].RemoveCellFromView(this[cell.X, cell.Y]);
                // Calculate the new FOV
                previousFOV = rogueFOV.ComputeFov(Player.Position.X, Player.Position.Y, 10, true);
                // Set status on new FOV
                foreach (var cell in previousFOV)
                {
                    rogueMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    mapData[cell.X, cell.Y].RenderToCell(this[cell.X, cell.Y], true, rogueMap.GetCell(cell.X, cell.Y).IsExplored);
                }
            }
        }



        private void GenerateMap()
        {
            // Create the map
            //RogueSharp.MapCreation.IMapCreationStrategy<RogueSharp.Map> mapCreationStrategy
            //    = new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy<RogueSharp.Map>(Width, Height, 100, 20, 7);
            //rogueMap = RogueSharp.Map.Create(mapCreationStrategy);
            MapGenerator mapGenerator = new MapGenerator(Width, Height, 50, 12, 6);

            // TODO: I don't think I should have two maps
            detailedMap = mapGenerator.CreateMap();
            rogueMap = detailedMap;

            rogueFOV = new RogueSharp.FieldOfView(rogueMap);
            mapData = new MapObjects.MapObjectBase[Width, Height];

            // Loop through the map information generated by RogueSharp and create our cached visuals of that data
            foreach (var cell in rogueMap.GetAllCells())
            {
                if (cell.IsWalkable)
                {
                    mapData[cell.X, cell.Y] = new MapObjects.Floor();
                    mapData[cell.X, cell.Y].RenderToCell(this[cell.X, cell.Y], false, false);
                }
                else
                {
                    rogueMap.SetCellProperties(cell.X, cell.Y, false, false);
                    mapData[cell.X, cell.Y] = new MapObjects.Wall();
                    mapData[cell.X, cell.Y].RenderToCell(this[cell.X, cell.Y], false, false);

                    // We're a wall, so we block LOS
                    rogueMap.SetCellProperties(cell.X, cell.Y, false, cell.IsWalkable);
                }
            }
            
            PositionPlayer();
            Monsters = detailedMap.getMonsters(Position - textSurface.RenderArea.Location);
        }


        private void PositionPlayer()
        {
            Player.Position = detailedMap.getPlayerStartingPosition();
            
            TextSurface.RenderArea = new Rectangle(Player.Position.X - (TextSurface.RenderArea.Width / 2),
                                                    Player.Position.Y - (TextSurface.RenderArea.Height / 2),
                                                    TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);

            Player.RenderOffset = Position - TextSurface.RenderArea.Location;
            MovePlayerBy(new Point(0, 0));
        }
    }
}
