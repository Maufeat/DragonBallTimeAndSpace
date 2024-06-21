using System;
using System.Collections.Generic;
using System.Linq;

namespace lighttool.NavMesh
{
    public class navBorder
    {
        public string borderName;

        public int nodeA;

        public int nodeB;

        public int pointA;

        public int pointB;

        public float length;

        public navVec3 center;
    }

    public class FindNode
    {
        public void CalcHeuristic(navMeshInfo info, navVec3 endPos)
        {
            navVec3 center = info.nodes[this.nodeid].center;
            double num = Math.Abs(center.x - endPos.x);
            double num2 = Math.Abs(center.z - endPos.z);
            this.HValue = Math.Sqrt(num * num + num2 * num2);
        }

        public double GetCost(navMeshInfo info, int neighborID)
        {
            navVec3 center = info.nodes[neighborID].center;
            navVec3 center2 = info.nodes[this.nodeid].center;
            double num = center.x - center2.x;
            double num2 = center.y - center2.y;
            double num3 = center.z - center2.z;
            return Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        public int nodeid;

        public int pathSessionId;

        public int ParentID = -1;

        public bool Open;

        public double HValue;

        public double GValue;

        public int ArrivalWall;
    }
    
    public class navMeshInfo
    {
        public void calcBound()
        {
            this.min = new navVec3();
            this.max = new navVec3();
            this.min.x = double.MaxValue;
            this.min.y = double.MaxValue;
            this.min.z = double.MaxValue;
            this.max.x = double.MinValue;
            this.max.y = double.MinValue;
            this.max.z = double.MinValue;
            for (int i = 0; i < this.vecs.Length; i++)
            {
                if (this.vecs[i].x < this.min.x)
                {
                    this.min.x = this.vecs[i].x;
                }
                if (this.vecs[i].y < this.min.y)
                {
                    this.min.y = this.vecs[i].y;
                }
                if (this.vecs[i].z < this.min.z)
                {
                    this.min.z = this.vecs[i].z;
                }
                if (this.vecs[i].x > this.max.x)
                {
                    this.max.x = this.vecs[i].x;
                }
                if (this.vecs[i].y > this.max.y)
                {
                    this.max.y = this.vecs[i].y;
                }
                if (this.vecs[i].z > this.max.z)
                {
                    this.max.z = this.vecs[i].z;
                }
            }
        }

        private static double cross(navVec3 p0, navVec3 p1, navVec3 p2)
        {
            return (p1.x - p0.x) * (p2.z - p0.z) - (p2.x - p0.x) * (p1.z - p0.z);
        }

        public bool inPoly(navVec3 p, int[] poly)
        {
            float num = 0f;
            if (poly.Length < 3)
            {
                return false;
            }
            if (navMeshInfo.cross(this.vecs[poly[0]], p, this.vecs[poly[1]]) < (double)(-(double)num))
            {
                return false;
            }
            if (navMeshInfo.cross(this.vecs[poly[0]], p, this.vecs[poly[poly.Length - 1]]) > (double)num)
            {
                return false;
            }
            int i = 2;
            int num2 = poly.Length - 1;
            int num3 = -1;
            while (i <= num2)
            {
                int num4 = i + num2 >> 1;
                if (navMeshInfo.cross(this.vecs[poly[0]], p, this.vecs[poly[num4]]) < (double)(-(double)num))
                {
                    num3 = num4;
                    num2 = num4 - 1;
                }
                else
                {
                    i = num4 + 1;
                }
            }
            if (num3 > poly.Length || num3 - 1 > poly.Length || num3 - 1 < 0)
            {
                return false;
            }
            double num5 = navMeshInfo.cross(this.vecs[poly[num3 - 1]], p, this.vecs[poly[num3]]);
            return num5 > (double)num;
        }

