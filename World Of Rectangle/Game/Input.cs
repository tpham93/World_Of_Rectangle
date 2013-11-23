using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace World_Of_Rectangle.Game
{
    class Input
    {
        public enum EMouseButton
        {
            LeftButton,
            MiddleButton,
            RightButton
        }

        private static MouseState mousestate;
        private static KeyboardState keyboardstate;

        private static Point lastPosition;
        private static Point currentPosition;

        private static bool[] currentMouseButtonStates;
        private static bool[] lastMouseButtonStates;

        private static Keys[] currentPressedKeys;
        private static Keys[] lastPressedKeys;

        private static int currentScrollValue;
        private static int lastScrollValue;

        private Input()
        {
        }

        public static void Initialize()
        {
            mousestate = Mouse.GetState();
            keyboardstate = Keyboard.GetState();

            lastPosition = new Point();
            currentPosition = new Point();

            currentMouseButtonStates = new bool[4];
            lastMouseButtonStates = new bool[4];

            currentPressedKeys = new Keys[0];
            lastPressedKeys = new Keys[0];

            currentScrollValue = mousestate.ScrollWheelValue;
            lastScrollValue = currentScrollValue;
        }

        public static void Update()
        {
            mousestate = Mouse.GetState();
            keyboardstate = Keyboard.GetState();

            lastPosition = currentPosition;
            currentPosition = new Point(mousestate.X, mousestate.Y);

            for (int i = 0; i < currentMouseButtonStates.Length; i++)
            {
                lastMouseButtonStates[i] = currentMouseButtonStates[i];
            }

            currentMouseButtonStates[(int)EMouseButton.LeftButton] = mousestate.LeftButton == ButtonState.Pressed;
            currentMouseButtonStates[(int)EMouseButton.MiddleButton] = mousestate.MiddleButton == ButtonState.Pressed;
            currentMouseButtonStates[(int)EMouseButton.RightButton] = mousestate.RightButton == ButtonState.Pressed;

            lastScrollValue = currentScrollValue;
            currentScrollValue = mousestate.ScrollWheelValue;

            lastPressedKeys = currentPressedKeys;
            currentPressedKeys = keyboardstate.GetPressedKeys();
        }

        public static bool hasMouseMoved()
        {
            return lastPosition != currentPosition;
        }

        public static Point mousePosition()
        {
            return currentPosition;
        }
        public static Vector2 mousePositionV()
        {
            return new Vector2(currentPosition.X, currentPosition.Y);
        }

        public static bool mouseButtonClicked(EMouseButton button)
        {
            return currentMouseButtonStates[(int)button] && !lastMouseButtonStates[(int)button];
        }
        public static bool mouseButtonReleased(EMouseButton button)
        {
            return !currentMouseButtonStates[(int)button] && lastMouseButtonStates[(int)button];
        }
        public static bool mouseButtonHold(EMouseButton button)
        {
            return currentMouseButtonStates[(int)button] && lastMouseButtonStates[(int)button];
        }
        public static bool mouseButtonPressed(EMouseButton button)
        {
            return currentMouseButtonStates[(int)button];
        }

        private static bool contains<T>(T elem, T[] array)
        {
            foreach (T e in array)
            {
                if (e.Equals(elem))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isKeyDown(Keys k)
        {
            return contains<Keys>(k, currentPressedKeys);
        }
        public static bool wasKeyDown(Keys k)
        {
            return contains<Keys>(k, lastPressedKeys);
        }

        public static bool keyClicked(Keys k)
        {
            return isKeyDown(k) && !wasKeyDown(k);
        }
        public static bool keyReleased(Keys k)
        {
            return !isKeyDown(k) && wasKeyDown(k);
        }
        public static bool keyHold(Keys k)
        {
            return isKeyDown(k) && wasKeyDown(k);
        }
        public static bool keyPressed(Keys k)
        {
            return isKeyDown(k);
        }

        public static bool hasScrolled()
        {
            return scrollChange() != 0;
        }

        public static bool scrolledUp()
        {
            return scrollChange() > 0;
        }

        public static bool scrolledDown()
        {
            return scrollChange() < 0;
        }

        public static int scrollChange()
        {
            return currentScrollValue - lastScrollValue;
        }
    }
}
