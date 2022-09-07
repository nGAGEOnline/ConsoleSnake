﻿using Snake.Library;
using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Console.Abstractions;

public class ConsoleBomb : IBomb
{
	#region CONSTS
	private const char EMPTY_SYMBOL = ' ';
	private const char BOMB_SYMBOL = '█';
	private static readonly char[] BombExplosionSymbols = new char[]{ '█', '▓', '▒'};
	private const int BLINK_TIME = 250;
	#endregion
	
	public Coord Coord { get; }
	public int DetonationTime { get; }

	private readonly Coord[] _explosionCoords;
	private readonly IRenderer _renderer;
	private int _timeRemaining = 0;
	private bool _blinkOn;

	public ConsoleBomb(Coord coord, IRenderer renderer, int detonationTime = 10000)
	{
		_renderer = renderer;
		Coord = coord;
		DetonationTime = detonationTime;
		_timeRemaining = DetonationTime;
		_explosionCoords = new[] { Coord + Coord.Up, Coord + Coord.Down, Coord + Coord.Left, Coord + Coord.Right };
	}

	public async Task Activate()
	{
		await StartTimer();
		await Explosion();
	}

	public async Task StartTimer()
	{
		while (_timeRemaining >= 3000)
			await Blinking(BLINK_TIME * 4);
		
		while (_timeRemaining >= 0)
			await Blinking(BLINK_TIME);
	}
	private async Task Explosion()
	{
		var explosionIndex = 0;
		const int animationDelay = 300;
		foreach (var coord in _explosionCoords)
			_renderer.Render(coord, $"{BombExplosionSymbols[explosionIndex]}", MessageType.Default);
		_renderer.Render(Coord, $"{BOMB_SYMBOL}", MessageType.BombOn);

		await Task.Delay(animationDelay);

		explosionIndex++;
		foreach (var coord in _explosionCoords)
			_renderer.Render(coord, $"{BombExplosionSymbols[explosionIndex]}", MessageType.BombOn);
		_renderer.Render(Coord, $"{BOMB_SYMBOL}", MessageType.PlayerDeath);

		await Task.Delay(animationDelay);

		explosionIndex++;
		foreach (var coord in _explosionCoords)
			_renderer.Render(coord, $"{BombExplosionSymbols[explosionIndex]}", MessageType.BombOff);
		_renderer.Render(Coord, $"{EMPTY_SYMBOL}", MessageType.BombOff);

		await Task.Delay(animationDelay);

		foreach (var coord in _explosionCoords)
			_renderer.Render(coord, $"{EMPTY_SYMBOL}", MessageType.BombOff);
	}

	private async Task Blinking(int blinkTime)
	{
		_renderer.Render(Coord, $"{BOMB_SYMBOL}", _blinkOn ? MessageType.BombOn : MessageType.BombOff);
		await Task.Delay(blinkTime);
		_blinkOn = !_blinkOn;
		_timeRemaining -= BLINK_TIME;
	}
}