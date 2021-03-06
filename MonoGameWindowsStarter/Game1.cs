﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

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

        Texture2D blackPawn;
        Texture2D whitePawn;
        SoundEffect pawnAttackSE;
        SoundEffect pawnMoveSE;
        SoundEffect pawnDeathSE;
        Texture2D blackRook;
        Texture2D whiteRook;
        Texture2D blackBishop;
        Texture2D whiteBishop;
        Texture2D blackKnight;
        Texture2D whiteKnight;
        Texture2D blackQueen;
        Texture2D whiteQueen;
        Texture2D blackKing;
        Texture2D whiteKing;

        Board board;

        List<IPiece> pieces;
        bool isPieceSelected;

        /// <summary>
        /// Particle system textures
        /// </summary>
        List<Texture2D> attackTextures;
        List<Texture2D> movementTextures;
        List<Texture2D> deathTextures;

        string turn;

        IPiece previousPiece;
        Vector2 previousPosition;
        string previousTurn;
        bool pieceKilled = false;
        IPiece killedPiece;

        bool playing;
        string winningTeam;
        SpriteFont font;

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
            playing = true;
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

            //Load in SpriteFont
            font = Content.Load<SpriteFont>("Font");

            //Load in songs
            songs[0] = Content.Load<Song>("chattle");
            songs[1] = Content.Load<Song>("goblin_scherzo");
            songs[2] = Content.Load<Song>("pixels_and_pawns");
            songs[3] = Content.Load<Song>("march_of_the_minotaur");
            songs[4] = Content.Load<Song>("halloween_morning");

            //Load in attack sound effects
            pawnAttackSE = Content.Load<SoundEffect>("Sounds/Attack/Attack08");

            //Load in move sound effects
            pawnMoveSE = Content.Load<SoundEffect>("Sounds/Movement/Movement06");

            //Load in Death sound effects
            pawnDeathSE = Content.Load<SoundEffect>("Sounds/Death/Death03");

            //Make the board
            Texture2D boardTexture = Content.Load<Texture2D>("Art/Board");
            board = new Board(boardTexture);

            //Import Pawn Textures
            blackPawn = Content.Load<Texture2D>("Art/Pieces/Goblin/BlackGoblin");
            whitePawn = Content.Load<Texture2D>("Art/Pieces/Goblin/WhiteGoblin");
            
            //Import Rook Textures
            blackRook = Content.Load<Texture2D>("Art/Pieces/Minotaur/BlackMinotaur");
            whiteRook = Content.Load<Texture2D>("Art/Pieces/Minotaur/WhiteMinotaur");

            //Import Bishop Textures
            blackBishop = Content.Load<Texture2D>("Art/Pieces/Gryphon/BlackGryphon");
            whiteBishop = Content.Load<Texture2D>("Art/Pieces/Gryphon/WhiteGryphon");

            //Import Knight Textures
            blackKnight = Content.Load<Texture2D>("Art/Pieces/Centaur/BlackCentaur");
            whiteKnight = Content.Load<Texture2D>("Art/Pieces/Centaur/WhiteCentaur");

            //Import Queen Textures
            blackQueen = Content.Load<Texture2D>("Art/Pieces/Dragon/BlackDragon");
            whiteQueen = Content.Load<Texture2D>("Art/Pieces/Dragon/WhiteDragon");

            // Create list of textures for attack 

            attackTextures = new List<Texture2D>();
            attackTextures.Add(Content.Load<Texture2D>("Art/Particles/Circle-Gray"));
            attackTextures.Add(Content.Load<Texture2D>("Art/Particles/Sparks"));
            attackTextures.Add(Content.Load<Texture2D>("Art/Particles/Circle-Orange"));

            movementTextures = new List<Texture2D>();
            movementTextures.Add(Content.Load<Texture2D>("Art/Particles/Circle-Gray"));

            deathTextures = new List<Texture2D>();
            deathTextures.Add(Content.Load<Texture2D>("Art/Particles/BloodParticle"));
            deathTextures.Add(Content.Load<Texture2D>("Art/Particles/BrownDirtParticle"));
            deathTextures.Add(Content.Load<Texture2D>("Art/Particles/WhiteDirtParticle"));



            //Import King Textures
            blackKing = Content.Load<Texture2D>("Art/Pieces/King/BlackKing");
            whiteKing = Content.Load<Texture2D>("Art/Pieces/King/WhiteKing");

            //Make white pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(64, i * 64);
                pieces.Add(new Pawn("white", position, whitePawn, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            }

            //Create knights
            pieces.Add(new Knight("white", new Vector2(0, 64), whiteKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Knight("white", new Vector2(0, 384), whiteKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create bishops
            pieces.Add(new Bishop("white", new Vector2(0, 320), whiteBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Bishop("white", new Vector2(0, 128), whiteBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create rooks
            pieces.Add(new Rook("white", new Vector2(0, 0), whiteRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Rook("white", new Vector2(0, 448), whiteRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create queen
            pieces.Add(new Queen("white", new Vector2(0, 192), whiteQueen, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create king
            pieces.Add(new King("white", new Vector2(0, 256), whiteKing, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Make black pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(384, i * 64);
                pieces.Add(new Pawn("black", position, blackPawn, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            }

            //Create knights
            pieces.Add(new Knight("black", new Vector2(448, 64), blackKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Knight("black", new Vector2(448, 384), blackKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create bishops
            pieces.Add(new Bishop("black", new Vector2(448, 320), blackBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Bishop("black", new Vector2(448, 128), blackBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create rooks
            pieces.Add(new Rook("black", new Vector2(448, 0), blackRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Rook("black", new Vector2(448, 448), blackRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create queen
            pieces.Add(new Queen("black", new Vector2(448, 192), blackQueen, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create king
            pieces.Add(new King("black", new Vector2(448, 256), blackKing, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
        }

        /// <summary>
        /// Removes all pieces, then replaces them on board in the correct location
        /// </summary>
        public void ResetBoard() {
            pieces.Clear();
            turn = "white";

            //Make white pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(64, i * 64);
                //Vector2 position = new Vector2(384, i * 64);

                pieces.Add(new Pawn("white", position, whitePawn, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            }

            //Create knights
            pieces.Add(new Knight("white", new Vector2(0, 64), whiteKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Knight("white", new Vector2(0, 384), whiteKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create bishops
            pieces.Add(new Bishop("white", new Vector2(0, 320), whiteBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Bishop("white", new Vector2(0, 128), whiteBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create rooks
            pieces.Add(new Rook("white", new Vector2(0, 0), whiteRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Rook("white", new Vector2(0, 448), whiteRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create queen
            pieces.Add(new Queen("white", new Vector2(0, 192), whiteQueen, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create king
            pieces.Add(new King("white", new Vector2(0, 256), whiteKing, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Make black pieces
            //Create pawns
            for (int i = 0; i < 8; i++)
            {
                Vector2 position = new Vector2(384, i * 64);
                pieces.Add(new Pawn("black", position, blackPawn, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            }

            //Create knights
            pieces.Add(new Knight("black", new Vector2(448, 64), blackKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Knight("black", new Vector2(448, 384), blackKnight, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create bishops
            pieces.Add(new Bishop("black", new Vector2(448, 320), blackBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Bishop("black", new Vector2(448, 128), blackBishop, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create rooks
            pieces.Add(new Rook("black", new Vector2(448, 0), blackRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));
            pieces.Add(new Rook("black", new Vector2(448, 448), blackRook, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create queen
            pieces.Add(new Queen("black", new Vector2(448, 192), blackQueen, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            //Create king
            pieces.Add(new King("black", new Vector2(448, 256), blackKing, pawnAttackSE, pawnMoveSE, pawnDeathSE, attackTextures, movementTextures, deathTextures));

            playing = true;
            winningTeam = "";
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
            if(mouseState.LeftButton == ButtonState.Pressed && playing)
            {
                foreach(IPiece piece in pieces)
                {
                    //Check if no piece is selected, the piece is on the right side, and the piece was clicked on
                    if(!isPieceSelected && piece.Side == turn && !piece.Dead && piece.CollidesWithPiece(mouseState.Position))
                    {
                        piece.Select();
                        previousPiece = piece;
                        previousPosition = previousPiece.Position;
                        isPieceSelected = true;
                    }
                    else if(piece.Selected)
                    {
                        //Get move location
                        Vector2 moveLocation = new Vector2((mouseState.Position.X / 64) * 64, (mouseState.Position.Y / 64) * 64);                      

                        bool movedPiece = false;

                        IPiece collidingPiece = null;
                        //Check if piece is attacking another piece
                        foreach(IPiece otherPiece in pieces)
                        {
                            if(otherPiece.CollidesWithPiece(moveLocation.ToPoint()) && !otherPiece.Dead)
                            {
                                collidingPiece = otherPiece;
                                break;
                            }
                        }

                        if(collidingPiece == null)
                        {
                            killedPiece = null;
                            pieceKilled = false;
                            piece.Move(moveLocation);
                            movedPiece = true;
                        }
                        else if(collidingPiece.Side != turn)
                        {
                            piece.Attack();
                            piece.Move(moveLocation);
                            killedPiece = collidingPiece;
                            pieceKilled = true;
                            collidingPiece.Kill();
                            movedPiece = true;

                            //Check for end game condition
                            if(collidingPiece.GetType() == typeof(King))
                            {
                                playing = false;
                                winningTeam = turn;
                            }
                        }

                        if(movedPiece)
                        {
                            isPieceSelected = false;
                            if (turn == "white")
                            {
                                previousTurn = "white";
                                turn = "black";
                            }
                            else
                            {
                                previousTurn = "black";
                                turn = "white";
                            }
                        }
                    }
                }
            }
            else if(mouseState.RightButton == ButtonState.Pressed && playing)
            {
                isPieceSelected = false;
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

            // Check for board reset
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                ResetBoard();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Back) && playing)
            {
                previousPiece.Move(previousPosition);
                turn = previousTurn;
                if(pieceKilled) 
                {
                      killedPiece.setState(AnimationState.Idle0);          
                }
                pieceKilled = false;
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

            if(!playing)
            {
                Vector2 location = font.MeasureString(winningTeam + " team won!");
                location.X = (graphics.GraphicsDevice.Viewport.Width / 2) - (location.X / 2);
                location.Y = graphics.GraphicsDevice.Viewport.Height / 4;
                spriteBatch.DrawString(font, winningTeam + " team won!", location - new Vector2(2, -2), Color.Black);
                spriteBatch.DrawString(font, winningTeam + " team won!", location, new Color(142, 237, 53));

                location = font.MeasureString("Press 'R' to play again!");
                location.X = (graphics.GraphicsDevice.Viewport.Width / 2) - (location.X / 2);
                location.Y = graphics.GraphicsDevice.Viewport.Height / 2;
                spriteBatch.DrawString(font, "Press 'R' to play again!", location - new Vector2(2, -2), Color.Black);
                spriteBatch.DrawString(font, "Press 'R' to play again!", location, new Color(142, 237, 53));
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
