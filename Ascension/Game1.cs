using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ascension;

/**
* Ascension is a game where the goal is to save Dr. Rodriguez from the 'EVIL MERGER'.
* The primary mechanic of the game is to platform vertically by using jump.
* A player can decide the strength of their jump by holding jump in longer, with a cap.
*/



public class Game1 : Game
{
    private Texture2D _playerAtlas;
    private Vector2 _playerPosition;
    private Texture2D _background;

    private float _viewportWidth;
    private float _viewportHeight;
    private float _worldWidth;
    private float _worldHeight;
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

        _background = Content.Load<Texture2D>("mountain_background_1080x20000");
        _worldWidth = _background.Width;
        _worldHeight = _background.Height;

        _playerAtlas = Content.Load<Texture2D>("player_atlas_32");
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here

        // assigned in Update(), in case user changes screen size
        _viewportWidth = GraphicsDevice.Viewport.Width;
        _viewportHeight = GraphicsDevice.Viewport.Height;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        Vector2 playerOffset = new(_viewportWidth, _viewportHeight);
        Vector2 scroll = _playerPosition - playerOffset;

        Vector2 minScroll = Vector2.Zero;
        Vector2 maxScroll = new(_worldWidth - _viewportWidth, _worldHeight - _viewportHeight);
        scroll = Vector2.Clamp(scroll, minScroll, maxScroll);
        Vector2 viewportOffset = -scroll;

        Matrix transform = Matrix.CreateTranslation(viewportOffset.X, viewportOffset.Y, 0);

        GraphicsDevice.Clear(new Color(90, 110, 130));
        _spriteBatch.Begin(transformMatrix: transform);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, _background.Width, _background.Height), Color.White);

        // atlas is 3x3, 32px each. frame 0 is top left, counts across then down
        Rectangle Frame(int i) => new Rectangle((i % 3) * 32, (i / 3) * 32, 32, 32);

        // cmsc245 - the story of one (1) assignment
        double t = gameTime.TotalGameTime.TotalSeconds;
        int scene = (int)(t / 1.5) % 7;
        int step = (int)(t * 8) % 2;   // flips every 1/8 sec, used for the run cycle

        int frame = 0;
        int x = 304;
        int y = 150;

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
                y += 8;   
                break;
            case 3:  
                frame = 3;
                y -= (int)(Math.Abs(Math.Sin(t * 4)) * 40);
                break;
            case 4:   
                frame = 4;
                y -= (int)(Math.Abs(Math.Sin(t * 4)) * 40);   
                break;
            case 5:   
                frame = 5 + step;
                x -= (int)((t * 60) % 80);
                break;
            case 6:   
                frame = 7 + step;
                x += (int)((t * 60) % 80);
                break;
        }

        _spriteBatch.Draw(_playerAtlas, new Rectangle(x, y, 192, 192), Frame(frame), Color.White);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
