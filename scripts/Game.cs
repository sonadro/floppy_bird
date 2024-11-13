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

		private Node Pipes;
		private Area2D TopPipe;
		private Area2D BottomPipe;
		private Area2D LeftBorder;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Pipes = GetNode<Node>("Pipes");
			LeftBorder = GetNode<Area2D>("LeftBorder");

			TopPipe = Pipes.GetChild<Area2D>(0);
			BottomPipe = Pipes.GetChild<Area2D>(1);
		}

		private void CyclePipes(float delta)
		{
			Vector2 MovementVector = new(-PipeSpeed, 0);

			TopPipe.Position += MovementVector * delta;
			BottomPipe.Position += MovementVector * delta;
		}

		private void ResetPipes()
		{
			float basePositionY = Rand.Next(TopPipeMinValueY, TopPipeMaxValueY);

			// move pipes to right, change y value
			TopPipe.Position = new Vector2(PipeDefaultPositionX, basePositionY);
			BottomPipe.Position = new Vector2(PipeDefaultPositionX, basePositionY + BottomPipeOffsetY);

			// min y value for top pipe is -200
			// max y value for top pipe is 40
			// y value of bottom pipe is y value of top pipe + 825
		}

		private void OnLeftBorderAreaEntered(Area2D area)
		{
			if (area.Name.ToString().Contains("Pipe"))
			{
				ResetPipes();
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			CyclePipes((float)delta);
		}
	}
}
