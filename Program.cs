using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace PlayerDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerDatabase playerDatabase = new PlayerDatabase();
            playerDatabase.Work();
        }
    }

    class PlayerDatabase
    {
        private List<Player> _players = new List<Player>();
        private int _lastPlayerId;

        public void Work()
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
                        BanPlayer();
                        break;

                    case CommandUnban:
                        UnbanPlayer();
                        break;

                    case CommandDelete:
                        DeletePlayer();
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
                player.ShowStats();
        }

        private void AddPlayer()
        {
            string name = ReadName();
            int level = ReadLevel();
            int id = GetID();

            _players.Add(new Player(id, name, level));
        }

        private int ReadInt()
        {
            int number;

            while (int.TryParse(Console.ReadLine(), out number) == false)
                Console.WriteLine("Это не число.");

            return number;
        }

        private int ReadLevel()
        {
            int level;

            do
            {
                Console.WriteLine("Введите уровень:");
                level = ReadInt();
            }
            while (level <= 0);

            return level;
        }

        private string ReadName()
        {
            Console.WriteLine("Введите имя персонажа:");
            return Console.ReadLine();
        }

        private void BanPlayer()
        {
            if (IsDatabaseEmpty() == false)
            {
                ShowAllPlayers();

                if (TryGetPlayer(out Player player))
                    player.Ban();
            }
        }

        private void UnbanPlayer()
        {
            if (IsDatabaseEmpty() == false)
            {
                ShowAllPlayers();

                if (TryGetPlayer(out Player player))
                    player.Unban();
            }
        }

        private void DeletePlayer()
        {
            if (IsDatabaseEmpty() == false)
            {
                ShowAllPlayers();

                if (TryGetPlayer(out Player player))
                    _players.Remove(player);
            }
        }

        private bool TryGetPlayer(out Player player)
        {
            player = null;

            Console.WriteLine("Введите идентификатор:");
            int id = ReadInt();

            foreach (var item in _players)
            {
                if (id == item.Id)
                {
                    player = item;
                    return true;
                }
            }

            Console.WriteLine("Это не идентификатор.");

            return false;
        }

        private bool IsDatabaseEmpty()
        {
            if (_players.Count == 0)
            {
                Console.WriteLine("База пуста.");
                return true;
            }

            return false;
        }

        private int GetID()
        {
            return _lastPlayerId++;
        }
    }

    class Player
    {
        private string _name;
        private int _level;
        private bool _isBanned;

        public Player(int id, string name, int level)
        {
            Id = id;
            _name = name;
            _level = level;
            _isBanned = false;
        }

        public int Id { get; private set; }

        public void ShowStats()
        {
            Console.WriteLine($"{Id} - {_name} - {_level} - {_isBanned}");
        }

        public void Ban()
        {
            if (_isBanned)
            {
                Console.WriteLine("Игрок уже забанен.");
            }
            else
            {
                Console.WriteLine("Игрок забанен.");

                _isBanned = true;
            }
        }

        public void Unban()
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
        }
    }
}
