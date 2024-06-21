using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Algorithms
{
    public class PathStraighten
    {
        private Vector2 NodeToV2(PathFinderNode data)
        {
            return new Vector2((float)data.X, (float)data.Y);
        }

        private bool IsFreePoint(float x, float y)
        {
            return x < (float)CellInfos.MapInfos.GetLength(0) && y < (float)CellInfos.MapInfos.GetLength(1) && x >= 0f && y >= 0f && !GraphUtils.IsBlockPointForPathfind(x, y);
        }

        private List<Vector2> GetLinePath(Vector2 scrp, Vector2 dstp)
        {
            if (!this.IsFreePoint(dstp.x, dstp.y))
            {
                return null;
            }
            List<Vector2> list = new List<Vector2>();
            float num = Vector2.Distance(scrp, dstp);
            for (float num2 = 0f; num2 < num; num2 += 1f)
            {
                Vector2 item = Vector2.Lerp(scrp, dstp, num2 / num);
                if (!this.IsFreePoint(item.x, item.y))
                {
                    return null;
                }
                list.Add(item);
            }
            list.Add(dstp);
            return list;
        }

        public List<cs_MoveData> GenPath(HashSet<PathFinderNode> AStarPath, Vector2 NextPosition)
        {
            if (AStarPath == null || AStarPath.Count <= 1)
            {
                return null;
            }
            this.vNextPosition = NextPosition;
            List<Vector2> list = new List<Vector2>();
            List<Vector2> list2 = new List<Vector2>();
            List<cs_MoveData> list3 = new List<cs_MoveData>();
            foreach (PathFinderNode data in AStarPath)
            {
                list.Add(this.NodeToV2(data));
            }
            List<Vector2> simpleLine = this.GetSimpleLine(list);
            if (simpleLine != null)
            {
                list2 = simpleLine;
            }
            else
            {
                this.GenNearSimplePath(list, ref list2);
            }
            if (list2.Count == 0)
            {
                list2 = list;
            }
            if (list2.Count < 2)
            {
                return list3;
            }
            cs_MoveData cs_MoveData = this.GenMoveData(list2[1], null);
            cs_MoveData last = cs_MoveData;
            list3.Add(cs_MoveData);
            for (int i = 2; i < list2.Count; i++)
            {
                list3.Add(last = this.GenMoveData(list2[i], last));
            }
            return list3;
        }

        public cs_MoveData GenMoveData(Vector2 p, cs_MoveData last = null)
        {
            cs_MoveData cs_MoveData = new cs_MoveData();
            if (last == null)
            {
                cs_MoveData.dir = CommonTools.GetServerDirByClientDir(new Vector2(p.x - this.vNextPosition.x, -(p.y - this.vNextPosition.y)));
            }
            else
            {
                cs_MoveData.dir = CommonTools.GetServerDirByClientDir(new Vector2(p.x - last.pos.fx, -(p.y - last.pos.fy)));
            }
            cs_MoveData.pos = new cs_FloatMovePos
            {
                fx = GraphUtils.Keep2DecimalPlaces(p.x),
                fy = GraphUtils.Keep2DecimalPlaces(p.y)
            };
            return cs_MoveData;
        }

        public cs_MoveData GenMoveData(Vector2 p, Vector2 last)
        {
            return new cs_MoveData
            {
                dir = CommonTools.GetServerDirByClientDir(new Vector2(p.x - last.x, -(p.y - last.y))),
                pos = new cs_FloatMovePos
                {
                    fx = GraphUtils.Keep2DecimalPlaces(p.x),
                    fy = GraphUtils.Keep2DecimalPlaces(p.y)
                }
            };
        }

        private void GenNearSimplePath(List<Vector2> astarPath, ref List<Vector2> outPath)
        {
            List<Vector2> list = null;
            Vector2 vector = astarPath[0];
            for (int i = 1; i < astarPath.Count - 1; i++)
            {
                Vector2 vector2 = astarPath[i];
                Vector2 dstp = astarPath[i + 1];
                List<Vector2> linePath = this.GetLinePath(vector, dstp);
                if (linePath == null)
                {
                    if (list == null)
                    {
                        outPath.Add(vector);
                    }
                    else
                    {
                        list.RemoveAt(list.Count - 1);
                        outPath = outPath.Concat(list).ToList<Vector2>();
                    }
                    vector = vector2;
                }
                list = linePath;
            }
            if (list != null)
            {
                outPath = outPath.Concat(list).ToList<Vector2>();
            }
            if (outPath.Count > 0)
            {
                Vector2 end = outPath[outPath.Count - 1];
                int num = astarPath.FindLastIndex((Vector2 x) => x == end);
                if (num != -1)
                {
                    while (++num < astarPath.Count)
                    {
                        outPath.Add(astarPath[num]);
                    }
                }
            }
        }

        private List<Vector2> GetSimpleLine(List<Vector2> ls)
        {
            return this.GetLinePath(ls[0], ls[ls.Count - 1]);
        }

        private Vector2 vNextPosition;
    }
}
