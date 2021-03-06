﻿using MechsVsMinionsLibrary.terrain;
using MechsVsMinionsLibrary.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MechsVsMinionsLibrary.game
{
    public class GameBoard
    {
        private static GameBoard instance;

        private IGameItem[,] boardItems;

        private ITerrain[,] boardTerrain;

        private IList<Minion> minions;

        public GameBoard()
        {
            boardItems = new IGameItem[5, 5];
            boardTerrain = new ITerrain[5, 5];
            minions = new List<Minion>();

            for (int i = 0; i < boardTerrain.GetLength(0); i++)
            {
                for (int j = 0; j < boardTerrain.GetLength(1); j++)
                {
                    boardTerrain[i, j] = new Plain();
                }
            }
        }

        public void MoveMinions(int direction)
        {
            List<Minion> tempMinions = minions.ToList();
            foreach (Minion m in tempMinions)
            {

                m.Location = m.Location.AdjacentDirection(direction);
                if (GetTerrain(m.Location) != null && GetGameItem(m.Location) == null)
                {
                    Remove(m.Location.AdjacentDirection(direction + 4));
                    Add(m, m.Location);
                }
            }
        }

        public void Remove(Location l)
        {
            if (l.X < 0 || l.X >= boardTerrain.GetLength(0) || l.Y < 0 || l.Y >= boardTerrain.GetLength(1))
                return;
            if (boardItems[l.X, l.Y] is Minion)
            {
                minions.Remove((Minion)boardItems[l.X, l.Y]);
            }
            boardItems[l.X, l.Y] = null;
        }

        public void Add(IGameItem gameItem, Location l)
        {
            if (gameItem is Minion)
            {
                minions.Add((Minion)gameItem);
            }

            if (l.X < 0 || l.X >= boardTerrain.GetLength(0) || l.Y < 0 || l.Y >= boardTerrain.GetLength(1))
                return;
            boardItems[l.X, l.Y] = gameItem;
        }

        public IGameItem GetGameItem(Location l)
        {

            if (l.X < 0 || l.X >= boardTerrain.GetLength(0) || l.Y < 0 || l.Y >= boardTerrain.GetLength(1))
                return null;
            return boardItems[l.X, l.Y];
        }

        public ITerrain GetTerrain(Location l)
        {
            if (l.X < 0 || l.X >= boardTerrain.GetLength(0) || l.Y < 0 || l.Y >= boardTerrain.GetLength(1))
                return null;
            return boardTerrain[l.X, l.Y];
        }

        public static GameBoard getInstance()
        {
            return instance;
        }

        public static void loadBoard(string file)
        {
            instance = new GameBoard();
        }

        public void Display()
        {
            Console.WriteLine(this);
            Console.ReadLine();
        }
        public int DisplayWithPrompt(string prompt)
        {
            Console.WriteLine(this);
            Console.Write(prompt);
            int responce;
            while (!int.TryParse(Console.ReadLine(), out responce))
            {
                Console.WriteLine(this);
                Console.Write(prompt);
            }
            return responce;
        }

        public override string ToString()
        {
            StringBuilder board = new StringBuilder();

            board.Append("\n/");
            for (int j = 0; j < boardTerrain.GetLength(0) - 1; j++)
            {
                board.Append("===+");
            }
            board.Append("===\\");

            for (int i = 0; i < boardTerrain.GetLength(1) - 1; i++)
            {
                board.Append("\n| ");
                for (int j = 0; j < boardTerrain.GetLength(0) - 1; j++)
                {
                    board.Append(GetDisplayChar(j, i));
                    board.Append(" | ");
                }
                board.Append(GetDisplayChar(boardItems.GetLength(0) - 1, i));
                board.Append(" |");

                board.Append("\n+");
                for (int j = 0; j < boardTerrain.GetLength(0); j++)
                {
                    board.Append("===+");
                }
            }
            board.Append("\n| ");
            for (int j = 0; j < boardTerrain.GetLength(0) - 1; j++)
            {
                board.Append(GetDisplayChar(j, boardTerrain.GetLength(1) - 1));
                board.Append(" | ");
            }
            board.Append(GetDisplayChar(boardItems.GetLength(0) - 1, boardTerrain.GetLength(0) - 1));
            board.Append(" |");
            board.Append("\n\\");
            for (int j = 0; j < boardTerrain.GetLength(0) - 1; j++)
            {
                board.Append("===+");
            }
            board.Append("===/\n");

            return board.ToString();
        }

        private string GetDisplayChar(int x, int y)
        {
            if (boardItems[x, y] != null)
            {
                return boardItems[x, y].ToString();
            }
            return " ";
        }
    }
}
