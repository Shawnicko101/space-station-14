﻿using SS14.Client.Graphics;
using System;
using SS14.Shared.Maths;

namespace SS14.Client.Helpers
{
    public class StarScroller
    {
        private readonly Random _random = new Random();
        private Star[,] _stars;

        public StarScroller()
        {
            MakeStars();
        }

        private void MakeStars()
        {
            _stars = new Star[64,4];

            for (int layer = 0; layer < 4; layer++)
            {
                for (int i = 0; i < _stars.Length/4; i++)
                {
                    _stars[i, layer].Position = new Vector2((float) (_random.NextDouble()*CluwneLib.Window.Viewport.Size.X),
                                                             (float)
                                                             (_random.NextDouble()* CluwneLib.Window.Viewport.Height));

                    // Select magnitude.
                    switch (layer)
                    {
                        case 0:
                            _stars[i, layer].Magnitude = Color.White;
                            _stars[i, layer].VDelta = (float) (_random.NextDouble()*100.0) + 55.0f;
                            break;
                        case 1:
                            _stars[i, layer].Magnitude = new Color(192, 192, 192, 255);
                            _stars[i, layer].VDelta = (float) (_random.NextDouble()*50.0) + 27.5f;
                            break;
                        case 2:
                            _stars[i, layer].Magnitude = Color.Gray;
                            _stars[i, layer].VDelta = (float) (_random.NextDouble()*25.0) + 13.5f;
                            break;
                        default:
                            _stars[i, layer].Magnitude = new Color(64, 64, 64, 255);
                            _stars[i, layer].VDelta = (float) (_random.NextDouble()*12.5) + 1.0f;
                            break;
                    }
                }
            }
        }

        private void DrawStars(int layer, float deltaTime)
        {
            // Draw the stars.
            for (int i = 0; i < _stars.Length/4; i++)
            {
                CluwneLib.drawPoint((int) _stars[i, layer].Position.X, (int) _stars[i, layer].Position.Y,
                                                    _stars[i, layer].Magnitude);

                // Move the stars down.
                var position = _stars[i, layer].Position;
                var positionY = _stars[i, layer].VDelta*deltaTime;
                _stars[i, layer].Position = new Vector2(position.X, position.Y + positionY);

                // Wrap around.
                if (_stars[i, layer].Position.Y > CluwneLib.Window.Viewport.Height)
                    _stars[i, layer].Position =
                        new Vector2((float) (_random.NextDouble()* CluwneLib.Window.Viewport.Width), 0);
            }
        }

        public void Render(float xTopleft, float yTopleft)
        {
            var delta = CluwneLib.Time.FrameTime.TotalSeconds;
            CluwneLib.ClearCurrentRendertarget(Color.Black);
            DrawStars(3, (float)delta / 2000);
            DrawStars(2, (float)delta / 2000);
            for (int layer = 1; layer >= 0; layer--)
                DrawStars(layer, (float)delta / 2000);
        }

        #region Nested type: Star

        private struct Star
        {
            /// <summary>
            /// Magnitude of the star.
            /// </summary>
            public Color Magnitude;

            /// <summary>
            /// Position of the star.
            /// </summary>
            public Vector2 Position;

            /// <summary>
            /// Vertical delta.
            /// </summary>
            public float VDelta;
        }

        #endregion
    }
}
