﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGameAndroid1
{
    public abstract class State
    {
        public abstract string Id { get; }
        List<Keys> m_holdedKeys = new List<Keys>();
        MouseState m_prevMouseState;
        private bool m_exitGameRequest = false;
        private bool m_inputEnabled = true;
        private bool wereInputInLastFrame = false;
        private string requestedState = string.Empty;

        Vector2 m_pressedMousePos;
        Vector2 m_mousePos;

        public abstract void OnInit();
        public abstract void OnShutdown();
        public abstract void OnUpdate(GameTime gameTime);
        public abstract void OnQuit();
        public abstract void OnLoad(ContentManager content, GraphicsDevice graphics);
        public abstract void OnUnload(ContentManager content, GraphicsDevice graphics);
        public abstract void OnDraw(ref SpriteBatch spriteBatch);

        public virtual void OnEnter()
        {
            requestedState = string.Empty;
        }

        // input handling
        public virtual void OnKeyPressed(Keys pressedKey)
        {
        }

        public virtual void OnKeyHold(Keys holdedKey)
        {
        }

        public virtual void OnKeyReleased(Keys releasedKey)
        {
        }

        public virtual void OnMousePressed(Vector2 mousePos)
        {
        }

        public virtual void OnMouseHold(Vector2 mousePos, Vector2 movement)
        {
        }

        public virtual void OnMouseReleased(Vector2 mousePos, Vector2 movement)
        {
        }

        public void HandleInput()
        {
            if (!m_inputEnabled)
                return;
            HandleKeyboard();
            HandleMouse();
        }

        public bool IsRequestingGameExit()
        {
            return m_exitGameRequest;
        }

        public void RequestGameExit()
        {
            m_exitGameRequest = true;
        }

        public void RequestState(string state) => requestedState = state;

        public bool RequestingChangeState => requestedState.Length != 0;

        public string RequestedState => requestedState;

        public bool IsInputEnabled()
        {
            return m_inputEnabled;
        }

        public void EnableInput()
        {
            m_inputEnabled = true;
        }

        public void DisableInput()
        {
            m_inputEnabled = false;
        }

        protected void HandleKeyboard()
        {
            List<Keys> pressedKeys = new List<Keys>(Keyboard.GetState().GetPressedKeys());

            foreach (var key in pressedKeys)
            {
                if (m_holdedKeys.Contains(key))
                    OnKeyHold(key);
                else
                {
                    OnKeyPressed(key);
                    m_holdedKeys.Add(key);
                }
            }

            foreach (var key in m_holdedKeys)
                if (!pressedKeys.Contains(key))
                    OnKeyReleased(key);

            m_holdedKeys.RemoveAll(key => !pressedKeys.Contains(key));
        }

        protected void HandleMouse()
        {
            MouseState currState = Mouse.GetState();
            TouchCollection touchCollection = TouchPanel.GetState();
            bool isInputNow = touchCollection.Count > 0;

            if (isInputNow && !wereInputInLastFrame)
            {
                OnMousePressed(touchCollection[0].Position);
                m_pressedMousePos = touchCollection[0].Position;
            }
            else if (!isInputNow && wereInputInLastFrame)
            {
                OnMouseReleased(m_pressedMousePos, m_pressedMousePos);
            }

            Vector2 movement = currState.Position.ToVector2();
            if (m_prevMouseState.LeftButton == ButtonState.Pressed &&
                currState.LeftButton == ButtonState.Pressed)
            {
                movement -= m_prevMouseState.Position.ToVector2();
                OnMouseHold(currState.Position.ToVector2(), movement);
            }
            else if (m_prevMouseState.LeftButton == ButtonState.Pressed &&
                     currState.LeftButton != ButtonState.Pressed)
            {
                movement -= m_pressedMousePos;
                OnMouseReleased(currState.Position.ToVector2(), movement);
            }
            else if (currState.LeftButton == ButtonState.Pressed)
            {
                OnMousePressed(currState.Position.ToVector2());
                m_pressedMousePos = currState.Position.ToVector2();
            }

            wereInputInLastFrame = isInputNow;
            m_prevMouseState = Mouse.GetState();
        }
    }
}