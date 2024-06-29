using System;
using System.Collections.Generic;
using System.Text;

public class SeaBattleGame
{
    private const int GridSize = 10;
    private char[,] playerGrid;
    private char[,] enemyGrid;
    private List<Ship> playerShips;
    private List<Ship> enemyShips;

    public SeaBattleGame()
    {
        playerGrid = new char[GridSize, GridSize];
        enemyGrid = new char[GridSize, GridSize];
        playerShips = new List<Ship>();
        enemyShips = new List<Ship>();

        InitializeGrid(playerGrid);
        InitializeGrid(enemyGrid);
        PlaceShips(playerGrid, playerShips);
        PlaceShips(enemyGrid, enemyShips);
    }

    private void InitializeGrid(char[,] grid)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                grid[i, j] = '~'; // ~  вода
            }
        }
    }

    private void PlaceShips(char[,] grid, List<Ship> ships)
    {
 
        ships.Add(new Ship(2, "Destroyer")); // Корабель з довжиною 2
        ships.Add(new Ship(3, "Submarine")); // Корабель з довжиною 3
        ships.Add(new Ship(3, "Cruiser")); // Корабель з довжиною 3
        ships.Add(new Ship(4, "Battleship")); // Корабель з довжиною 4
        ships.Add(new Ship(5, "Carrier")); // Корабель з довжиною 5

        Random rand = new Random();

        foreach (var ship in ships)
        {
            bool placed = false;

            while (!placed)
            {
                int row = rand.Next(GridSize);
                int col = rand.Next(GridSize);
                bool horizontal = rand.Next(2) == 0;

                if (CanPlaceShip(grid, row, col, ship.Size, horizontal))
                {
                    PlaceShip(grid, row, col, ship.Size, horizontal);
                    placed = true;
                }
            }
        }
    }

    private bool CanPlaceShip(char[,] grid, int row, int col, int size, bool horizontal)
    {
        if (horizontal)
        {
            if (col + size > GridSize)
                return false;

            for (int i = 0; i < size; i++)
            {
                if (grid[row, col + i] != '~')
                    return false;
            }
        }
        else
        {
            if (row + size > GridSize)
                return false;

            for (int i = 0; i < size; i++)
            {
                if (grid[row + i, col] != '~')
                    return false;
            }
        }

        return true;
    }

    private void PlaceShip(char[,] grid, int row, int col, int size, bool horizontal)
    {
        if (horizontal)
        {
            for (int i = 0; i < size; i++)
            {
                grid[row, col + i] = 'S';
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                grid[row + i, col] = 'S';
            }
        }
    }

    public string ProcessMove(string move)
    {
        if (string.IsNullOrEmpty(move) || move.Length < 2)
        {
            return "Неправильний формат ходу. Використовуйте формат A1, B2 тощо.";
        }

        char rowChar = move[0];
        int row = rowChar - 'A';
        if (row < 0 || row >= GridSize)
        {
            return "Неправильний рядок. Використовуйте літери від A до J.";
        }

        if (!int.TryParse(move.Substring(1), out int col) || col < 1 || col > GridSize)
        {
            return "Неправильна колонка. Використовуйте цифри від 1 до 10.";
        }
        col -= 1; // Перетворюємо в індекс масиву

        char target = enemyGrid[row, col];

        if (target == '~')
        {
            enemyGrid[row, col] = 'M'; // M означає промах
            return "Промах!";
        }
        else if (target == 'S')
        {
            enemyGrid[row, col] = 'H'; // H означає влучання
            return "Влучив!";
        }
        else
        {
            return "Ви вже стріляли в цю точку.";
        }
    }

    public string GetPlayerGrid()
    {
        return GetGridString(playerGrid);
    }

    public string GetEnemyGrid()
    {
        return GetGridString(enemyGrid);
    }

    private string GetGridString(char[,] grid)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("  1 2 3 4 5 6 7 8 9 10");
        for (int i = 0; i < GridSize; i++)
        {
            sb.Append((char)('A' + i) + " ");
            for (int j = 0; j < GridSize; j++)
            {
                sb.Append(grid[i, j] + " ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private class Ship
    {
        public int Size { get; }
        public string Name { get; }

        public Ship(int size, string name)
        {
            Size = size;
            Name = name;
        }
    }
}
