using Godot;
using System.Text.Json;

namespace sonadro.FloppyBird
{
	public partial class Bird : Area2D
	{
		private static readonly string ExitGameInput = "ui_cancel"; // input to close game
		private static readonly string JumpInput = "ui_accept"; // input to jump
		private static readonly string SaveFileName = "user://save.json";

		private static readonly int BirdMinPositionY = 64;
		private static readonly int BirdMaxPositionY = 624;

		private static readonly float GravityForce = 50; // downwards gravity force
		private static readonly float JumpForce = 750; // jump force
		private Vector2 Velocity = Vector2.Zero; // bird velocity

		private RichTextLabel ScoreLabel;
		private RichTextLabel HighScoreLabel;

		private int Score = 0;
		private int HighScore;

		private AnimatedSprite2D Sprite;

		public override void _Ready()
		{
			// fetch nodes
			Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			ScoreLabel = GetNode<RichTextLabel>("../UI/ScoreLabel");
			HighScoreLabel = GetNode<RichTextLabel>("../UI/HighScoreLabel");

			// get HighScore from saveFile
			var file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Read);

			if (file != null)
			{
				// get data
				string fileData = file.GetAsText();
				SaveFile saveFile = JsonSerializer.Deserialize<SaveFile>(fileData);

				// update HighScore
				HighScore = saveFile.HighScore;
				HighScoreLabel.Text = HighScoreLabel.Text = $"[center]High Score: [color=green]{HighScore}";
			}
		}

		private void OnAreaEntered(Area2D area)
		{
			// if pipe and bird collides, exit game

			if (area.Name.ToString().Contains("Pipe"))
			{
				ExitGame();
			}
		}

		private void OnScoreCounterAreaEntered(Area2D area)
		{
			// if pipe passes bird, update score

			if (area.Name.ToString().Contains("Pipe"))
			{
				UpdateScore();
			}
		}

		private void UpdateScore()
		{
			// update score & label
			Score++;
			ScoreLabel.Text = $"[center]Score: [color=green]{Score}";

			// update high score & label
			if (Score > HighScore)
			{
				HighScore = Score;
				HighScoreLabel.Text = $"[center]High Score: [color=green]{HighScore}";
			}
		}

		private void ExitGame()
		{
			// create save file
			SaveFile saveFile = new(HighScore);
			string saveFileJson = JsonSerializer.Serialize(saveFile);

			// write to file
			var file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Write);
			file.StoreString(saveFileJson);

			// close access to file
			file.Close();

			// exit game
			GetTree().Quit();
		}

		public override void _Process(double delta)
		{
			// exit game input
			if (Input.IsActionJustPressed(ExitGameInput))
			{
				ExitGame();
			}

			// apply gravity & clamp vertical velocity
			Velocity.Y += GravityForce;
			Velocity.Y = Mathf.Clamp(Velocity.Y, -JumpForce, JumpForce);

			// jump
			if (Input.IsActionJustPressed(JumpInput))
			{
				Velocity.Y = -JumpForce;
			}

			// play correct animations
			if (Velocity.Y < 0)
			{
				Sprite.Play("wingdown");
			}
			else
			{
				Sprite.Play("wingup");
			}

			// update & clamp bird Y position
			Position += Velocity * (float)delta;
			Position = new Vector2(Position.X, Mathf.Clamp(Position.Y, BirdMinPositionY, BirdMaxPositionY));
		}
	}
}