        public void genBorder()
        {
            Dictionary<string, navBorder> dictionary = new Dictionary<string, navBorder>();
            for (int i = 0; i < this.nodes.Length; i++)
            {
                navNode navNode = this.nodes[i];
                for (int j = 0; j < navNode.borderByPoint.Length; j++)
                {
                    string text = navNode.borderByPoint[j];
                    if (!dictionary.ContainsKey(text))
                    {
                        dictionary[text] = new navBorder();
                        dictionary[text].borderName = text;
                        dictionary[text].nodeA = navNode.nodeID;
                        dictionary[text].nodeB = -1;
                        dictionary[text].pointA = -1;
                    }
                    else
                    {
                        dictionary[text].nodeB = navNode.nodeID;
                        if (dictionary[text].nodeA > dictionary[text].nodeB)
                        {
                            dictionary[text].nodeB = dictionary[text].nodeA;
                            dictionary[text].nodeB = navNode.nodeID;
                        }
                        navNode navNode2 = this.nodes[dictionary[text].nodeA];
                        navNode navNode3 = this.nodes[dictionary[text].nodeB];
                        for (int k = 0; k < navNode2.poly.Length; k++)
                        {
                            if (navNode3.poly.Contains(navNode2.poly[k]))
                            {
                                if (dictionary[text].pointA == -1)
                                {
                                    dictionary[text].pointA = navNode2.poly[k];
                                }
                                else
                                {
                                    dictionary[text].pointB = navNode2.poly[k];
                                }
                            }
                        }
                        int pointA = dictionary[text].pointA;
                        int pointB = dictionary[text].pointB;
                        double num = this.vecs[pointA].x - this.vecs[pointB].x;
                        double num2 = this.vecs[pointA].y - this.vecs[pointB].y;
                        double num3 = this.vecs[pointA].z - this.vecs[pointB].z;
                        dictionary[text].length = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
                        dictionary[text].center = new navVec3();
                        dictionary[text].center.x = this.vecs[pointA].x * 0.5 + this.vecs[pointB].x * 0.5;
                        dictionary[text].center.y = this.vecs[pointA].y * 0.5 + this.vecs[pointB].y * 0.5;
                        dictionary[text].center.z = this.vecs[pointA].z * 0.5 + this.vecs[pointB].z * 0.5;
                        dictionary[text].borderName = dictionary[text].nodeA + "-" + dictionary[text].nodeB;
                    }
                }
            }
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            string[] array = dictionary.Keys.ToArray<string>();
            for (int l = 0; l < array.Length; l++)
            {
                if (dictionary[array[l]].nodeB >= 0)
                {
                    dictionary2[array[l]] = dictionary[array[l]].borderName;
                }
            }
            this.borders = new Dictionary<string, navBorder>();
            foreach (KeyValuePair<string, navBorder> keyValuePair in dictionary)
            {
                if (dictionary2.ContainsKey(keyValuePair.Key))
                {
                    this.borders[dictionary2[keyValuePair.Key]] = keyValuePair.Value;
                }
            }
            for (int m = 0; m < this.nodes.Length; m++)
            {
                navNode navNode4 = this.nodes[m];
                List<string> list = new List<string>();
                for (int n = 0; n < navNode4.borderByPoint.Length; n++)
                {
                    if (dictionary2.ContainsKey(navNode4.borderByPoint[n]))
                    {
                        list.Add(dictionary2[navNode4.borderByPoint[n]]);
                    }
                }
                navNode4.borderByPoly = list.ToArray();
            }
        }

        public navVec3[] vecs;

        public navNode[] nodes;

        public Dictionary<string, navBorder> borders;

        public navVec3 min;

        public navVec3 max;
    }

    public class navNode
    {
        public void genBorder()
        {
            string[] array = new string[this.poly.Length];
            for (int i = 0; i < this.poly.Length; i++)
            {
                int num = i;
                int num2 = i + 1;
                if (num2 >= this.poly.Length)
                {
                    num2 = 0;
                }
                int num3 = this.poly[num];
                int num4 = this.poly[num2];
                if (num3 < num4)
                {
                    array[i] = num3 + "-" + num4;
                }
                else
                {
                    array[i] = num4 + "-" + num3;
                }
            }
            this.borderByPoint = array;
        }

        public string isLinkTo(navMeshInfo info, int nid)
        {
            if (this.nodeID == nid)
            {
                return null;
            }
            if (nid < 0)
            {
                return null;
            }
            for (int i = 0; i < this.borderByPoly.Length; i++)
            {
                string text = this.borderByPoly[i];
                if (info.borders.ContainsKey(text))
                {
                    if (info.borders[text].nodeA == nid || info.borders[text].nodeB == nid)
                    {
                        return text;
                    }
                }
            }
            return null;
        }

