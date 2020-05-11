using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    public class Board
    {
		// 2D matric containing the board
        int[,] board;

		Texture2D texture;

		Rectangle size;

		public Board(Texture2D texture)
		{
			board = new int[,]
			{
				{1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{1, 1, 1, 1, 1, 1, 1, 1},
				{1, 1, 1, 1, 1, 1, 1, 1}
			};

			this.texture = texture;
			size = texture.Bounds;
		}

		/// <summary>
		/// Checks to see if a coordinate set has a piece
		/// </summary>
		/// <param name="coordinates">Vector2 of X & Y coordinates</param>
		/// <returns>bool</returns>
        public bool PieceAt(Vector2 coordinates)
		{
			int x = Convert.ToInt32(coordinates.X);
			int y = Convert.ToInt32(coordinates.Y); 

			if (board[x, y] == 1)
				return true;

			return false; 
		}

		/// <summary>
		/// Draw the board
		/// </summary>
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, size, Color.White);
		}
	}
}
