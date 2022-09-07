﻿using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;
using static System.Console;

namespace Snake.Console.Abstractions;

public class ConsoleRenderer : IRenderer
{
	#region CONSTS
	// ═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬
	// ─│┌┐└┘├┬┴┼
	// ♦◊◌●☼
	// █▓▒░
	// ■□▪▫
	// ▲►▼◄
	private const char EMPTY_SYMBOL = ' ';
	private const char BORDER_SYMBOL = '█';
	private const char SNAKE_SYMBOL = '▒';
	private const char SNAKE_HEAD_SYMBOL = '▓';
	private const char FRUIT_SYMBOL = '■';
	private const char BOMB_SYMBOL = '●';
	private const char BOMB_EXPLODE_SYMBOL = '◌';
	private const char BOMB_BLINK_TOP_SYMBOL = '▲';
	private const char BOMB_BLINK_BOTTOM_SYMBOL = '▼';
	private const char BOMB_BLINK_LEFT_SYMBOL = '◄';
	private const char BOMB_BLINK_RIGHT_SYMBOL = '►';

	private const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
	private const ConsoleColor SNAKE_COLOR = ConsoleColor.Green;
	private const ConsoleColor DEAD_SNAKE_COLOR = ConsoleColor.DarkGreen;
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;
	private const ConsoleColor SCORE_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor POSITIONS_COLOR = ConsoleColor.DarkCyan;
	private const ConsoleColor PLAYER_DEATH_COLOR = ConsoleColor.DarkRed;
	private const ConsoleColor BOMB_ON_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor BOMB_OFF_COLOR = ConsoleColor.DarkYellow;

	#endregion

	public ConsoleRenderer() 
		=> CursorVisible = false;

	public void Render(IBoard board)
	{
		System.Console.Clear();
		for (var y = 0; y < board.Height + 2; y++)
		{
			for (var x = 0; x < board.Width + 2; x++)
				if (x == 0 || x == board.Width + 1 ||
				    y == 0 || y == board.Height + 1)
					Print(new Coord(x - 1, y - 1), BORDER_SYMBOL);
			
			if (SnakeGame.IsDebugMode && y > 0 && y <= board.Height)
				Print(new Coord(board.Width + 8 - (y - 1).ToString().Length, y - 1), (y - 1).ToString());
		}
	}

	public void Render(ISnake snake)
	{
		for (var i = 0; i < 2; i++)
			Print(snake.Coords.ElementAt(i), i == 0 ? SNAKE_HEAD_SYMBOL : SNAKE_SYMBOL, SNAKE_COLOR);
	}

	public void Render(IFruit fruit)
		=> Print(fruit.Coord, FRUIT_SYMBOL, FRUIT_COLOR);
	public void RenderFruit(Coord coord) 
		=> Print(coord, FRUIT_SYMBOL, FRUIT_COLOR);

	public void Render(Coord coord, string text, ConsoleColor color = DEFAULT_COLOR)
		=> Print(coord, text, color);
	public void Render(Coord coord, string text, MessageType messageType) 
		=> Print(coord, text, messageType switch{
			MessageType.Score => SCORE_COLOR,
			MessageType.DebugPositions => POSITIONS_COLOR,
			MessageType.PlayerDeath => PLAYER_DEATH_COLOR,
			MessageType.Restart => DEFAULT_COLOR,
			MessageType.BombOn => BOMB_ON_COLOR,
			MessageType.BombOff => BOMB_OFF_COLOR,
			MessageType.Default => DEFAULT_COLOR,
			_ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
		});

	private static void Print(Coord coord, char character, ConsoleColor color = DEFAULT_COLOR)
		=> Print(coord, $"{character}", color);
	private static void Print(Coord coord, string text, ConsoleColor color = DEFAULT_COLOR)
	{
		var currentColor = ForegroundColor;
		ForegroundColor = color;
		SetCursorPosition(coord.X + 1, coord.Y + 1);
		Write(text);
		ForegroundColor = currentColor;
	}

	public void Clear(Coord coord) 
		=> Print(coord, EMPTY_SYMBOL);
}