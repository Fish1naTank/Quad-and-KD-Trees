
using SFML.Window;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class TreeManager
    {
        public static uint NumOfTrees = 3;
        public enum TreeMode { NoTree, QuadTree, KDTree }
        public TreeMode treeMode = TreeMode.NoTree;
        public uint treeCapacity = 1;
        public bool drawTrees = false;
        public bool drawPossiblePoints = false;

        public enum PointDrawMode { NoDraw, DrawPoint, DrawCloud }
        public PointDrawMode pointDrawMode = PointDrawMode.DrawPoint;
        public bool shapeType = true;
        public bool collidingPoints = false;
        public bool movingPoints = false;
        public bool enableWindowBoundry = false;
        public bool varyingPointSize = false;

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
