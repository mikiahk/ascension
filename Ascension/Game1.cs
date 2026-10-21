using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ascension;

/**
* Ascension is a game where the goal is to rescue Ryan from the top of the Mountain.
* The primary mechanic of the game is to platform vertically by using jump.
* A player can decide the strength of their jump by holding jump in longer, with a cap.
*/



public class Game1 : Game
{
    private const int PlayerSize = 64;
    private Texture2D _playerAtlas;
    private Vector2 _playerPosition;
    private Texture2D _background;

    private float _viewportWidth;
    private float _viewportHeight;
    private int _movement = 10;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        //batches sprites
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(_spriteBatch);

        _viewportWidth = GraphicsDevice.Viewport.Width;
        _viewportHeight = GraphicsDevice.Viewport.Height;

        _background = Content.Load<Texture2D>(@"images\Pixel Mountain");
        _playerPosition = new((_background.Width - PlayerSize) / 2, _background.Height - PlayerSize);

        _playerAtlas = Content.Load<Texture2D>(@"images\player_atlas_32");
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here

        // reassigned in Update(), in case user changes screen size
        _viewportWidth = GraphicsDevice.Viewport.Width;
        _viewportHeight = GraphicsDevice.Viewport.Height;

        KeyboardState keyboardInput = Keyboard.GetState();
        
        if (keyboardInput.IsKeyDown(Keys.W) ||
            keyboardInput.IsKeyDown(Keys.Up))
        {
            _playerPosition.Y -= _movement;
        }
        else if (keyboardInput.IsKeyDown(Keys.S) ||
                 keyboardInput.IsKeyDown(Keys.Down))
        {
            _playerPosition.Y += _movement;
        }
        else if (keyboardInput.IsKeyDown(Keys.A) ||
                 keyboardInput.IsKeyDown(Keys.Left))
        {
            _playerPosition.X -= _movement;
        }
        else if (keyboardInput.IsKeyDown(Keys.D) ||
                 keyboardInput.IsKeyDown(Keys.Right))
        {
            _playerPosition.X += _movement;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(new Color(90, 110, 130));

        // atlas is 3x3, 32px each. frame 0 is top left, counts across then down
        Rectangle Frame(int i) => new Rectangle((i % 3) * 32, (i / 3) * 32, 32, 32);

        double t = gameTime.TotalGameTime.TotalSeconds;
        int scene = (int)(t / 1.5) % 7;
        int step = (int)(t * 8) % 2;   // flips every 1/8 sec, used for the run cycle

        int frame = 0;

        switch (scene)
        {
            case 0:   
                frame = 0;
                break;
            case 1:   
                frame = 1;
                break;
            case 2:   
                frame = 2;
                _playerPosition.Y += 8;   
                break;
            case 3:  
                frame = 3;
                _playerPosition.Y -= (int)(Math.Abs(Math.Sin(t * 4)) * 40);
                break;
            case 4:   
                frame = 4;
                _playerPosition.Y -= (int)(Math.Abs(Math.Sin(t * 4)) * 40);   
                break;
            case 5:   
                frame = 5 + step;
                _playerPosition.X -= (int)((t * 60) % 80);
                break;
            case 6:   
                frame = 7 + step;
                _playerPosition.X += (int)((t * 60) % 80);
                break;
        }

        Vector2 playerCenter = _playerPosition + new Vector2(PlayerSize / 2f, PlayerSize / 2f);
        Vector2 viewportCenter = new(_viewportWidth / 2, _viewportHeight / 2);
        Vector2 viewportOffset = viewportCenter - playerCenter;
        Matrix transform = Matrix.CreateTranslation(viewportOffset.X, viewportOffset.Y, 0);
        _spriteBatch.Begin(transformMatrix: transform);
        _spriteBatch.Draw(_background, new Rectangle(0, 0, _background.Width, _background.Height), Color.White);
        _spriteBatch.Draw(_playerAtlas, new Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, PlayerSize, PlayerSize), Frame(frame), Color.White);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
