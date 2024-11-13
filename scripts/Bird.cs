using Godot;

namespace sonadro.FloppyBird
{
	public partial class Bird : Area2D
	{
		private static readonly string ExitGameInput = "ui_cancel"; // input to close game
		private static readonly string JumpInput = "ui_accept"; // input to jump

		private static readonly float GravityForce = 50; // downwards gravity force
		private static readonly float JumpForce = 750; // jump force
		private Vector2 Velocity = Vector2.Zero;

		private AnimatedSprite2D Sprite;

		private bool Alive = true;

		public override void _Ready()
		{
			Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}

		private void OnAreaEntered(Area2D area)
		{
			if (area.Name.ToString().Contains("Pipe"))
			{
				Alive = false;
			}
		}

		private void ExitGame()
		{
			//TODO: save highscore
			GetTree().Quit();
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed(ExitGameInput))
			{
				ExitGame();
			}

			Velocity.Y += GravityForce;
			Velocity.Y = Mathf.Clamp(Velocity.Y, -JumpForce, JumpForce);

			if (Input.IsActionJustPressed(JumpInput))
			{
				Velocity.Y = -JumpForce;
			}

			if (Velocity.Y < 0)
			{
				Sprite.Play("wingdown");
			}
			else
			{
				Sprite.Play("wingup");
			}

			Position += Velocity * (float)delta;
		}
	}
}
