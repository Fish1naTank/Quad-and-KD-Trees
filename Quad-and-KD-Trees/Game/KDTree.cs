using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Quad_and_KD_Trees
{
    class KDTree
    {
        const int K = 2;
        uint capacity;
        Point medianPoint;
        private int _bestMedian = -1;

        private List<Point> xSortedList;
        private List<Point> ySortedList;
        private KDTree _root;
        private bool _split = false;
        private int _axis = 0;
        private KDTree[] _childTrees;

        //draw
        public RectangleShape SplitLine = new RectangleShape();

        //pass a point list to sort, pass null if we are a child tree
        public KDTree(List<Point> pPointList, uint pCapacity, KDTree pRoot = null)
        {
            if(pPointList != null && pPointList.Count > 1)
            {
                quickSort(0, pPointList.Count - 1, 0, pPointList);
                xSortedList = pPointList.ToList();
                quickSort(0, pPointList.Count - 1, 1, pPointList);
                ySortedList = pPointList.ToList();
            }

            capacity = pCapacity;
            if (pRoot != null) _root = pRoot;
        }

        public void GenerateTree(List<Point> pXSortedList = null, List<Point> pYSortedList = null, int pAxis = 0)
        {
            //select division axis
            _axis = pAxis % K;

            // check if we have a new data set
            if (pXSortedList != null) xSortedList = pXSortedList;
            if (pYSortedList != null) ySortedList = pYSortedList;

            if (xSortedList == null || ySortedList == null) return;

            //check if we can take it
            if (xSortedList.Count <= capacity) return;

            //over capacity require median split
            if (!_split)
            {
                //check if it is nessessary
                Vector2f largestSize = getLargestPointSize(xSortedList);
                //             0 
                //  0 | 1     ---
                //             1 
                if (_axis == 0)
                {
                    _bestMedian = (xSortedList.Count + 1) / 2;

                    //check for better median
                    Point lastPoint = xSortedList.Last();
                    for (int i = _bestMedian; i > 0; i--)
                    {
                        //check segmentSize with largest possible point size
                        float segmentSize = (lastPoint.position.X) - (xSortedList[i - 1].position.X);
                        if (segmentSize < largestSize.X)
                        {
                            //check if dividing will help anyway
                            //like if we have 30 objcts connected in a line
                            //make a cut so we can get to a better median
                            /**/
                            float cluseterSize = ySortedList.Last().position.Y - (ySortedList[i - 1].position.Y);
                            if (cluseterSize > largestSize.Y * 2)
                            {
                                break;
                            }
                            /**/

                            //if we run out of points
                            if(i == 1) return;
                        }
                        else
                        {
                            _bestMedian = i;
                            break;
                        }
                    }
                    
                }
                else //_axis == 1
                {
                    _bestMedian = (ySortedList.Count + 1) / 2;

                    //check for better median
                    Point lastPoint = ySortedList.Last();
                    for (int i = _bestMedian; i > 0; i--)
                    {
                        //check segmentSize with largest possible point size
                        float segmentSize = (lastPoint.position.Y) - (ySortedList[i - 1].position.Y);
                        if (segmentSize < largestSize.Y)
                        {
                            //check if dividing will help anyway
                            //like if we have 30 objcts connected in a line
                            //make a cut so we can get to a better median
                            /**/
                            float cluseterSize = xSortedList.Last().position.X - (xSortedList[i - 1].position.X);
                            if (cluseterSize > largestSize.X * 2)
                            {
                                break;
                            }
                            /**/

                            //if we run out of points
                            if (i == 1) return;
                        }
                        else
                        {
                            _bestMedian = i;
                            break;
                        }
                    }
                }

                split();
            }

            //give points to children
            List<Point> ylowMedianList;
            List<Point> yhighMedianList;
            List<Point> xlowMedianList;
            List<Point> xhighMedianList;

            // low = 0, high = 1
            //             0 
            //  0 | 1     ---
            //             1 
            if (_axis == 0)
            {
                if(_bestMedian == -1) _bestMedian = (xSortedList.Count + 1) / 2;
                medianPoint = xSortedList[_bestMedian - 1];

                xlowMedianList = xSortedList.GetRange(0, _bestMedian);
                xhighMedianList = xSortedList.GetRange(_bestMedian, xSortedList.Count - _bestMedian);

                //seperate y sorted list
                //try to preserve order
                ylowMedianList = ySortedList.ToList();
                yhighMedianList = ySortedList.ToList();
                foreach (Point p in xhighMedianList)
                {
                    ylowMedianList.Remove(p);
                }
                foreach (Point p in xlowMedianList)
                {
                    yhighMedianList.Remove(p);
                }
            }
            else //_axis == 1
            {
                if (_bestMedian == -1) _bestMedian = (ySortedList.Count + 1) / 2;
                medianPoint = ySortedList[_bestMedian - 1];

                //y first
                ylowMedianList = ySortedList.GetRange(0, _bestMedian);
                yhighMedianList = ySortedList.GetRange(_bestMedian, ySortedList.Count - _bestMedian);

                //seperate x sorted list
                //try to preserve order
                xlowMedianList = xSortedList.ToList();
                xhighMedianList = xSortedList.ToList();
                foreach (Point p in yhighMedianList)
                {
                    xlowMedianList.Remove(p);
                }
                foreach (Point p in ylowMedianList)
                {
                    xhighMedianList.Remove(p);
                }
            }

            //change axis and pass sorted lists
            int axis = _axis;
            axis += 1;

            _childTrees[0].GenerateTree(xlowMedianList, ylowMedianList, axis);
            _childTrees[1].GenerateTree(xhighMedianList, yhighMedianList, axis);
        }

        //get close points
        public List<Point> QueryRange(Boundry pRange, List<Point> pFound = null)
        {
            List<Point> found = pFound;
            if (found == null) found = new List<Point>();

            List<Point> pointList = _axis == 0 ? xSortedList : ySortedList;

            if (pointList == null || pRange == null) return found;

            if (!_split)
            {
                foreach (Point p in pointList)
                {
                    found.Add(p);
                }
            }
            else
            {
                int treeToSearch = treesToSearch(pRange);
                if (treeToSearch == 0 || treeToSearch == 1)
                {
                    _childTrees[treeToSearch].QueryRange(pRange, found);
                }
                else
                {
                    foreach (KDTree childTree in _childTrees)
                    {
                        childTree.QueryRange(pRange, found);
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

        private int treesToSearch(Boundry pRange)
        {
            //adjust range for our data set
            Boundry range = rangeAdjust(pRange);

            Vector2f[] edgs = new Vector2f[2];
            // 0 -
            // - 1
            edgs[0] = range.position - range.size;
            edgs[1] = range.position + range.size;

            // 0 | 1
            if (_axis == 0)
            {
                if (edgs[1].X <= medianPoint.position.X)
                {
                    return 0;
                }
                else if (edgs[0].X > medianPoint.position.X)
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
                if (edgs[1].Y <= medianPoint.position.Y)
                {
                    return 0;
                }
                else if (edgs[0].Y > medianPoint.position.Y)
                {
                    return 1;
                }

                return -1;
            }

            //should never happen
            return -2;
        }

        private Boundry rangeAdjust(Boundry pRange)
        {
            Vector2f sizeIncrease = getLargestPointSize(_axis == 0 ? xSortedList : ySortedList);

            if (sizeIncrease != new Vector2f(0, 0))
            {
                //if(rotating shapes)
                //float largerSize = (float)length(sizeIncrease);
                float largerSize = sizeIncrease.X > sizeIncrease.Y ? sizeIncrease.X : sizeIncrease.Y;
                sizeIncrease = new Vector2f(largerSize, largerSize);

                if (pRange is CircleBoundry)
                {
                    return new CircleBoundry(pRange.position, pRange.size.X + sizeIncrease.X);
                }
                else if (pRange is RectBoundry)
                {
                    return new RectBoundry(pRange.position, pRange.size + sizeIncrease);
                }
            }

            return pRange;
        }

        private Vector2f getLargestPointSize(List<Point> points)
        {
            Vector2f largestSize = new Vector2f(0, 0);
            foreach (Point point in points)
            {
                if (point != null)
                {
                    Vector2f pointSize = point.GetSize();
                    if (pointSize.X > largestSize.X || pointSize.Y > largestSize.Y)
                    {
                        largestSize = pointSize;
                    }
                }
            }
            return largestSize;
        }

        private Vector2f getSmallestPointSize(List<Point> points)
        {
            Vector2f smallestSize = new Vector2f(0, 0);
            foreach (Point point in points)
            {
                if (point != null)
                {
                    Vector2f pointSize = point.GetSize();
                    if (pointSize.X < smallestSize.X || pointSize.Y < smallestSize.Y)
                    {
                        smallestSize = pointSize;
                    }
                }
            }
            return smallestSize;
        }

        private void quickSort(int pStartIndex, int pEndIndex, int pAxis, List<Point> pPointList)
        {
            // no need for sort
            if (pStartIndex >= pEndIndex) return;

            int pivot = pEndIndex;
            int LastLow = pStartIndex;

            //iterate through list until all items are <= pivot are on the left, and are > on the right
            for (int i = pStartIndex; i <= pEndIndex; i++)
            {
                switch (pAxis)
                {
                    case 0:
                        if (pPointList[i].position.X <= pPointList[pivot].position.X)
                            swap(pPointList, LastLow++, i);
                        break;

                    case 1:
                        if (pPointList[i].position.Y <= pPointList[pivot].position.Y)
                            swap(pPointList, LastLow++, i);
                        break;
                }
            }

            //do this with each sub array, one to the left and right of the pivot
            quickSort(pStartIndex, LastLow - 2, pAxis, pPointList);
            quickSort(LastLow, pEndIndex, pAxis, pPointList);
        }

        private void swap(List<Point> pPointList, int a, int b)
        {
            if (a == b) return;
            Point temp = pPointList[a];
            pPointList[a] = pPointList[b];
            pPointList[b] = temp;
        }

        private double length(Vector2f v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }
    }
}
