using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class KDTree
    {
        const int K = 2;
        uint capacity;
        List<Point> pointList;

        private KDTree _root;
        private bool _split = false;
        private KDTree[] _childTrees;

        //draw
        private RectangleShape _splitLine;

        public KDTree(List<Point> pPointList, uint pCapacity, KDTree pRoot = null)
        {
            pointList = pPointList;
            if (pointList != null) quickSort(0, pointList.Count - 1, 0);
            capacity = pCapacity;
            if (pRoot != null) _root = pRoot;
        }

        public void GenerateTree(List<Point> pPointList = null, int pAxis = 0)
        {
            //select division axis
            int axis = pAxis % K;

            // check if we have a new data set
            if (pPointList != null)
            {
                pointList = pPointList;
                quickSort(0, pointList.Count - 1, pAxis);
            }

            if (pointList == null) return;

            //check if we need to divide
            if(pointList.Count <= capacity) return;

            //over capacity require median split
            if(!_split)
            {
                split();
            }

            int medianIndex = (pointList.Count + 1) / 2;

            placeSplitLine(medianIndex, axis);

            List<Point> lowMedianList = pointList.GetRange(0, medianIndex);
            List<Point> highMedianList = pointList.GetRange(medianIndex, pointList.Count - medianIndex);

            _childTrees[0].GenerateTree(lowMedianList, axis);
            _childTrees[1].GenerateTree(highMedianList, axis);
        }

        public void DrawTree(RenderWindow pWindow, Color pColor)
        {
            _splitLine = new RectangleShape(new Vector2f(100, 1));
            _splitLine.FillColor = pColor;
            _splitLine.Rotation = 45;
            
            pWindow.Draw(_splitLine);
        }

        private void placeSplitLine(int pMedianIndex, int pAxis)
        {
            if (_splitLine == null) _splitLine = new RectangleShape();
            //calculate line size and position
        }

        private void split()
        {
            _childTrees = new KDTree[2];
            _childTrees[0] = new KDTree(null, capacity, this);
            _childTrees[1] = new KDTree(null, capacity, this);

            _split = true;
        }

        private void quickSort(int pStart, int pEnd, int pAxis)
        {
            // no need for sort
            if (pStart >= pEnd) return;

            int pivot = pEnd;
            int LastLow = pStart;

            for (int i = pStart; i <= pEnd; i++)
            {
                switch (pAxis)
                {
                    case 0:
                        if (pointList[i].position.X <= pointList[pivot].position.X)
                            swap(LastLow++, i);
                        break;

                    case 1:
                        if (pointList[i].position.Y <= pointList[pivot].position.Y)
                            swap(LastLow++, i);
                        break;
                }
            }

            quickSort(pStart, LastLow - 2, pAxis);
            quickSort(LastLow, pEnd, pAxis);
        }

        private void swap(int a, int b)
        {
            if (a == b) return;
            Point temp = pointList[a];
            pointList[a] = pointList[b];
            pointList[b] = temp;
        }
    }
}
