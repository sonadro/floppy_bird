using Godot;
using System;

namespace sonadro.FloppyBird
{
	public partial class Game : Node2D
	{
		private static readonly float PipeSpeed = 400f; // speed of pipes
		private static readonly float PipeDefaultPositionX = 1075f; // default X position of pipes when they reset to the right of the screen

		private static readonly int TopPipeMinValueY = -200; // minimum y value of top pipe (highest position)
		private static readonly int TopPipeMaxValueY = 40; // maximum y value of top pipe (lowest position)
		private static readonly float BottomPipeOffsetY = 825f; // y value of bottom pipe = y value of top pipe + BottomPipeOffsetY
		
		private static readonly Random Rand = new(); // rng

		private Vector2 MovementVector = new(-PipeSpeed, 0); // move to the left

		private Node Pipes;
		private Area2D TopPipe;
		private Area2D BottomPipe;
		private Area2D LeftBorder;

		public override void _Ready()
		{
			// fetch nodes
			Pipes = GetNode<Node>("Pipes");
			LeftBorder = GetNode<Area2D>("LeftBorder");

			TopPipe = Pipes.GetChild<Area2D>(0);
			BottomPipe = Pipes.GetChild<Area2D>(1);
		}

		private void CyclePipes(float delta)
		{
			// update pipe positions
			TopPipe.Position += MovementVector * delta;
			BottomPipe.Position += MovementVector * delta;
		}

		private void ResetPipes()
		{
			// randomize Y position for top pipe, between specified range
			float basePositionY = Rand.Next(TopPipeMinValueY, TopPipeMaxValueY);

			// move pipes to right, change y value
			TopPipe.Position = new Vector2(PipeDefaultPositionX, basePositionY);
			BottomPipe.Position = new Vector2(PipeDefaultPositionX, basePositionY + BottomPipeOffsetY); // bottim pipe is <BottomPipeOffsetY> pixels below top pipe
		}

		private void OnLeftBorderAreaEntered(Area2D area)
		{
			// if pipes touch the left border, reset pipes to the right
			if (area.Name.ToString().Contains("Pipe"))
			{
				ResetPipes();
			}
		}

		public override void _Process(double delta)
		{
			// update pipes
			CyclePipes((float)delta);
		}
	}
}
