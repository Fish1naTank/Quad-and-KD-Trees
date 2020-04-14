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
        Point medianPoint;

        private List<Point> pointList;
        private KDTree _root;
        private bool _split = false;
        private int _axis = 0;
        private KDTree[] _childTrees;

        //draw
        public RectangleShape SplitLine = new RectangleShape();

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
            _axis = pAxis % K;

            // check if we have a new data set
            if (pPointList != null)
            {
                pointList = pPointList;
            }

            if (pointList == null) return;

            quickSort(0, pointList.Count - 1, _axis);

            //check if we need to divide
            if (pointList.Count <= capacity) return;

            //over capacity require median split
            if(!_split)
            {
                split();
            }

            //give points to children
            int median = (pointList.Count + 1) / 2;
            medianPoint = pointList[median - 1];

            List<Point> lowMedianList = pointList.GetRange(0, median);
            List<Point> highMedianList = pointList.GetRange(median, pointList.Count - median);

            //           0 
            // 0 | 1    ---
            //           1 
            int axis = _axis;
            axis += 1;
            _childTrees[0].GenerateTree(lowMedianList, axis);
            _childTrees[1].GenerateTree(highMedianList, axis);
        }
        public List<Point> QueryRectangelRange(Boundry pRectRange, List<Point> pFound = null)
        {
            List<Point> found = pFound;
            if (found == null) found = new List<Point>();

            if (!_split)
            {
                foreach (Point p in pointList)
                {
                    if (pRectRange.Contains(p.position)) found.Add(p);
                }
            }
            else
            {
                int treeToSearch = treesToSearch(pRectRange);
                if (treeToSearch == 0 || treeToSearch == 1)
                {
                    _childTrees[treeToSearch].QueryRectangelRange(pRectRange, found);
                }
                else
                {
                    foreach (KDTree childTree in _childTrees)
                    {
                        childTree.QueryRectangelRange(pRectRange, found);
                    }
                }
            }
            return found;
        }

        public void DrawSplitLines(RenderWindow pWindow, Vector2f pWindowSize, Color pColor, Vector2f pOriginOffset)
        {
            if (medianPoint == null) return;

            //calculate line size and position
            Vector2f splitPos = new Vector2f(0,0);

            //draw virtical split
            if (_axis == 0)
            {
                splitPos.X = medianPoint.position.X;
                if (_root != null)
                {
                    splitPos.Y = _root.medianPoint.position.Y;
                }
                else
                {
                    SplitLine.Rotation = 90;
                }

                //line size
                SplitLine.Size = new Vector2f(pWindowSize.Y, 1);

                //
                // 0 | 1
                //
                //set childtree rotations
                if (_childTrees != null)
                {
                    _childTrees[0].SplitLine.Rotation = 180;
                    _childTrees[1].SplitLine.Rotation = 0;

                    //new split size
                    Vector2f splitWindowSize1 = pWindowSize - new Vector2f(splitPos.X - pOriginOffset.X, 0);
                    Vector2f splitWindowSize0 = pWindowSize - new Vector2f(splitWindowSize1.X, 0);

                    //recur
                    _childTrees[0].DrawSplitLines(pWindow, splitWindowSize0, pColor, pOriginOffset);
                    _childTrees[1].DrawSplitLines(pWindow, splitWindowSize1, pColor, pOriginOffset + new Vector2f(splitWindowSize0.X, 0));
                }
            }
            //draw horizontal split
            else if(_axis == 1)
            {
                splitPos.Y = medianPoint.position.Y;
                if (_root != null)
                {
                    splitPos.X = _root.medianPoint.position.X;
                }
                else
                {
                    SplitLine.Rotation = 0;
                }

                //line size
                SplitLine.Size = new Vector2f(pWindowSize.X, 1);

                //  0 
                // ---
                //  1 
                //set child tree rotations
                if (_childTrees != null)
                {
                    _childTrees[0].SplitLine.Rotation = -90;
                    _childTrees[1].SplitLine.Rotation = 90;

                    //new split size
                    Vector2f splitWindowSize1 = pWindowSize - new Vector2f(0, splitPos.Y - pOriginOffset.Y);
                    Vector2f splitWindowSize0 = pWindowSize - new Vector2f(0, splitWindowSize1.Y);

                    //recur
                    _childTrees[0].DrawSplitLines(pWindow, splitWindowSize0, pColor, pOriginOffset);
                    _childTrees[1].DrawSplitLines(pWindow, splitWindowSize1, pColor, pOriginOffset + new Vector2f(0, splitWindowSize0.Y));
                }
            }

            SplitLine.Position = splitPos;
            SplitLine.FillColor = pColor;

            pWindow.Draw(SplitLine);
        }

        private void split()
        {
            _childTrees = new KDTree[2];
            _childTrees[0] = new KDTree(null, capacity, this);
            _childTrees[1] = new KDTree(null, capacity, this);

            _split = true;
        }

        private int treesToSearch(Boundry pRectRange)
        {
            Vector2f[] points = new Vector2f[2];
            // 0 -
            // - 1
            points[0] = pRectRange.position - pRectRange.size;
            points[1] = pRectRange.position + pRectRange.size;

            // 0 | 1
            if (_axis == 0)
            {
                if (points[1].X < medianPoint.position.X)
                {
                    return 0;
                }
                else if(medianPoint.position.X < points[0].X)
                {
                    return 1;
                }

                return -1;
            }
            //  0
            // ---
            //  1
            else if (_axis == 1)
            {
                if (points[1].Y < medianPoint.position.Y)
                {
                    return 0;
                }
                else if (medianPoint.position.Y < points[0].Y)
                {
                    return 1;
                }

                return -1;
            }

            //should never happen
            return -2;
        }

        private void quickSort(int pStart, int pEnd, int pAxis)
        {
            // no need for sort
            if (pStart >= pEnd) return;

            int pivot = pEnd;
            int LastLow = pStart;

            //iterate through list until all items are <= pivot are on the left, and are > on the right
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

            //do this with each sub array, one to the left and right of the pivot
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