        public int[] getLinked(navMeshInfo info)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.borderByPoly.Length; i++)
            {
                string key = this.borderByPoly[i];
                if (info.borders.ContainsKey(key))
                {
                    int num;
                    if (info.borders[key].nodeA == this.nodeID)
                    {
                        num = info.borders[key].nodeB;
                    }
                    else
                    {
                        num = info.borders[key].nodeA;
                    }
                    if (num >= 0)
                    {
                        list.Add(num);
                    }
                }
            }
            return list.ToArray();
        }

        public void genCenter(navMeshInfo info)
        {
            this.center = new navVec3();
            this.center.x = 0.0;
            this.center.y = 0.0;
            this.center.z = 0.0;
            for (int i = 0; i < this.poly.Length; i++)
            {
                if (this.poly[i] < info.vecs.Length)
                {
                    this.center.x += info.vecs[this.poly[i]].x;
                    this.center.y += info.vecs[this.poly[i]].y;
                    this.center.z += info.vecs[this.poly[i]].z;
                }
            }
            this.center.x /= (double)this.poly.Length;
            this.center.y /= (double)this.poly.Length;
            this.center.z /= (double)this.poly.Length;
        }

        public int nodeID;

        public int[] poly;

        public string[] borderByPoly;

        public string[] borderByPoint;

