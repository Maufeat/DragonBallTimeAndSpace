using System;
using System.Collections.Generic;

public class Dijkstra
{
    private void Ppath(int[] path, int i, int v, List<int> roads)
    {
        int num = path[i];
        if (num == v)
        {
            return;
        }
        this.Ppath(path, num, v, roads);
        roads.Add(num);
    }

    private void Dispath(int[] dist, int[] path, int[] s, int n, int v, List<int> roads)
    {
        if (s[n] == 1)
        {
            roads.Add(v);
            this.Ppath(path, n, v, roads);
            roads.Add(n);
        }
        else
        {
            FFDebug.LogError("Dijkstra", string.Format("从{0}到{1}不存在路径\n", v, n));
        }
    }

    public void FindDijkstra(int v, int target, List<int> roads)
    {
        int num = 0;
        for (int i = 0; i < this.info.n; i++)
        {
            this.dist[i] = this.info.edges[v, i];
            this.s[i] = 0;
            if (this.info.edges[v, i] < 32767)
            {
                this.path[i] = v;
            }
            else
            {
                this.path[i] = -1;
            }
        }
        this.s[v] = 1;
        this.path[v] = 0;
        for (int i = 0; i < this.info.n; i++)
        {
            int num2 = 32767;
            for (int j = 0; j < this.info.n; j++)
            {
                if (this.s[j] == 0 && this.dist[j] < num2)
                {
                    num = j;
                    num2 = this.dist[j];
                }
            }
            this.s[num] = 1;
            for (int j = 0; j < this.info.n; j++)
            {
                if (this.s[j] == 0 && this.info.edges[num, j] < 32767 && this.dist[num] + this.info.edges[num, j] < this.dist[j])
                {
                    this.dist[j] = this.dist[num] + this.info.edges[num, j];
                    this.path[j] = num;
                }
            }
        }
        this.Dispath(this.dist, this.path, this.s, target, v, roads);
    }

    public void InitData(int[,] relation, int number)
    {
        this.dist = new int[number];
        this.path = new int[number];
        this.s = new int[number];
        this.info.n = number;
        this.info.edges = new int[number, number];
        for (int i = 0; i < this.info.n; i++)
        {
            for (int j = 0; j < this.info.n; j++)
            {
                this.info.edges[i, j] = relation[i, j];
            }
        }
    }

    public void Test()
    {
        this.info.n = 4;
        this.info.edges = new int[4, 4];
        int[,] relation = new int[,]
        {
            {
                0,
                5,
                32767,
                7
            },
            {
                5,
                0,
                4,
                1
            },
            {
                32767,
                4,
                0,
                32767
            },
            {
                7,
                1,
                32767,
                0
            }
        };
        this.InitData(relation, 4);
        FFDebug.LogError("Dijkstra", "最小生成树构成:");
        List<int> list = new List<int>();
        this.InitData(relation, 4);
        this.FindDijkstra(2, 3, list);
        for (int i = 0; i < list.Count; i++)
        {
            FFDebug.LogError(this, string.Format("roads {0} {1}", i, list[i]));
        }
    }

    private const int INF = 32767;

    private int[] dist;

    private int[] path;

    private int[] s;

    private Dijkstra.MGraph info = default(Dijkstra.MGraph);

    private struct VertexType
    {
        public string VexNo;

        public string VexName;

        public string otherInfo;
    }

    private struct MGraph
    {
        public int[,] edges;

        public int n;
    }
}
