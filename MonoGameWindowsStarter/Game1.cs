using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Song[] songs;
        int songiterator = 0;

        Board board;

        List<IPiece> pieces;
        bool isPieceSelected;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //Adjust screen to the size of the board
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 512;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            songs = new Song[5];
            pieces = new List<IPiece>();
            isPieceSelected = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load in songs
            songs[0] = Content.Load<Song>("chattle");
            songs[1] = Content.Load<Song>("goblin_scherzo");
            songs[2] = Content.Load<Song>("pixels_and_pawns");
            songs[3] = Content.Load<Song>("march_of_the_minotaur");
            songs[4] = Content.Load<Song>("halloween_morning");

            //Load in attack sound effects
            SoundEffect pawnAttackSE = Content.Load<SoundEffect>("Sounds/Attack/Attack08");

            //Load in move sound effects
            SoundEffect pawnMoveSE = Content.Load<SoundEffect>("Sounds/Movement/Movement06");

            //Load in death sound effects
            SoundEffect pawndeathSE = Content.Load<SoundEffect>("Sounds/Death/Death03");

            //Make the board
            Texture2D boardTexture = Content.Load<Texture2D>("Art/Board");
            board = new Board(boardTexture);

            //Import Pawn Textures
            Texture2D blackPawn = Content.Load<Texture2D>("Art/Pieces/Goblin/BlackGoblin");
            Texture2D whitePawn = Content.Load<Texture2D>("Art/Pieces/Goblin/WhiteGoblin");

            //Make white pieces
            //Create pawns
            for(int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(i * 64, 382);
                pieces.Add(new Pawn(position, whitePawn, pawnAttackSE, pawnMoveSE, pawndeathSE));
            }

            //Create rooks
            //Create knights
            //Create bishops
            //Create queen
            //Create king

            //Make black pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(i * 64, 64);
                pieces.Add(new Pawn(position, blackPawn, pawnAttackSE, pawnMoveSE, pawndeathSE));
            }

            //Create rooks
            //Create knights
            //Create bishops
            //Create queen
            //Create king
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Check mouse
            MouseState mouseState = Mouse.GetState();
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                foreach(IPiece piece in pieces)
                {
                    //Check if piece was clicked on
                    if(piece.CollidesWithPiece(mouseState.Position))
                    {
                        //Check if a piece is already selected
                        if(isPieceSelected)
                        {
                            if (piece.Selected)
                                piece.Move();
                        }
                        else
                        {
                            piece.Select();
                            isPieceSelected = true;
                        }
                    }
                }
            }

            //Check if the next song needs to be played           
            if(MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(songs[songiterator]);
                songiterator++;
                if(songiterator > 4)
                {
                    songiterator = 0;
                }
            }

            //Update pieces
            foreach(IPiece piece in pieces)
            {
                piece.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Draw board
            board.Draw(spriteBatch);

            //Draw pieces
            foreach(IPiece piece in pieces)
            {
                piece.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
