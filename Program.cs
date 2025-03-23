using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ArcadeNexus
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("========== Arcade Nexus ==========");
                Console.WriteLine("1. 控制台扫雷");
                Console.WriteLine("2. 记忆数字挑战");
                Console.WriteLine("3. 2048 数字合并");
                Console.WriteLine("4. 贪吃蛇游戏");
                Console.WriteLine("5. 躲避障碍物");
                Console.WriteLine("6. 打飞机游戏");
                Console.WriteLine("7. 井字棋");
                Console.WriteLine("8. Tetris（俄罗斯方块）");
                Console.WriteLine("9. 迷宫探险游戏");
                Console.WriteLine("10. 射击目标游戏");
                Console.WriteLine("11. 反应测试游戏");
                Console.WriteLine("12. 数学答题游戏");
                Console.WriteLine("13. 平台跳跃游戏");
                Console.WriteLine("14. 炸弹人小游戏");
                Console.WriteLine("15. 成语接龙");
                Console.WriteLine("0. 退出");
                Console.Write("请选择游戏编号：");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        PlayMinesweeper();
                        break;
                    case "2":
                        PlayMemoryNumbers();
                        break;
                    case "3":
                        Play2048();
                        break;
                    case "4":
                        PlaySnakeGame();
                        break;
                    case "5":
                        PlayDodgeObstacles();
                        break;
                    case "6":
                        PlayAirplaneGame();
                        break;
                    case "7":
                        PlayTicTacToe();
                        break;
                    case "8":
                        PlayTetris();
                        break;
                    case "9":
                        PlayMaze();
                        break;
                    case "10":
                        PlayShootingTarget();
                        break;
                    case "11":
                        PlayReactionTest();
                        break;
                    case "12":
                        PlayMathQuiz();
                        break;
                    case "13":
                        PlayPlatformer();
                        break;
                    case "14":
                        PlayBomberman();
                        break;
                    case "15":
                        PlayChengyuSolitaire();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("无效选项，请重试。");
                        Pause();
                        break;
                }
            }
        }

        // 辅助方法：在持续游戏中检测是否有中途退出请求
        static bool CheckMidGameExit()
        {
            if (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                    string exitChoice = Console.ReadLine();
                    if (exitChoice.ToLower() == "m")
                        return true;
                    else if (exitChoice.ToLower() == "x")
                        Environment.Exit(0);
                }
            }
            return false;
        }

        static void Pause()
        {
            Console.WriteLine("按任意键返回菜单...");
            Console.ReadKey();
        }

        #region 控制台扫雷
        static void PlayMinesweeper()
        {
            const int rows = 9, cols = 9, bombsCount = 10;
            bool[,] bombs = new bool[rows, cols];
            int[,] counts = new int[rows, cols];
            bool[,] revealed = new bool[rows, cols];
            bool[,] flagged = new bool[rows, cols];
            Random rnd = new Random();

            int placed = 0;
            while (placed < bombsCount)
            {
                int r = rnd.Next(rows);
                int c = rnd.Next(cols);
                if (!bombs[r, c])
                {
                    bombs[r, c] = true;
                    placed++;
                }
            }
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (bombs[r, c])
                        continue;
                    int count = 0;
                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            int nr = r + dr, nc = c + dc;
                            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && bombs[nr, nc])
                                count++;
                        }
                    }
                    counts[r, c] = count;
                }
            }

            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (revealed[r, c])
                        {
                            if (bombs[r, c])
                                Console.Write("* ");
                            else if (counts[r, c] == 0)
                                Console.Write("  ");
                            else
                                Console.Write(counts[r, c] + " ");
                        }
                        else if (flagged[r, c])
                        {
                            Console.Write("F ");
                        }
                        else
                        {
                            Console.Write("# ");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("输入命令：");
                Console.WriteLine("  r row col   (揭开格子)");
                Console.WriteLine("  f row col   (标记/取消标记)");
                Console.WriteLine("  q           (退出到菜单或退出应用)");
                string input = Console.ReadLine();
                if (input.ToLower() == "q")
                {
                    Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                    string exitChoice = Console.ReadLine();
                    if (exitChoice.ToLower() == "m")
                        return;
                    else if (exitChoice.ToLower() == "x")
                        Environment.Exit(0);
                    else
                        continue;
                }

                string[] parts = input.Split();
                if (parts.Length < 3)
                    continue;
                string cmd = parts[0];
                if (!int.TryParse(parts[1], out int rIndex) || !int.TryParse(parts[2], out int cIndex))
                    continue;
                if (rIndex < 0 || rIndex >= rows || cIndex < 0 || cIndex >= cols)
                    continue;

                if (cmd.ToLower() == "r")
                {
                    if (flagged[rIndex, cIndex])
                    {
                        Console.WriteLine("此格已标记！");
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (bombs[rIndex, cIndex])
                    {
                        revealed[rIndex, cIndex] = true;
                        Console.Clear();
                        for (int r = 0; r < rows; r++)
                        {
                            for (int c = 0; c < cols; c++)
                            {
                                if (bombs[r, c])
                                    Console.Write("* ");
                                else if (counts[r, c] == 0)
                                    Console.Write("  ");
                                else
                                    Console.Write(counts[r, c] + " ");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine("你踩到炸弹了！游戏结束！");
                        gameOver = true;
                    }
                    else
                    {
                        RevealCell(rIndex, cIndex, revealed, bombs, counts, rows, cols);
                    }
                }
                else if (cmd.ToLower() == "f")
                {
                    flagged[rIndex, cIndex] = !flagged[rIndex, cIndex];
                }
                bool win = true;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (!bombs[r, c] && !revealed[r, c])
                            win = false;
                    }
                }
                if (win)
                {
                    Console.Clear();
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < cols; c++)
                        {
                            if (bombs[r, c])
                                Console.Write("* ");
                            else if (counts[r, c] == 0)
                                Console.Write("  ");
                            else
                                Console.Write(counts[r, c] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("恭喜，你赢了！");
                    gameOver = true;
                }
            }
            Pause();
        }
        static void RevealCell(int r, int c, bool[,] revealed, bool[,] bombs, int[,] counts, int rows, int cols)
        {
            if (r < 0 || r >= rows || c < 0 || c >= cols)
                return;
            if (revealed[r, c])
                return;
            revealed[r, c] = true;
            if (counts[r, c] == 0)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (dr == 0 && dc == 0)
                            continue;
                        RevealCell(r + dr, c + dc, revealed, bombs, counts, rows, cols);
                    }
                }
            }
        }
        #endregion

        #region 记忆数字挑战
        static void PlayMemoryNumbers()
        {
            Console.WriteLine("记忆数字挑战游戏开始！（输入 q 可退出）");
            Random rnd = new Random();
            int length = 5;
            string sequence = "";
            for (int i = 0; i < length; i++)
            {
                sequence += rnd.Next(0, 10).ToString();
            }
            Console.WriteLine("记住这串数字： " + sequence);
            Thread.Sleep(3000);
            Console.Clear();
            Console.Write("请输入刚才的数字序列（或输入 q 退出）：");
            string input = Console.ReadLine();
            if (input.ToLower() == "q") return;
            if (input == sequence)
                Console.WriteLine("恭喜你，答对了！");
            else
                Console.WriteLine("答错了，正确序列为：" + sequence);
            Pause();
        }
        #endregion

        #region 2048 数字合并
        static void Play2048()
        {
            const int size = 4;
            int[,] board = new int[size, size];
            Random rnd = new Random();
            AddRandomTile(board, rnd);
            AddRandomTile(board, rnd);

            while (true)
            {
                if (CheckMidGameExit()) break;
                Console.Clear();
                Print2048Board(board);
                if (Check2048Win(board))
                {
                    Console.WriteLine("恭喜你达到了2048！");
                    break;
                }
                if (!CanMove(board))
                {
                    Console.WriteLine("游戏结束，无法移动！");
                    break;
                }
                Console.Write("使用 WASD 控制移动（按 Q 退出）：");
                ConsoleKeyInfo key = Console.ReadKey(true);
                bool moved = false;
                if (key.Key == ConsoleKey.Q)
                {
                    Console.WriteLine();
                    Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                    string choice = Console.ReadLine();
                    if (choice.ToLower() == "m")
                        break;
                    else if (choice.ToLower() == "x")
                        Environment.Exit(0);
                }
                else
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.W:
                            moved = MoveUp(board);
                            break;
                        case ConsoleKey.S:
                            moved = MoveDown(board);
                            break;
                        case ConsoleKey.A:
                            moved = MoveLeft(board);
                            break;
                        case ConsoleKey.D:
                            moved = MoveRight(board);
                            break;
                    }
                    if (moved)
                        AddRandomTile(board, rnd);
                }
            }
            Pause();
        }
        static void Print2048Board(int[,] board)
        {
            int size = board.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(board[i, j] == 0 ? ".\t" : board[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        static void AddRandomTile(int[,] board, Random rnd)
        {
            List<(int, int)> empty = new List<(int, int)>();
            int size = board.GetLength(0);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j] == 0)
                        empty.Add((i, j));
            if (empty.Count == 0)
                return;
            var pos = empty[rnd.Next(empty.Count)];
            board[pos.Item1, pos.Item2] = rnd.Next(10) < 9 ? 2 : 4;
        }
        static bool MoveLeft(int[,] board)
        {
            bool moved = false;
            int size = board.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                int[] row = new int[size];
                for (int j = 0; j < size; j++)
                    row[j] = board[i, j];
                int[] merged = Merge(row);
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] != merged[j])
                    {
                        board[i, j] = merged[j];
                        moved = true;
                    }
                }
            }
            return moved;
        }
        static bool MoveRight(int[,] board)
        {
            bool moved = false;
            int size = board.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                int[] row = new int[size];
                for (int j = 0; j < size; j++)
                    row[j] = board[i, j];
                Array.Reverse(row);
                int[] merged = Merge(row);
                Array.Reverse(merged);
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] != merged[j])
                    {
                        board[i, j] = merged[j];
                        moved = true;
                    }
                }
            }
            return moved;
        }
        static bool MoveUp(int[,] board)
        {
            bool moved = false;
            int size = board.GetLength(0);
            for (int j = 0; j < size; j++)
            {
                int[] col = new int[size];
                for (int i = 0; i < size; i++)
                    col[i] = board[i, j];
                int[] merged = Merge(col);
                for (int i = 0; i < size; i++)
                {
                    if (board[i, j] != merged[i])
                    {
                        board[i, j] = merged[i];
                        moved = true;
                    }
                }
            }
            return moved;
        }
        static bool MoveDown(int[,] board)
        {
            bool moved = false;
            int size = board.GetLength(0);
            for (int j = 0; j < size; j++)
            {
                int[] col = new int[size];
                for (int i = 0; i < size; i++)
                    col[i] = board[i, j];
                Array.Reverse(col);
                int[] merged = Merge(col);
                Array.Reverse(merged);
                for (int i = 0; i < size; i++)
                {
                    if (board[i, j] != merged[i])
                    {
                        board[i, j] = merged[i];
                        moved = true;
                    }
                }
            }
            return moved;
        }
        static int[] Merge(int[] line)
        {
            List<int> list = new List<int>();
            foreach (var num in line)
            {
                if (num != 0)
                    list.Add(num);
            }
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] == list[i + 1])
                {
                    list[i] *= 2;
                    list[i + 1] = 0;
                }
            }
            List<int> newList = new List<int>();
            foreach (var num in list)
            {
                if (num != 0)
                    newList.Add(num);
            }
            while (newList.Count < line.Length)
                newList.Add(0);
            return newList.ToArray();
        }
        static bool Check2048Win(int[,] board)
        {
            int size = board.GetLength(0);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j] == 2048)
                        return true;
            return false;
        }
        static bool CanMove(int[,] board)
        {
            int size = board.GetLength(0);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j] == 0)
                        return true;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i < size - 1 && board[i, j] == board[i + 1, j])
                        return true;
                    if (j < size - 1 && board[i, j] == board[i, j + 1])
                        return true;
                }
            }
            return false;
        }
        #endregion

        #region 贪吃蛇游戏（正常逻辑）
        static void PlaySnakeGame()
        {
            int width = 40, height = 20;
            List<(int, int)> snake = new List<(int, int)>();
            // 初始化蛇，长度为3
            int startX = width / 2, startY = height / 2;
            snake.Add((startX, startY));
            snake.Add((startX - 1, startY));
            snake.Add((startX - 2, startY));
            (int dx, int dy) direction = (1, 0);
            Random rnd = new Random();
            (int, int) food = GenerateSnakeFood(snake, width, height, rnd);
            int score = 0;
            bool gameOver = false;
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (!gameOver)
            {
                if (CheckMidGameExit())
                    break;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else
                    {
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                if (direction.dy != 1) direction = (0, -1);
                                break;
                            case ConsoleKey.DownArrow:
                                if (direction.dy != -1) direction = (0, 1);
                                break;
                            case ConsoleKey.LeftArrow:
                                if (direction.dx != 1) direction = (-1, 0);
                                break;
                            case ConsoleKey.RightArrow:
                                if (direction.dx != -1) direction = (1, 0);
                                break;
                        }
                    }
                }
                if (stopwatch.ElapsedMilliseconds > 150)
                {
                    stopwatch.Restart();
                    var head = snake[0];
                    var newHead = (head.Item1 + direction.dx, head.Item2 + direction.dy);
                    // 检查边界
                    if (newHead.Item1 < 0 || newHead.Item1 >= width || newHead.Item2 < 0 || newHead.Item2 >= height)
                    {
                        gameOver = true;
                        break;
                    }
                    // 检查自身碰撞
                    if (snake.Contains(newHead))
                    {
                        gameOver = true;
                        break;
                    }
                    snake.Insert(0, newHead);
                    // 如果吃到食物，则生成新食物，否则移除尾部
                    if (newHead.Equals(food))
                    {
                        score += 10;
                        food = GenerateSnakeFood(snake, width, height, rnd);
                    }
                    else
                    {
                        snake.RemoveAt(snake.Count - 1);
                    }
                    // 绘制画面
                    Console.Clear();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (snake[0].Equals((x, y)))
                                Console.Write("O");
                            else if (snake.Contains((x, y)))
                                Console.Write("o");
                            else if (food.Equals((x, y)))
                                Console.Write("F");
                            else
                                Console.Write(" ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("Score: " + score);
                }
            }
            Console.Clear();
            Console.WriteLine("游戏结束！得分：" + score);
            Pause();
        }
        static (int, int) GenerateSnakeFood(List<(int, int)> snake, int width, int height, Random rnd)
        {
            int fx, fy;
            do
            {
                fx = rnd.Next(0, width);
                fy = rnd.Next(0, height);
            } while (snake.Contains((fx, fy)));
            return (fx, fy);
        }
        #endregion

        #region 躲避障碍物
        static void PlayDodgeObstacles()
        {
            int width = 20, height = 15;
            int playerX = width / 2;
            int playerY = height - 1;
            List<(int, int)> obstacles = new List<(int, int)>();
            Random rnd = new Random();
            int score = 0;
            bool gameOver = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!gameOver)
            {
                if (CheckMidGameExit())
                    break;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else if (key == ConsoleKey.LeftArrow && playerX > 0)
                        playerX--;
                    else if (key == ConsoleKey.RightArrow && playerX < width - 1)
                        playerX++;
                }
                if (stopwatch.ElapsedMilliseconds > 200)
                {
                    stopwatch.Restart();
                    for (int i = 0; i < obstacles.Count; i++)
                    {
                        obstacles[i] = (obstacles[i].Item1, obstacles[i].Item2 + 1);
                    }
                    obstacles.RemoveAll(o => o.Item2 >= height);
                    if (rnd.NextDouble() < 0.5)
                    {
                        int ox = rnd.Next(0, width);
                        obstacles.Add((ox, 0));
                    }
                    foreach (var o in obstacles)
                    {
                        if (o.Item1 == playerX && o.Item2 == playerY)
                        {
                            gameOver = true;
                            break;
                        }
                    }
                    score++;
                    Console.Clear();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (x == playerX && y == playerY)
                                Console.Write("A");
                            else if (obstacles.Exists(o => o.Item1 == x && o.Item2 == y))
                                Console.Write("O");
                            else
                                Console.Write(" ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("得分: " + score);
                    Console.WriteLine("使用左右箭头移动。");
                }
            }
            Console.Clear();
            Console.WriteLine("游戏结束！你的得分：" + score);
            Pause();
        }
        #endregion

        #region 打飞机游戏
        static void PlayAirplaneGame()
        {
            int width = 20, height = 15;
            int playerX = width / 2;
            int playerY = height - 1;
            List<(int, int)> bullets = new List<(int, int)>();
            List<(int, int)> enemies = new List<(int, int)>();
            Random rnd = new Random();
            int score = 0;
            bool gameOver = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!gameOver)
            {
                if (CheckMidGameExit())
                    break;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else if (key == ConsoleKey.LeftArrow && playerX > 0)
                        playerX--;
                    else if (key == ConsoleKey.RightArrow && playerX < width - 1)
                        playerX++;
                    else if (key == ConsoleKey.Spacebar)
                        bullets.Add((playerX, playerY - 1));
                }
                if (stopwatch.ElapsedMilliseconds > 200)
                {
                    stopwatch.Restart();
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        var b = bullets[i];
                        b = (b.Item1, b.Item2 - 1);
                        if (b.Item2 < 0)
                            bullets.RemoveAt(i);
                        else
                            bullets[i] = b;
                    }
                    for (int i = enemies.Count - 1; i >= 0; i--)
                    {
                        var e = enemies[i];
                        e = (e.Item1, e.Item2 + 1);
                        if (e.Item2 >= height)
                            enemies.RemoveAt(i);
                        else
                            enemies[i] = e;
                    }
                    if (rnd.NextDouble() < 0.3)
                    {
                        int ex = rnd.Next(0, width);
                        enemies.Add((ex, 0));
                    }
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        var b = bullets[i];
                        for (int j = enemies.Count - 1; j >= 0; j--)
                        {
                            var e = enemies[j];
                            if (b.Item1 == e.Item1 && b.Item2 == e.Item2)
                            {
                                bullets.RemoveAt(i);
                                enemies.RemoveAt(j);
                                score += 10;
                                break;
                            }
                        }
                    }
                    foreach (var e in enemies)
                    {
                        if (e.Item1 == playerX && e.Item2 == playerY)
                        {
                            gameOver = true;
                            break;
                        }
                    }
                    Console.Clear();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (x == playerX && y == playerY)
                                Console.Write("^");
                            else if (bullets.Exists(b => b.Item1 == x && b.Item2 == y))
                                Console.Write("|");
                            else if (enemies.Exists(e => e.Item1 == x && e.Item2 == y))
                                Console.Write("V");
                            else
                                Console.Write(" ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("得分: " + score);
                    Console.WriteLine("左右移动，空格发射子弹。");
                }
            }
            Console.Clear();
            Console.WriteLine("游戏结束！你的得分：" + score);
            Pause();
        }
        #endregion

        #region Tetris（俄罗斯方块）
        static void PlayTetris()
        {
            int width = 10, height = 20;
            int[,] board = new int[height, width];
            Tetromino current = Tetromino.GetRandom();
            int currentX = width / 2 - 2, currentY = 0;
            Random rnd = new Random();
            bool gameOver = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!gameOver)
            {
                if (CheckMidGameExit())
                    break;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        if (!CheckCollision(board, current, currentX - 1, currentY))
                            currentX--;
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        if (!CheckCollision(board, current, currentX + 1, currentY))
                            currentX++;
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        var rotated = current.Rotate();
                        if (!CheckCollision(board, rotated, currentX, currentY))
                            current = rotated;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        if (!CheckCollision(board, current, currentX, currentY + 1))
                            currentY++;
                    }
                }
                if (stopwatch.ElapsedMilliseconds > 500)
                {
                    stopwatch.Restart();
                    if (!CheckCollision(board, current, currentX, currentY + 1))
                        currentY++;
                    else
                    {
                        Merge(board, current, currentX, currentY);
                        ClearLines(board, height, width);
                        current = Tetromino.GetRandom();
                        currentX = width / 2 - 2;
                        currentY = 0;
                        if (CheckCollision(board, current, currentX, currentY))
                        {
                            gameOver = true;
                        }
                    }
                }
                Console.Clear();
                int[,] display = (int[,])board.Clone();
                for (int i = 0; i < current.Shape.GetLength(0); i++)
                {
                    for (int j = 0; j < current.Shape.GetLength(1); j++)
                    {
                        if (current.Shape[i, j] != 0)
                        {
                            int x = currentX + j, y = currentY + i;
                            if (y >= 0 && y < height && x >= 0 && x < width)
                                display[y, x] = current.Shape[i, j];
                        }
                    }
                }
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Console.Write(display[i, j] == 0 ? ". " : "[]");
                    }
                    Console.WriteLine();
                }
            }
            Console.Clear();
            Console.WriteLine("游戏结束！");
            Pause();
        }
        static bool CheckCollision(int[,] board, Tetromino tetro, int posX, int posY)
        {
            int rows = tetro.Shape.GetLength(0), cols = tetro.Shape.GetLength(1);
            int height = board.GetLength(0), width = board.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (tetro.Shape[i, j] != 0)
                    {
                        int x = posX + j, y = posY + i;
                        if (x < 0 || x >= width || y < 0 || y >= height) return true;
                        if (board[y, x] != 0) return true;
                    }
                }
            }
            return false;
        }
        static void Merge(int[,] board, Tetromino tetro, int posX, int posY)
        {
            int rows = tetro.Shape.GetLength(0), cols = tetro.Shape.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (tetro.Shape[i, j] != 0)
                    {
                        board[posY + i, posX + j] = tetro.Shape[i, j];
                    }
                }
            }
        }
        static void ClearLines(int[,] board, int height, int width)
        {
            for (int i = height - 1; i >= 0; i--)
            {
                bool full = true;
                for (int j = 0; j < width; j++)
                {
                    if (board[i, j] == 0) { full = false; break; }
                }
                if (full)
                {
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            board[k, j] = board[k - 1, j];
                        }
                    }
                    for (int j = 0; j < width; j++)
                        board[0, j] = 0;
                    i++;
                }
            }
        }
        public class Tetromino
        {
            public int[,] Shape;
            public Tetromino(int[,] shape) { Shape = shape; }
            public Tetromino Rotate()
            {
                int rows = Shape.GetLength(0), cols = Shape.GetLength(1);
                int[,] rotated = new int[cols, rows];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        rotated[j, rows - 1 - i] = Shape[i, j];
                return new Tetromino(rotated);
            }
            public static Tetromino GetRandom()
            {
                Random rnd = new Random();
                int r = rnd.Next(7);
                switch (r)
                {
                    case 0: return new Tetromino(new int[,] { { 1, 1, 1, 1 } });
                    case 1: return new Tetromino(new int[,] { { 2, 2 }, { 2, 2 } });
                    case 2: return new Tetromino(new int[,] { { 0, 3, 0 }, { 3, 3, 3 } });
                    case 3: return new Tetromino(new int[,] { { 4, 0, 0 }, { 4, 4, 4 } });
                    case 4: return new Tetromino(new int[,] { { 0, 0, 5 }, { 5, 5, 5 } });
                    case 5: return new Tetromino(new int[,] { { 6, 6, 0 }, { 0, 6, 6 } });
                    case 6: return new Tetromino(new int[,] { { 0, 7, 7 }, { 7, 7, 0 } });
                    default: return new Tetromino(new int[,] { { 1, 1, 1, 1 } });
                }
            }
        }
        #endregion

        #region 平台跳跃游戏
        static void PlayPlatformer()
        {
            int width = 30, height = 10;
            char[,] screen = new char[height, width];
            int playerX = width / 4, playerY = height - 2;
            float playerYFloat = playerY;
            float velocityY = 0;
            bool isJumping = false;
            List<(int, int)> obstacles = new List<(int, int)>();
            Random rnd = new Random();
            int score = 0;
            bool gameOver = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!gameOver)
            {
                if (CheckMidGameExit())
                    break;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else if (key == ConsoleKey.LeftArrow && playerX > 0)
                        playerX--;
                    else if (key == ConsoleKey.RightArrow && playerX < width - 1)
                        playerX++;
                    else if (key == ConsoleKey.Spacebar && !isJumping)
                    {
                        velocityY = -3;
                        isJumping = true;
                    }
                }
                playerYFloat += velocityY;
                velocityY += 0.5f;
                if (playerYFloat >= height - 2)
                {
                    playerYFloat = height - 2;
                    velocityY = 0;
                    isJumping = false;
                }
                playerY = (int)playerYFloat;
                for (int i = 0; i < obstacles.Count; i++)
                {
                    obstacles[i] = (obstacles[i].Item1 - 1, obstacles[i].Item2);
                }
                obstacles.RemoveAll(o => o.Item1 < 0);
                if (rnd.NextDouble() < 0.1)
                {
                    int ox = width - 1;
                    int oy = height - 2;
                    obstacles.Add((ox, oy));
                }
                foreach (var o in obstacles)
                {
                    if (o.Item1 == playerX && o.Item2 == playerY)
                    {
                        gameOver = true;
                        break;
                    }
                }
                score++;
                Array.Clear(screen, 0, screen.Length);
                for (int x = 0; x < width; x++)
                    screen[height - 1, x] = '=';
                foreach (var o in obstacles)
                {
                    if (o.Item2 >= 0 && o.Item2 < height && o.Item1 >= 0 && o.Item1 < width)
                        screen[o.Item2, o.Item1] = 'O';
                }
                screen[playerY, playerX] = '@';
                Console.Clear();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Console.Write(screen[y, x] == '\0' ? " " : screen[y, x].ToString());
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("得分：" + score);
                Thread.Sleep(100);
            }
            Console.Clear();
            Console.WriteLine("游戏结束！得分：" + score);
            Pause();
        }
        #endregion

        #region 炸弹人小游戏
        static void PlayBomberman()
        {
            int width = 10, height = 10;
            char[,] grid = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                        grid[i, j] = '#';
                    else
                        grid[i, j] = ' ';
                }
            }
            Random rnd = new Random();
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    if (rnd.NextDouble() < 0.3)
                        grid[i, j] = '%';
                }
            }
            int playerX = 1, playerY = 1;
            grid[playerY, playerX] = '@';
            List<(int, int, int)> bombs = new List<(int, int, int)>();
            bool gameOver = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!gameOver)
            {
                if (CheckMidGameExit())
                    break;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    int newX = playerX, newY = playerY;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else if (key == ConsoleKey.LeftArrow) newX--;
                    else if (key == ConsoleKey.RightArrow) newX++;
                    else if (key == ConsoleKey.UpArrow) newY--;
                    else if (key == ConsoleKey.DownArrow) newY++;
                    else if (key == ConsoleKey.Spacebar)
                    {
                        if (!bombs.Exists(b => b.Item1 == playerX && b.Item2 == playerY))
                            bombs.Add((playerX, playerY, 5));
                    }
                    if (newX >= 0 && newX < width && newY >= 0 && newY < height && grid[newY, newX] == ' ')
                    {
                        grid[playerY, playerX] = ' ';
                        playerX = newX; playerY = newY;
                        grid[playerY, playerX] = '@';
                    }
                }
                if (stopwatch.ElapsedMilliseconds > 500)
                {
                    stopwatch.Restart();
                    for (int i = bombs.Count - 1; i >= 0; i--)
                    {
                        var b = bombs[i];
                        b = (b.Item1, b.Item2, b.Item3 - 1);
                        if (b.Item3 <= 0)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    if (Math.Abs(dx) + Math.Abs(dy) == 1)
                                    {
                                        int nx = b.Item1 + dx, ny = b.Item2 + dy;
                                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                                        {
                                            if (grid[ny, nx] == '%')
                                                grid[ny, nx] = ' ';
                                            if (nx == playerX && ny == playerY)
                                                gameOver = true;
                                        }
                                    }
                                }
                            }
                            bombs.RemoveAt(i);
                        }
                        else
                        {
                            bombs[i] = b;
                        }
                    }
                    Console.Clear();
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (bombs.Exists(b => b.Item1 == j && b.Item2 == i))
                                Console.Write("B");
                            else
                                Console.Write(grid[i, j]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("使用箭头移动，空格放置炸弹。（按 Q 退出）");
                }
            }
            Console.Clear();
            Console.WriteLine("游戏结束！");
            Pause();
        }
        #endregion

        #region 井字棋
        static void PlayTicTacToe()
        {
            Console.WriteLine("井字棋游戏开始！（输入 q 可退出）");
            char[,] board = new char[3, 3]
            {
                { '1', '2', '3' },
                { '4', '5', '6' },
                { '7', '8', '9' }
            };

            int moves = 0;
            char currentPlayer = 'X';
            bool gameEnded = false;
            while (!gameEnded)
            {
                Console.Clear();
                PrintTicTacToeBoard(board);
                Console.WriteLine("玩家 " + currentPlayer + " 请输入选择的位置（或输入 q 退出）：");
                string input = Console.ReadLine();
                if (input.ToLower() == "q") break;
                if (int.TryParse(input, out int pos) && pos >= 1 && pos <= 9)
                {
                    int row = (pos - 1) / 3;
                    int col = (pos - 1) % 3;
                    if (board[row, col] != 'X' && board[row, col] != 'O')
                    {
                        board[row, col] = currentPlayer;
                        moves++;
                        if (CheckWin(board, currentPlayer))
                        {
                            Console.Clear();
                            PrintTicTacToeBoard(board);
                            Console.WriteLine("玩家 " + currentPlayer + " 获胜！");
                            gameEnded = true;
                        }
                        else if (moves == 9)
                        {
                            Console.Clear();
                            PrintTicTacToeBoard(board);
                            Console.WriteLine("平局！");
                            gameEnded = true;
                        }
                        else
                        {
                            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                        }
                    }
                    else
                    {
                        Console.WriteLine("该位置已被占用，请重试。");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("输入无效，请重试。");
                    Thread.Sleep(1000);
                }
            }
            Pause();
        }
        static void PrintTicTacToeBoard(char[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(" {0} | {1} | {2} ", board[i, 0], board[i, 1], board[i, 2]);
                if (i < 2)
                    Console.WriteLine("---+---+---");
            }
        }
        static bool CheckWin(char[,] board, char player)
        {
            for (int i = 0; i < 3; i++)
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
                    return true;
            for (int i = 0; i < 3; i++)
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
                    return true;
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
                return true;
            return false;
        }
        #endregion

        #region 迷宫探险游戏
        static void PlayMaze()
        {
            Console.WriteLine("迷宫探险游戏开始！（按 Q 退出）");
            char[,] maze = new char[,]
            {
                { '#', '#', '#', '#', '#', '#', '#', '#' },
                { '#', '@', ' ', ' ', '#', ' ', 'E', '#' },
                { '#', ' ', '#', ' ', '#', ' ', '#', '#' },
                { '#', ' ', '#', ' ', ' ', ' ', '#', '#' },
                { '#', ' ', ' ', '#', '#', ' ', ' ', '#' },
                { '#', '#', ' ', ' ', '#', ' ', '#', '#' },
                { '#', '#', '#', '#', '#', '#', '#', '#' }
            };
            int playerRow = 1, playerCol = 1;
            bool exitFound = false;
            while (!exitFound)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        Console.WriteLine("退出选项：按 M 返回主菜单，按 X 退出应用程序，其它任意键继续游戏");
                        string exitChoice = Console.ReadLine();
                        if (exitChoice.ToLower() == "m") break;
                        else if (exitChoice.ToLower() == "x") Environment.Exit(0);
                    }
                    else
                    {
                        int newRow = playerRow, newCol = playerCol;
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                newRow--;
                                break;
                            case ConsoleKey.DownArrow:
                                newRow++;
                                break;
                            case ConsoleKey.LeftArrow:
                                newCol--;
                                break;
                            case ConsoleKey.RightArrow:
                                newCol++;
                                break;
                        }
                        if (maze[newRow, newCol] != '#')
                        {
                            maze[playerRow, playerCol] = ' ';
                            playerRow = newRow;
                            playerCol = newCol;
                            if (maze[playerRow, playerCol] == 'E')
                            {
                                maze[playerRow, playerCol] = '@';
                                exitFound = true;
                            }
                            else
                            {
                                maze[playerRow, playerCol] = '@';
                            }
                        }
                    }
                }
                Thread.Sleep(50);
            }
            Console.Clear();
            PrintMaze(maze);
            Console.WriteLine("恭喜你走出迷宫！");
            Pause();
        }
        static void PrintMaze(char[,] maze)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region 射击目标游戏
        static void PlayShootingTarget()
        {
            Console.WriteLine("射击目标游戏开始！（输入 q 可退出）");
            const int rows = 5, cols = 10;
            Random rnd = new Random();
            int score = 0, rounds = 5;
            for (int round = 0; round < rounds; round++)
            {
                int targetRow = rnd.Next(rows);
                int targetCol = rnd.Next(cols);
                char[,] grid = new char[rows, cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        grid[i, j] = '.';
                grid[targetRow, targetCol] = 'O';
                Console.Clear();
                Console.WriteLine("请记住目标位置：");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                        Console.Write(grid[i, j] + " ");
                    Console.WriteLine();
                }
                Thread.Sleep(1000);
                Console.Clear();
                Console.Write("请输入目标所在的行（0-" + (rows - 1) + "，或输入 q 退出）：");
                string sRow = Console.ReadLine();
                if (sRow.ToLower() == "q") break;
                int guessRow = int.TryParse(sRow, out guessRow) ? guessRow : -1;
                Console.Write("请输入目标所在的列（0-" + (cols - 1) + "，或输入 q 退出）：");
                string sCol = Console.ReadLine();
                if (sCol.ToLower() == "q") break;
                int guessCol = int.TryParse(sCol, out guessCol) ? guessCol : -1;
                if (guessRow == targetRow && guessCol == targetCol)
                {
                    Console.WriteLine("命中！");
                    score++;
                }
                else
                {
                    Console.WriteLine("未命中，正确位置为：行 {0}, 列 {1}", targetRow, targetCol);
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("射击目标游戏结束，你的得分：{0}/{1}", score, rounds);
            Pause();
        }
        #endregion

        #region 反应测试游戏
        static void PlayReactionTest()
        {
            Console.WriteLine("反应测试游戏开始！（输入 q 可退出）");
            Random rnd = new Random();
            int delay = rnd.Next(2000, 5000);
            Console.WriteLine("准备……");
            Thread.Sleep(delay);
            Console.WriteLine("按任意键！（或输入 q 退出）");
            Stopwatch watch = Stopwatch.StartNew();
            string input = Console.ReadLine();
            if (input.ToLower() == "q") return;
            watch.Stop();
            Console.WriteLine("你的反应时间为：{0} 毫秒", watch.ElapsedMilliseconds);
            Pause();
        }
        #endregion

        #region 数学答题游戏
        static void PlayMathQuiz()
        {
            Console.WriteLine("数学答题游戏开始！（输入 q 可退出）");
            Random rnd = new Random();
            int a = rnd.Next(1, 10);
            int b = rnd.Next(1, 10);
            Console.WriteLine("请计算：{0} + {1} = ?", a, b);
            string answer = Console.ReadLine();
            if (answer.ToLower() == "q") return;
            if (int.TryParse(answer, out int result) && result == a + b)
                Console.WriteLine("回答正确！");
            else
                Console.WriteLine("回答错误，正确答案为：{0}", a + b);
            Pause();
        }
        #endregion

        #region 成语接龙游戏
        static void PlayChengyuSolitaire()
        {
            Console.WriteLine("成语接龙游戏开始！（输入 exit 或 q 可退出）");
            List<string> chengyuList = new List<string>
            {
                "画龙点睛",
                "睛花缭乱",
                "乱七八糟",
                "糟糠之妻",
                "妻离子散",
                "散兵游勇",
                "勇往直前",
                "前功尽弃",
                "弃旧图新"
            };

            Random rnd = new Random();
            string current = chengyuList[rnd.Next(chengyuList.Count)];
            Console.WriteLine("起始成语： " + current);
            while (true)
            {
                Console.WriteLine("请输入一个以“{0}”结尾字开头的成语（或输入 exit 或 q 退出）：", current[current.Length - 1]);
                string input = Console.ReadLine();
                if (input.ToLower() == "exit" || input.ToLower() == "q")
                    break;
                if (input.Length == 0)
                    continue;
                if (input[0] != current[current.Length - 1])
                {
                    Console.WriteLine("不符合接龙规则，请重新输入。");
                    continue;
                }
                if (!chengyuList.Contains(input))
                {
                    Console.WriteLine("未找到该成语，请重新输入。");
                    continue;
                }
                current = input;
                Console.WriteLine("接龙成功，新成语： " + current);
            }
            Pause();
        }
        #endregion
    }
}