        public navVec3 center;
    }

    public class navVec3
    {
        public navVec3()
        {
        }

        public navVec3(double _x, double _y, double _z, int _polyId = -1)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.polyId = _polyId;
        }

        public navVec3 clone()
        {
            return new navVec3
            {
                x = this.x,
                y = this.y,
                z = this.z
            };
        }

        public void ResetValue(double _x, double _y, double _z, int _polyId = -1)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.polyId = _polyId;
        }

        public static double DistAZ(navVec3 start, navVec3 end)
        {
            double num = end.x - start.x;
            double num2 = end.z - start.z;
            return Math.Sqrt(num * num + num2 * num2);
        }

        public static navVec3 NormalAZ(navVec3 start, navVec3 end)
        {
            double num = end.x - start.x;
            double num2 = end.z - start.z;
            double num3 = Math.Sqrt(num * num + num2 * num2);
            return new navVec3
            {
                x = num / num3,
                y = 0.0,
                z = num2 / num3
            };
        }

        public static navVec3 Cross(navVec3 start, navVec3 end)
        {
            return new navVec3
            {
                x = start.y * end.z - start.z * end.y,
                y = start.z * end.x - start.x * end.z,
                z = start.x * end.y - start.y * end.x
            };
        }

        public static double DotAZ(navVec3 start, navVec3 end)
        {
            return start.x * end.x + start.z * end.z;
        }

        public static double Angle(navVec3 start, navVec3 end)
        {
            double d = start.x * end.x + start.z * end.z;
            navVec3 navVec = navVec3.Cross(start, end);
            double num = Math.Acos(d);
            if (navVec.y < 0.0)
            {
                num = -num;
            }
            return num;
        }

        public static navVec3 Border(navVec3 start, navVec3 end, float dist)
        {
            navVec3 navVec = navVec3.NormalAZ(start, end);
            return new navVec3
            {
                x = start.x + navVec.x * (double)dist,
                y = start.y + navVec.y * (double)dist,
                z = start.z + navVec.z * (double)dist
            };
        }

        public double x;

        public double y;

        public double z;

        public int polyId = -1;
    }

    public class pathFinding
    {
        public static int[] calcAStarPolyPath(navMeshInfo info, int startPoly, int endPoly, navVec3 endPos = null, float offset = 0.1f)
        {
            List<FindNode> nodeFind = new List<FindNode>();
            foreach (navNode navNode in info.nodes)
            {
                FindNode findNode = new FindNode();
                findNode.nodeid = navNode.nodeID;
                nodeFind.Add(findNode);
            }
            if (endPos == null)
            {
                endPos = info.nodes[endPoly].center.clone();
            }
            FindNode findNode2 = nodeFind[startPoly];
            findNode2.nodeid = startPoly;
            int num = 1;
            bool flag = false;
            List<int> list = new List<int>();
            List<int> list2 = new List<int>();
            findNode2.pathSessionId = num;
            list.Add(startPoly);
            while (list.Count > 0)
            {
                FindNode findNode3 = nodeFind[list[list.Count - 1]];
                list.Remove(findNode3.nodeid);
                list2.Add(findNode3.nodeid);
                if (findNode3.nodeid == endPoly)
                {
                    flag = true;
                    break;
                }
                foreach (int num2 in info.nodes[findNode3.nodeid].getLinked(info))
                {
                    if (num2 >= 0)
                    {
                        FindNode findNode4 = nodeFind[num2];
                        if (findNode4 == null || findNode4.nodeid != num2)
                        {
                            return null;
                        }
                        if (findNode4.pathSessionId != num)
                        {
                            string text = info.nodes[findNode4.nodeid].isLinkTo(info, findNode3.nodeid);
                            if (text != null && info.borders[text].length >= offset * 2f)
                            {
                                findNode4.pathSessionId = num;
                                findNode4.ParentID = findNode3.nodeid;
                                findNode4.Open = true;
                                findNode4.CalcHeuristic(info, endPos);
                                findNode4.GValue = findNode3.GValue + findNode3.GetCost(info, findNode4.nodeid);
                                list.Add(findNode4.nodeid);
                                list.Sort(delegate (int x, int y)
                                {
                                    double num3 = nodeFind[x].HValue + nodeFind[x].GValue;
                                    double num4 = nodeFind[y].HValue + nodeFind[y].GValue;
                                    if (num3 < num4 - 0.001)
                                    {
                                        return 1;
                                    }
                                    if (num3 > num4 + 0.001)
                                    {
                                        return -1;
                                    }
                                    return 0;
                                });
                                findNode4.ArrivalWall = findNode3.nodeid;
                            }
                        }
                        else if (findNode4.Open)
                        {
                            if (findNode4.GValue + findNode4.GetCost(info, findNode3.nodeid) < findNode3.GValue)
                            {
                                findNode3.GValue = findNode4.GValue + findNode4.GetCost(info, findNode3.nodeid);
                                findNode3.ParentID = findNode4.nodeid;
                                findNode3.ArrivalWall = findNode4.nodeid;
                            }
                        }
                    }
                }
            }
            List<int> list3 = new List<int>();
            if (list2.Count != 0)
            {
                FindNode findNode5 = nodeFind[list2[list2.Count - 1]];
                list3.Add(findNode5.nodeid);
                while (findNode5.ParentID != -1)
                {
                    list3.Add(findNode5.ParentID);
                    findNode5 = nodeFind[findNode5.ParentID];
                }
            }
            if (!flag)
            {
                return null;
            }
            return list3.ToArray();
        }

        private static double NearAngle(double a, double b)
        {
            double num = a;
            if (a >= 180.0)
            {
                num = 360.0 - a;
            }
            double num2 = b;
            if (b >= 180.0)
            {
                num2 = 360.0 - b;
            }
            if (num < num2)
            {
                return a;
            }
            return b;
        }

        public static navVec3[] FindPath(navMeshInfo info, navVec3 startPos, navVec3 endPos, float offset = 0.1f)
        {
            int startPoly = -1;
            int endPoly = -1;
            for (int i = 0; i < info.nodes.Length; i++)
            {
                if (info.inPoly(startPos, info.nodes[i].poly))
                {
                    startPoly = i;
                }
                if (info.inPoly(startPos, info.nodes[i].poly))
                {
                    endPoly = i;
                }
            }
            int[] polyPath = pathFinding.calcAStarPolyPath(info, startPoly, endPoly, endPos, offset);
            return pathFinding.calcWayPoints(info, startPos, endPos, polyPath, offset);
        }

        public static navVec3[] calcWayPoints(navMeshInfo info, navVec3 startPos, navVec3 endPos, int[] polyPath, float offset = 0.1f)
        {
            List<navVec3> list = new List<navVec3>();
            if (polyPath.Length == 0 || startPos == null || endPos == null)
            {
                return null;
            }
            List<int> list2 = new List<int>(polyPath);
            list2.Reverse();
            list.Add(startPos);
            int num = 0;
            navVec3 navVec = null;
            int num2 = -1;
            navVec3 navVec2 = null;
            int num3 = -1;
            int num4 = 0;
            navVec3 navVec3 = null;
            navVec3 navVec4 = null;
            navVec3 start = startPos.clone();
            for (int i = 0; i < 100; i++)
            {
                for (int j = num; j < list2.Count; j++)
                {
                    if (j == list2.Count - 1)
                    {
                        if (navVec == null || navVec2 == null)
                        {
                            num4 = 0;
                            break;
                        }
                        navVec3 end = navVec3.NormalAZ(start, endPos);
                        double num5 = navVec3.Angle(navVec, end);
                        double num6 = navVec3.Angle(navVec2, end);
                        if (num5 * num6 <= 0.0)
                        {
                            num4 = 0;
                            break;
                        }
                        if (num5 > 0.0)
                        {
                            num4 = 1;
                        }
                        else
                        {
                            num4 = -1;
                        }
                    }
                    else
                    {
                        int num7 = list2[j];
                        int num8 = list2[j + 1];
                        string key = num7 + "-" + num8;
                        if (num8 < num7)
                        {
                            key = num8 + "-" + num7;
                        }
                        navBorder navBorder = info.borders[key];
                        navVec3 navVec5 = navVec3.Border(info.vecs[navBorder.pointA], info.vecs[navBorder.pointB], offset);
                        navVec3 navVec6 = navVec3.Border(info.vecs[navBorder.pointB], info.vecs[navBorder.pointA], offset);
                        double num9 = navVec3.DistAZ(start, navVec5);
                        double num10 = navVec3.DistAZ(start, navVec6);
                        if (num9 >= 0.001 && num10 >= 0.001)
                        {
                            if (navVec == null)
                            {
                                navVec = navVec3.NormalAZ(start, navVec5);
                                navVec3 = navVec5.clone();
                                num2 = j;
                            }
                            if (navVec2 == null)
                            {
                                navVec2 = navVec3.NormalAZ(start, navVec6);
                                navVec4 = navVec6.clone();
                                num3 = j;
                            }
                            double num11 = navVec3.Angle(navVec, navVec2);
                            if (num11 < 0.0)
                            {
                                navVec3 navVec7 = navVec;
                                navVec3 navVec8 = navVec3;
                                int num12 = num2;
                                navVec = navVec2;
                                navVec3 = navVec4;
                                num2 = num3;
                                navVec2 = navVec7;
                                navVec4 = navVec8;
                                num3 = num12;
                            }
                            if (num2 != j || num3 != j)
                            {
                                navVec3 navVec9 = navVec3.NormalAZ(start, navVec5);
                                navVec3 navVec10 = navVec3.NormalAZ(start, navVec6);
                                double num13 = navVec3.Angle(navVec9, navVec10);
                                if (num13 < 0.0)
                                {
                                    navVec3 navVec11 = navVec9;
                                    navVec3 navVec12 = navVec5;
                                    navVec9 = navVec10;
                                    navVec5 = navVec6;
                                    navVec10 = navVec11;
                                    navVec6 = navVec12;
                                }
                                double num14 = navVec3.Angle(navVec, navVec9);
                                double num15 = navVec3.Angle(navVec2, navVec9);
                                double num16 = navVec3.Angle(navVec, navVec10);
                                double num17 = navVec3.Angle(navVec2, navVec10);
                                if (num14 < 0.0 && num16 < 0.0)
                                {
                                    num4 = -1;
                                    break;
                                }
                                if (num15 > 0.0 && num17 > 0.0)
                                {
                                    num4 = 1;
                                    break;
                                }
                                if (num14 > 0.0 && num15 < 0.0)
                                {
                                    navVec = navVec9;
                                    navVec3 = navVec5;
                                    num2 = j;
                                }
                                if (num16 > 0.0 && num17 < 0.0)
                                {
                                    navVec2 = navVec10;
                                    navVec4 = navVec6;
                                    num3 = j;
                                }
                            }
                        }
                    }
                }
                if (num4 == 0)
                {
                    break;
                }
                if (num4 == -1)
                {
                    list.Add(navVec3.clone());
                    start = navVec3;
                    num = num2;
                }
                else
                {
                    list.Add(navVec4.clone());
                    start = navVec4;
                    num = num3;
                }
                navVec = null;
                navVec2 = null;
                num2 = -1;
                num3 = -1;
            }
            list.Add(endPos);
            return list.ToArray();
        }
    }

}
