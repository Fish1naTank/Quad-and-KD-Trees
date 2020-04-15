
using SFML.Window;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class TreeManager
    {
        public enum TreeMode { NoTree, QuadTree, KDTree }
        public TreeMode treeMode = TreeMode.NoTree;
        public bool drawTrees = true;
        public bool drawPossiblePoints = true;

        public enum PointDrawMode { NoDraw, DrawPoint, DrawCloud }
        public PointDrawMode pointDrawMode = PointDrawMode.DrawPoint;
        public bool movingPoints = false;
        public bool collidingPoints = true;

        private List<Keyboard.Key> _keys = new List<Keyboard.Key>();
        private List<Mouse.Button> _buttons = new List<Mouse.Button>();
        public TreeManager() { }

        public bool KeyboardReleased(Keyboard.Key pKey)
        {
            if (Keyboard.IsKeyPressed(pKey))
            {
                if(!_keys.Contains(pKey))
                {
                    _keys.Add(pKey);
                }
                return false;
            }

            if (_keys.Contains(pKey) && !Keyboard.IsKeyPressed(pKey))
            {
                _keys.Remove(pKey);
                return true;
            }

            return false;
        }

        public bool MouseReleased(Mouse.Button pButton)
        {
            if (Mouse.IsButtonPressed(pButton))
            {
                if (!_buttons.Contains(pButton))
                {
                    _buttons.Add(pButton);
                }
                return false;
            }

            if (_buttons.Contains(pButton) && !Mouse.IsButtonPressed(pButton))
            {
                _buttons.Remove(pButton);
                return true;
            }

            return false;
        }
    }
}
