using System;
using System.Collections.Generic;

namespace PlayerDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerDatabase playerDatabase = new PlayerDatabase();
            playerDatabase.ShowMenu();
        }
    }

    class PlayerDatabase
    {
        private Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private int _adtPlayerId;

        public void ShowMenu()
        {
            const string CommandShowAllPlayers = "1";
            const string CommandAdd = "2";
            const string CommandBan = "3";
            const string CommandUnban = "4";
            const string CommandDelete = "5";
            const string CommandExit = "exit";

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine($"Введите команду:\n{CommandShowAllPlayers} - чтобы просмотреть базу игроков.\n" +
                    $"{CommandAdd} - чтобы добавить игрока.\n" +
                    $"{CommandBan} - чтобы забанить игрока.\n" +
                    $"{CommandUnban} - чтобы разбанить игрока.\n" +
                    $"{CommandDelete} - чтобы удалить игрока.\n" +
                    $"{CommandExit} - чтобы выйти.");
                string userInput = Console.ReadLine();

                Console.Clear();

                switch (userInput)
                {
                    case CommandShowAllPlayers:
                        ShowAllPlayers();
                        break;

                    case CommandAdd:
                        AddPlayer();
                        break;

                    case CommandBan:
                        Ban();
                        break;

                    case CommandUnban:
                        Unban();
                        break;

                    case CommandDelete:
                        Delete();
                        break;

                    case CommandExit:
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Вы ввели неизвестную команду.");
                        break;
                }
            }

            Console.WriteLine("Вы вышли из меню.");
        }

        private void ShowAllPlayers()
        {
            Console.WriteLine($"Всего в базе: {_players.Count} игроков.");

            foreach (var player in _players)
            {
                Console.Write($"{player.Key} - ");
                player.Value.ShowStats();
            }
        }

        private void AddPlayer()
        {
            bool isInputFalse = false;

            int level = 0;

            Console.WriteLine("Введите имя персонажа:");
            string name = Console.ReadLine();

            foreach (Player value in _players.Values)
            {
                if (value.IsNameTaken(name))
                {
                    Console.WriteLine("Персонаж с таким именем уже есть.");

                    isInputFalse = true;
                    break;
                }
            }

            if (isInputFalse == false)
            {
                Console.WriteLine("Введите уровень:");
                bool isLevel = int.TryParse(Console.ReadLine(), out level);

                if (isLevel == false || level <= 0)
                {
                    isInputFalse = true;
                    Console.WriteLine("Это не уровень.");
                }
            }

            if (isInputFalse == false)
                _players.Add(GetID(), new Player(name, level));
        }

        private void Ban()
        {
            ShowAllPlayers();

            if (TryGetPlayer(out int id, out Player player))
                player.IsBan();
        }

        private void Unban()
        {
            ShowAllPlayers();

            if (TryGetPlayer(out int id, out Player player))
                player.isUnban();
        }

        private void Delete()
        {
            ShowAllPlayers();

            if (TryGetPlayer(out int id, out Player player))
                _players.Remove(id);
        }

        private bool TryGetPlayer(out int id, out Player player)
        {
            Console.WriteLine("Введите идентификатор:");
            string userInput = Console.ReadLine();

            int.TryParse(userInput, out id);
            bool isId = _players.TryGetValue(id, out player);

            if (isId == false)
                Console.WriteLine("Это не идентификатор.");

            return isId;
        }

        private int GetID()
        {
            return _adtPlayerId++;
        }
    }

    class Player
    {
        private string _name;
        private int _level;
        private bool _isBanned;

        public Player(string name, int level)
        {
            _name = name;
            _level = level;
            _isBanned = false;
        }

        public void ShowStats()
        {
            Console.WriteLine($"{_name} - {_level} - {_isBanned}");
        }

        public bool IsBan()
        {
            if (_isBanned == true)
            {
                Console.WriteLine("Игрок уже забанен.");
            }
            else
            {
                Console.WriteLine("Игрок забанен.");

                _isBanned = true;
            }

            return _isBanned;
        }

        public bool isUnban()
        {
            if (_isBanned == false)
            {
                Console.WriteLine("Игрок не забанен.");
            }
            else
            {
                Console.WriteLine("Игрок разбанен.");

                _isBanned = false;
            }

            return _isBanned;
        }

        public bool IsNameTaken(string name)
        {
            return _name.ToLower() == name.ToLower();
        }
    }
}
