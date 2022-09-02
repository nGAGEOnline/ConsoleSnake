﻿using Snake.Library.Enums;

namespace Snake.Library;

public class GameSettings
{
	public int Width { get; }
	public int Height { get; }
	
	public Difficulty Difficulty { get; }
	public bool DebugMode { get; }

	public GameSettings(int width, int height, Difficulty difficulty, bool debugMode = false)
	{
		Difficulty = difficulty;
		DebugMode = debugMode;
		Width = width;
		Height = height;
	}

	public int GetPointsByDifficulty()
	{
		return Difficulty switch
		{
			Difficulty.Beginner => 10,
			Difficulty.Easy => 15,
			Difficulty.Normal => 20,
			Difficulty.Hard => 25,
			Difficulty.Insane => 30,
			_ => 0
		};
	}
	
	public int GetDelayByDifficulty()
	{
		return Difficulty switch
		{
			Difficulty.Beginner => 200,
			Difficulty.Easy => 150,
			Difficulty.Normal => 100,
			Difficulty.Hard => 60,
			Difficulty.Insane => 40,
			_ => 500
		};
	}
}
