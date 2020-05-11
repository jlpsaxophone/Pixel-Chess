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

        string turn;

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
            turn = "white";
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
            
            //Import Rook Textures
            Texture2D blackRook = Content.Load<Texture2D>("Art/Pieces/Minotaur/BlackMinotaur");
            Texture2D whiteRook = Content.Load<Texture2D>("Art/Pieces/Minotaur/WhiteMinotaur");

            //Import Bishop Textures
            Texture2D blackBishop = Content.Load<Texture2D>("Art/Pieces/Gryphon/BlackGryphon");
            Texture2D whiteBishop = Content.Load<Texture2D>("Art/Pieces/Gryphon/WhiteGryphon");

            //Import Knight Textures
            Texture2D blackKnight = Content.Load<Texture2D>("Art/Pieces/Centaur/BlackCentaur");
            Texture2D whiteKnight = Content.Load<Texture2D>("Art/Pieces/Centaur/WhiteCentaur");

            //Import Queen Textures
            Texture2D blackQueen = Content.Load<Texture2D>("Art/Pieces/Dragon/BlackDragon");
            Texture2D whiteQueen = Content.Load<Texture2D>("Art/Pieces/Dragon/WhiteDragon");

            //Make white pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(64, i * 64);
                pieces.Add(new Pawn("white", position, whitePawn, pawnAttackSE, pawnMoveSE, pawndeathSE));
            }

            //Create knights
            //Create bishops
            //Create queen
            //Create king

            //Make black pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(384, i * 64);
                pieces.Add(new Pawn("black", position, blackPawn, pawnAttackSE, pawnMoveSE, pawndeathSE));
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
                    //Check if no piece is selected, the piece is on the right side, and the piece was clicked on
                    if(!isPieceSelected && piece.Side == turn && !piece.Dead && piece.CollidesWithPiece(mouseState.Position))
                    {
                        piece.Select();
                        isPieceSelected = true;
                    }
                    else if(piece.Selected && piece.IsValidMove(mouseState.Position))
                    {
                        //Get move location
                        Vector2 moveLocation = new Vector2((mouseState.Position.X / 64) * 64, (mouseState.Position.Y / 64) * 64);

                        bool movedPiece = false;

                        IPiece collidingPiece = null;
                        //Check if piece is attacking another piece
                        foreach(IPiece otherPiece in pieces)
                        {
                            if(otherPiece.CollidesWithPiece(moveLocation.ToPoint()))
                            {
                                collidingPiece = otherPiece;
                                break;
                            }
                        }

                        if(collidingPiece == null)
                        {
                            piece.Move(moveLocation);
                            movedPiece = true;
                        }
                        else if(collidingPiece.Side != turn)
                        {
                            piece.Attack();
                            piece.Move(moveLocation);
                            collidingPiece.Kill();
                            movedPiece = true;
                        }

                        if(movedPiece)
                        {
                            isPieceSelected = false;
                            if (turn == "white")
                            {
                                turn = "black";
                            }
                            else
                            {
                                turn = "white";
                            }
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
