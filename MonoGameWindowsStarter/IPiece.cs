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

        string Side { get; }

        bool Dead { get; }

        Vector2 Position { get; }

        void Move(Vector2 positionNew);

        void Select();

        void Kill();
        
        void Attack();

        bool CollidesWithPiece(Point location);

        bool IsValidMove(Point location);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        AnimationState State { get; }

        void setState(AnimationState state);
    }
}
