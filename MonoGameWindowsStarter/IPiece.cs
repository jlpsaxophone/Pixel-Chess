using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// An interface representing a chess piece
    /// </summary>
    interface IPiece
    {
        bool Selected { get; }

        void Move(Vector2 positionNew);

        void Select();

        void Kill();

        bool CollidesWithPiece(Point location);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
