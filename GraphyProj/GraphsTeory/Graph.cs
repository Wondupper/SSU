using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphsTeory
{
    public class Graph
    {


        public Dictionary<string, Dictionary<string, double>> MainGraph = new Dictionary<string, Dictionary<string, double>>();
        public Dictionary<int, string> Sopostavitel = new Dictionary<int, string>();
        public Dictionary<string, int> DeSopostavitel = new Dictionary<string, int>();
        public Dictionary<string, int> nov = new Dictionary<string, int>();
        public string type;

        //////////////////////////////Конструктор обыкновенный
        public Graph() { }

        //////////////////////////////Конструктор только типа
        public Graph(string type) { this.type = type; }

        //////////////////////////////Конструктор по файлу
        public Graph(string Path, string type)
        {
            using (StreamReader file = new StreamReader(Path))
            {
                try
                {
                    this.type = type;
                    if (type == "t1")
                    {
                        while (!file.EndOfStream)
                        {
                            string Point = file.ReadLine();
                            Dictionary<string, double> Link = new Dictionary<string, double>();
                            string LinkString = file.ReadLine();
                            if (String.IsNullOrEmpty(LinkString))
                            {
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                            else
                            {
                                char[] c = { ' ' };
                                List<string> Points = LinkString.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList();
                                for (int i = 0; i < Points.Count; i++)
                                {
                                    Link.Add(Points[i], 1);
                                }
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                        }
                    }
                    if (type == "t2")
                    {
                        while (!file.EndOfStream)
                        {
                            string Point = file.ReadLine();
                            Dictionary<string, double> Link = new Dictionary<string, double>();
                            string LinkString = file.ReadLine();
                            if (String.IsNullOrEmpty(LinkString))
                            {
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                            else
                            {
                                char[] c = { ' ' };
                                List<string> PointsAndWeights = LinkString.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList();
                                for (int i = 0; i < PointsAndWeights.Count; i += 2)
                                {
                                    Link.Add(PointsAndWeights[i], double.Parse(PointsAndWeights[i + 1]));
                                }
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                        }
                    }
                    if (type == "t3")
                    {
                        while (!file.EndOfStream)
                        {
                            string Point = file.ReadLine();
                            Dictionary<string, double> Link = new Dictionary<string, double>();
                            string LinkString = file.ReadLine();
                            if (String.IsNullOrEmpty(LinkString))
                            {
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                            else
                            {
                                char[] c = { ' ' };
                                List<string> Points = LinkString.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList();
                                for (int i = 0; i < Points.Count; i++)
                                {
                                    Link.Add(Points[i], 1);
                                }
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                        }
                    }
                    if (type == "t4")
                    {
                        while (!file.EndOfStream)
                        {
                            string Point = file.ReadLine();
                            Dictionary<string, double> Link = new Dictionary<string, double>();
                            string LinkString = file.ReadLine();
                            if (String.IsNullOrEmpty(LinkString))
                            {
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                            else
                            {
                                char[] c = { ' ' };
                                List<string> PointsAndWeights = LinkString.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList();
                                for (int i = 0; i < PointsAndWeights.Count; i += 2)
                                {
                                    Link.Add(PointsAndWeights[i], double.Parse(PointsAndWeights[i + 1]));
                                }
                                MainGraph.Add(Point, Link);
                                nov.Add(Point, 0);
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Мультиграф не поддерживается");
                }
            }
            Console.WriteLine("Создание графа прошло успешно");
        }

        //////////////////////////////Констурктор копии 
        public Graph(Graph graph)
        {
            try
            {
                foreach (var Point in graph.MainGraph)
                {
                    Dictionary<string, double> Link = new Dictionary<string, double>();
                    foreach (var Points in Point.Value)
                    {
                        Link.Add(Points.Key, Points.Value);
                    }
                    MainGraph.Add(Point.Key, Link);
                    this.type = graph.type;
                    nov.Add(Point.Key, 0);
                    foreach (var Points in graph.Sopostavitel)
                    {
                        this.Sopostavitel.Add(Points.Key, Points.Value);
                    }
                    foreach (var Points in graph.DeSopostavitel)
                    {
                        this.DeSopostavitel.Add(Points.Key, Points.Value);
                    }
                }

                //Console.WriteLine("Создание копии графа прошло успешно");
            }
            catch
            {
                Console.WriteLine("Мультиграф не поддерживается");
            }
        }

        //////////////////////////////Очистка словаря обхода
        public void ClearNov()
        {
            foreach (var Point in MainGraph)
            {
                nov[Point.Key] = 0;
            }
            //Console.WriteLine("Очистка массива для обходов прошла успешно");
        }

        //////////////////////////////Создание сопоставителей
        public void SetSopostavitels()
        {
            int i = 0;
            foreach (var Point in MainGraph)
            {
                Sopostavitel.Add(i, Point.Key);
                DeSopostavitel.Add(Point.Key, i);
                i++;
            }
        }

        //////////////////////////////Создание матрицы смежности
        public double[,] CreateMatrixMinW()
        {
            double[,] array = new double[MainGraph.Count, MainGraph.Count];
            for (int i = 0; i < MainGraph.Count; i++)
            {
                for (int j = 0; j < MainGraph.Count; j++)
                {
                    array[i, j] = 0;
                }
            }
            int q1 = 0, q2 = 0;
            foreach (var Point in MainGraph)
            {
                foreach (var Points in Point.Value)
                {
                    array[DeSopostavitel[Point.Key], DeSopostavitel[Points.Key]] = Points.Value;
                    q2++;
                }
                q2 = 0;
                q1++;
            }
            for (int i = 0; i < MainGraph.Count; i++)
            {
                for (int j = 0; j < MainGraph.Count; j++)
                {
                    if (array[i, j] == 0)
                    {
                        array[i, j] = int.MaxValue;
                    }
                }
            }
            return array;
        }
        public double[,] CreateMatrix()
        {
            double[,] array = new double[MainGraph.Count, MainGraph.Count];
            for (int i = 0; i < MainGraph.Count; i++)
            {
                for (int j = 0; j < MainGraph.Count; j++)
                {
                    array[i, j] = 0;
                }
            }
            int q1 = 0, q2 = 0;
            foreach (var Point in MainGraph)
            {
                foreach (var Points in Point.Value)
                {
                    array[DeSopostavitel[Point.Key], DeSopostavitel[Points.Key]] = Points.Value;
                    q2++;
                }
                q2 = 0;
                q1++;
            }
            return array;
        }

        //////////////////////////////Копирование словаря обхода

        public Dictionary<string, int> CopyNov()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            foreach (var key in nov)
            {
                temp.Add(key.Key, key.Value);
            }
            return temp;
        }

        //////////////////////////////Очистка сопоставителей
        public void ClearSopostavitels()
        {
            Sopostavitel.Clear();
            DeSopostavitel.Clear();
        }

        //////////////////////////////Показ графа
        public void ShowMainGraph()
        {
            if (this.type == "t1")
            {
                foreach (var Point in MainGraph)
                {
                    Console.Write("{0}", Point.Key);
                    Console.WriteLine();
                    if (Point.Value.Count != 0)
                    {
                        foreach (var Points in Point.Value)
                        {
                            Console.Write("{0} ", Points.Key);
                        }
                    }
                    Console.WriteLine();
                }
            }
            if (this.type == "t2")
            {
                foreach (var Point in MainGraph)
                {
                    Console.Write("{0}", Point.Key);
                    Console.WriteLine();
                    if (Point.Value.Count != 0)
                    {
                        foreach (var Points in Point.Value)
                        {
                            Console.Write("{0} {1} ", Points.Key, Points.Value);
                        }
                    }
                    Console.WriteLine();
                }
            }
            if (this.type == "t3")
            {
                foreach (var Point in MainGraph)
                {
                    Console.Write("{0}", Point.Key);
                    Console.WriteLine();
                    if (Point.Value.Count != 0)
                    {
                        foreach (var Points in Point.Value)
                        {
                            Console.Write("{0} ", Points.Key);
                        }
                    }
                    Console.WriteLine();
                }
            }
            if (this.type == "t4")
            {
                foreach (var Point in MainGraph)
                {
                    Console.Write("{0}", Point.Key);
                    Console.WriteLine();
                    if (Point.Value.Count != 0)
                    {
                        foreach (var Points in Point.Value)
                        {
                            Console.Write("{0} {1} ", Points.Key, Points.Value);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
        //
        //////////////////////////////Добавление точки
        public void AddPoint(string Point)
        {
            Dictionary<string, double> Link = new Dictionary<string, double>();
            if (MainGraph.ContainsKey(Point))
            {
                Console.WriteLine("Ошибка: Такая точка уже есть");
            }
            else
            {
                MainGraph.Add(Point, Link);
                nov.Add(Point, 0);
                Console.WriteLine("Добавление новой точки прошло успешно");
            }
        }

        //////////////////////////////Добавление пути между точками
        public void AddWayFromPoints(string Point1, string Point2, double Weight)
        {
            try
            {
                if (this.type == "t2")
                {
                    if (MainGraph.ContainsKey(Point1) && MainGraph.ContainsKey(Point2) && Weight != 0 && Point1 != Point2)
                    {
                        MainGraph[Point1].Add(Point2, Weight);
                        MainGraph[Point2].Add(Point1, Weight);
                        Console.WriteLine("Создание ребра прошло успешно");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: нарушено одно из условий:");
                        Console.WriteLine("* обе вершины должны быть в графе");
                        Console.WriteLine("* В неориентированном графе не должно быть петель");
                    }
                }
                if (this.type == "t4")
                {
                    if (MainGraph.ContainsKey(Point1) && MainGraph.ContainsKey(Point2) && Weight != 0)
                    {
                        MainGraph[Point1].Add(Point2, Weight);
                        Console.WriteLine("Создание ребра прошло успешно");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: нарушено одно из условий:");
                        Console.WriteLine("* обе вершины должны быть в графе");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Мультиграф не поддерживается");
            }
        }

        public void AddWayFromPoints(string Point1, string Point2)
        {
            try
            {
                if (this.type == "t1")
                {
                    if (MainGraph.ContainsKey(Point1) && MainGraph.ContainsKey(Point2) && Point1 != Point2)
                    {
                        MainGraph[Point1].Add(Point2, 1);
                        MainGraph[Point2].Add(Point1, 1);
                        Console.WriteLine("Создание ребра прошло успешно");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: нарушено одно из условий:");
                        Console.WriteLine("* обе вершины должны быть в графе");
                        Console.WriteLine("* В неориентированном графе не должно быть петель");
                    }
                }
                if (this.type == "t3")
                {
                    if (MainGraph.ContainsKey(Point1) && MainGraph.ContainsKey(Point2))
                    {
                        MainGraph[Point1].Add(Point2, 1);
                        Console.WriteLine("Создание ребра прошло успешно");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: нарушено одно из условий:");
                        Console.WriteLine("* обе вершины должны быть в графе");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Мультиграф не поддерживается");
            }
        }

        //////////////////////////////Удаление точки
        public void DeletePoint(string Point)
        {
            Dictionary<string, double> Link = new Dictionary<string, double>();
            if (!MainGraph.ContainsKey(Point))
            {
                Console.WriteLine("Ошибка: Такой точки нет");
            }
            else
            {
                MainGraph.Remove(Point);
                nov.Remove(Point);
                foreach (string Points in MainGraph.Keys)
                {
                    if (MainGraph[Points].ContainsKey(Point))
                    {
                        MainGraph[Points].Remove(Point);
                    }
                }
                Console.WriteLine("Удаление точки прошло успешно");
            }
        }

        //////////////////////////////Удаление пути между точками
        public void DeleteWayFromPoints(string Point1, string Point2)
        {
            if (this.type == "t1" || this.type == "t2")
            {
                if (MainGraph.ContainsKey(Point1) && MainGraph.ContainsKey(Point2))
                {
                    if (MainGraph[Point1].ContainsKey(Point2) && MainGraph[Point2].ContainsKey(Point1))
                    {
                        MainGraph[Point1].Remove(Point2);
                        MainGraph[Point2].Remove(Point1);
                        Console.WriteLine("Удаление ребра прошло успешно");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: нарушено одно из условий:");
                    Console.WriteLine("* обе вершины должны быть в графе");
                }
            }
            if (this.type == "t3" || this.type == "t4")
            {
                if (MainGraph.ContainsKey(Point1) && MainGraph.ContainsKey(Point2))
                {
                    if (MainGraph[Point1].ContainsKey(Point2) && MainGraph[Point2].ContainsKey(Point1))
                    {
                        MainGraph[Point1].Remove(Point2);
                        MainGraph[Point2].Remove(Point1);
                        Console.WriteLine("Удаление ребра прошло успешно");
                    }
                    else if (!MainGraph[Point1].ContainsKey(Point2) && MainGraph[Point2].ContainsKey(Point1))
                    {
                        MainGraph[Point2].Remove(Point1);
                        Console.WriteLine("Удаление ребра прошло успешно");
                    }
                    else if (MainGraph[Point1].ContainsKey(Point2) && !MainGraph[Point2].ContainsKey(Point1))
                    {
                        MainGraph[Point1].Remove(Point2);
                        Console.WriteLine("Удаление ребра прошло успешно");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: нарушено одно из условий:");
                    Console.WriteLine("* обе вершины должны быть в графе");
                }
            }
        }


        //////////////////////////////Запись в файл
        public void WrtiteToFile(string Path)
        {
            using (StreamWriter file = new StreamWriter(Path))
            {
                if (this.type == "t1")
                {
                    foreach (var Point in MainGraph)
                    {
                        file.Write(Point.Key);
                        file.WriteLine();
                        if (Point.Value.Count == 0)
                        {

                        }
                        else
                        {
                            foreach (var Points in Point.Value)
                            {
                                file.Write("{0} ", Points.Key);
                            }
                        }
                        file.WriteLine();
                    }
                }
                if (this.type == "t2")
                {
                    foreach (var Point in MainGraph)
                    {
                        file.Write(Point.Key);
                        file.WriteLine();
                        if (Point.Value.Count == 0)
                        {

                        }
                        else
                        {
                            foreach (var Points in Point.Value)
                            {
                                file.Write("{0} {1} ", Points.Key, Points.Value);
                            }
                        }
                        file.WriteLine();
                    }
                }
                if (this.type == "t3")
                {
                    foreach (var Point in MainGraph)
                    {
                        file.Write(Point.Key);
                        file.WriteLine();
                        if (Point.Value.Count == 0)
                        {

                        }
                        else
                        {
                            foreach (var Points in Point.Value)
                            {
                                file.Write("{0} ", Points.Key);
                            }
                        }
                        file.WriteLine();
                    }
                }
                if (this.type == "t4")
                {
                    foreach (var Point in MainGraph)
                    {
                        file.Write(Point.Key);
                        file.WriteLine();
                        if (Point.Value.Count == 0)
                        {

                        }
                        else
                        {
                            foreach (var Points in Point.Value)
                            {
                                file.Write("{0} {1} ", Points.Key);
                            }
                        }
                        file.WriteLine();
                    }
                }
            }
            Console.WriteLine("Запись в файл прошла успешно");
        }

        //////////////////////////////Обход в глубину
        public void Dfs(string isv, Dictionary<string, string> from, List<List<string>> AllCycles)
        {
            if (MainGraph.ContainsKey(isv))
            {
                nov[isv] = 1;
                foreach (var inv in MainGraph[isv])
                {
                    if (inv.Value != 0 && nov[inv.Key] == 0)
                    {
                        from[inv.Key] = isv;
                        Dfs(inv.Key, from, AllCycles);
                    }
                    else if (inv.Value != 0 && nov[inv.Key] == 1)
                    {
                        List<string> cycle = new List<string>();
                        cycle.Add(inv.Key);
                        cycle.Add(isv);
                        string temp = isv;
                        while (true)
                        {
                            if (from[temp] == cycle[0])
                            {
                                cycle.Add(cycle[0]);
                                break;
                            }
                            else
                            {
                                cycle.Add(from[temp]);
                                temp = from[temp];
                            }
                        }
                        if (cycle[0] == cycle[1] && cycle[1] == cycle[2])
                        {
                            cycle.Remove(cycle[0]);
                        }
                        cycle.Reverse();
                        AllCycles.Add(cycle);
                    }
                }
                nov[isv] = 2;
            }
            else
            {
                Console.WriteLine("Такой точки нет в графе");
            }
        }

        //////////////////////////////Обход в ширину
        public void Bfs(string v)
        {
            if (MainGraph.ContainsKey(v))
            {
                Queue<string> queue = new Queue<string>();
                queue.Enqueue(v);
                nov[v] = 1;
                while (queue.Count != 0)
                {
                    v = queue.Dequeue();
                    foreach (var Points in MainGraph[v])
                    {
                        if (Points.Value != 0 && nov[Points.Key] == 0)
                        {
                            queue.Enqueue(Points.Key);
                            nov[Points.Key] = 1;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Такой точки нет в графе");
            }
        }

        public bool Bfs(double[,] graph, int start, int end, int[] parent)
        {
            bool[] visited = new bool[parent.Length];
            Queue<int> queue = new Queue<int>();

            visited[start] = true;
            queue.Enqueue(start);
            parent[start] = -1;

            while (queue.Any())
            {
                int u = queue.Dequeue();

                for (int v = 0; v < graph.GetLength(1); v++)
                {
                    if (!visited[v] && graph[u, v] > 0)
                    {
                        visited[v] = true;
                        queue.Enqueue(v);
                        parent[v] = u;
                    }
                }
            }

            return visited[end];
        }

        //////////////////////////////Дийкстра 7 задача

        public void Dijkstr(string ver,string u)
        {
            SetSopostavitels();
            Console.WriteLine(DeSopostavitel[u]);
            int v = DeSopostavitel[ver];
            double[,] array = CreateMatrixMinW();
            bool[] nov2 = new bool[MainGraph.Count];
            for (int i = 0; i < nov2.Length; i++)
            {
                nov2[i] = true;
            }
            int[] p;
            double[] d = dijkstr(v, out p, array, nov2);
            Console.WriteLine("Длина кратчайшие пути от вершины {0} до вершины {1} ", Sopostavitel[v], u);
            for (int i = 0; i < MainGraph.Count; i++)
            {
                if (i != v && i == DeSopostavitel[u])
                {
                    Console.Write("{0} равна {1}, ", Sopostavitel[i], d[i]);
                    Console.Write("путь ");
                    if (d[i] != int.MaxValue)
                    {
                        Stack items = new Stack();
                        WayDijkstr(v, i, p, ref items, array);
                        while (items.Count != 0)
                        {
                            Console.Write("{0} ", items.Pop());
                        }
                    }
                    Console.WriteLine();
                    break;
                }
            }
            ClearSopostavitels();
            ClearNov();
        }

        public double[] dijkstr(int v, out int[] p, double[,] array, bool[] nov2)
        {
            nov2[v] = false;
            double[,] c = new double[MainGraph.Count, MainGraph.Count];
            for (int i = 0; i < MainGraph.Count; i++)
            {
                for (int u = 0; u < MainGraph.Count; u++)
                {
                    if (array[i, u] == 0 || i == u)
                    {
                        c[i, u] = int.MaxValue;
                    }
                    else
                    {
                        c[i, u] = array[i, u];
                    }
                }
            }
            double[] d = new double[MainGraph.Count];
            p = new int[MainGraph.Count];
            for (int u = 0; u < MainGraph.Count; u++)
            {
                if (u != v)
                {
                    d[u] = c[v, u];
                    p[u] = v;
                }
            }
            for (int i = 0; i < MainGraph.Count - 1; i++)
            {
                double min = int.MaxValue;
                int w = 0;
                for (int u = 0; u < MainGraph.Count; u++)
                {
                    if (nov2[u] && min > d[u])
                    {
                        min = d[u];
                        w = u;
                    }
                }
                nov2[w] = false;
                for (int u = 0; u < MainGraph.Count; u++)
                {
                    double distance = d[w] + c[w, u];
                    if (nov2[u] && d[u] > distance)
                    {
                        d[u] = distance;
                        p[u] = w;
                    }
                }
            }
            return d;
        }
        public void WayDijkstr(int a, int b, int[] p, ref Stack items, double[,] array)
        {
            items.Push(Sopostavitel[b]);
            if (a == p[b])
            {
                items.Push(Sopostavitel[a]);
            }
            else
            {
                WayDijkstr(a, p[b], p, ref items, array);
            }
        }

        //////////////////////////////Беллман-Форд 9 задача

        public void BellmanFord(string v,string v1,string v2)
        {
            SetSopostavitels();

            HashSet<string> NegCycles = FindIntrEdges();// Сет в котором будет хранить отрицательные циклы


            Dictionary<string,double> dist = new Dictionary<string,double>();
            foreach (string key in MainGraph.Keys)
            {
                dist.Add(key, double.MaxValue);
            }
            dist[v] = 0;
            for (int i = 1; i <= MainGraph.Keys.Count - 1;i++)
            {
                foreach (var Point in MainGraph)
                {
                    foreach (var Points in Point.Value)
                    {
                        if (dist[Point.Key] != int.MaxValue && dist[Point.Key] + Points.Value < dist[Points.Key] && !NegCycles.Contains(Point.Key+" "+Points.Key))// Добавлена 3 проверка, что ребро которое проверяем не содержится в сете отрицаьельных циклов
                            dist[Points.Key] = dist[Point.Key] + Points.Value;
                    }
                }

            }

            foreach (var pars in dist)
            {
                if (pars.Key == v1 || pars.Key == v2)
                {
                    Console.WriteLine("{0},{1}", pars.Key, pars.Value);
                }
            }
        }

        public HashSet<string> FindIntrEdges() // метод для поиска всех отрицательных циклов.
        {
            // Сначала ищем все циклы графа. Делаем это с помощью алгоритма Флойда с восстановлением пути.
            double[,] array = new double[MainGraph.Count, MainGraph.Count];
            for (int i = 0; i < MainGraph.Count; i++)
            {
                for (int j = 0; j < MainGraph.Count; j++)
                {
                    array[i, j] = 0;
                }
            }
            int q1 = 0, q2 = 0;
            foreach (var Point in MainGraph)
            {
                foreach (var Points in Point.Value)
                {
                    array[DeSopostavitel[Point.Key], DeSopostavitel[Points.Key]] = 1;
                    q2++;
                }
                q2 = 0;
                q1++;
            }

            int[,] p;
            array = floyd(out p, array);

            // Идея в следующем: за цикл будем считать кротчайшее растояние от вершины a до b и от b до a.

            List<List<string>> AllCycles = new List<List<string>>();
            for (int i = 0; i < MainGraph.Count; i++)
            {
                for (int j = 0; j < MainGraph.Count; j++)
                {
                    if (i != j)
                    {
                        if (array[i, j] != int.MaxValue)
                        {
                            List<string> first = new List<string>();
                            List<string> second = new List<string>();
                            Queue items = new Queue();
                            items.Enqueue(i);
                            WayFloyd(i, j, p, ref items);// Находим кротчайший путь от a до b
                            items.Enqueue(j);
                            while (items.Count != 0)
                            {
                                first.Add(Sopostavitel[int.Parse(items.Dequeue().ToString())]);
                            }
                            items.Enqueue(j);
                            WayFloyd(j, i, p, ref items);// Находим кротчайший путь от b до a
                            items.Enqueue(i);
                            while (items.Count != 0)
                            {
                                second.Add(Sopostavitel[int.Parse(items.Dequeue().ToString())]);
                            }
                            second.Remove(second.First());
                            first.AddRange(second);// Соединяем путь получая цикл
                            AllCycles.Add(first);// Добавляем цикл в список всех циклов
                        }
                    }
                }
            }

            HashSet<string> NegCycles = new HashSet<string>();// Сет в который будет записывать отрицательные циклы

            foreach (List<string> ls in AllCycles)// Ищем отрицательные циклы
            {
                double sum = 0;
                for (int i = 0; i < ls.Count - 1; i++)// Считаем сумму ребер i цикла
                {
                    sum += (MainGraph[ls[i]])[ls[i + 1]];
                }
                if (sum < 0)// Если сумма отрицательная значит нашли отрицательный цикл и записываем его в сет.
                {
                    for (int i = 0; i < ls.Count - 1; i++)
                    {
                        NegCycles.Add(ls[i] + " " + ls[i + 1]);
                    }
                }
            }

            return NegCycles;
        }

        //////////////////////////////Флойд 8 задача

        public void Floyd()
        {
            SetSopostavitels();
            double[,] array = CreateMatrixMinW();
            int[,] p;
            double[,] a = floyd(out p, array);
            int i, j;
            for (i = 0; i < MainGraph.Count; i++)
            {
                for (j = 0; j < MainGraph.Count; j++)
                {
                    if (i != j)
                    {
                        if (a[i, j] == int.MaxValue)
                        {
                            Console.WriteLine("Пути из вершины {0} в вершину {1} не существует", Sopostavitel[i], Sopostavitel[j]);
                        }
                        else
                        {
                            Console.WriteLine("Кратчайший путь от вершины " + Sopostavitel[i] + " до вершины " + Sopostavitel[j] + " равен " + a[i, j].ToString());
                        }
                    }
                }
            }
            ClearSopostavitels();
        }
        public double[,] floyd(out int[,] p, double[,] array)
        {
            int i, j, k;
            double[,] a = new double[MainGraph.Count, MainGraph.Count];
            p = new int[MainGraph.Count, MainGraph.Count];
            for (i = 0; i < MainGraph.Count; i++)
            {
                for (j = 0; j < MainGraph.Count; j++)
                {
                    if (i == j)
                    {
                        a[i, j] = 0;
                    }
                    else
                    {
                        if (array[i, j] == 0)
                        {
                            a[i, j] = int.MaxValue;
                        }
                        else
                        {
                            a[i, j] = array[i, j];
                        }
                    }
                    p[i, j] = -1;
                }
            }
            for (k = 0; k < MainGraph.Count; k++)
            {
                for (i = 0; i < MainGraph.Count; i++)
                {
                    for (j = 0; j < MainGraph.Count; j++)
                    {
                        double distance = a[i, k] + a[k, j];
                        if (a[i, j] > distance)
                        {
                            a[i, j] = distance;
                            p[i, j] = k;
                        }
                    }
                }
            }
            return a;
        }

        public void WayFloyd(int a, int b, int[,] p, ref Queue items)
        {
            int k = p[a, b];
            if (k != -1)
            {
                WayFloyd(a, k, p, ref items);
                items.Enqueue(k);
                WayFloyd(k, b, p, ref items);
            }
        }

        ////////////////////////////// 1 Задача

        public void Get_Points_Degree()
        {
            if (this.type == "t3" || this.type == "t4")
            {
                foreach (var Point in MainGraph)
                {
                    int sum = 0;
                    sum += Point.Value.Count;
                    foreach (var Point2 in MainGraph)
                    {
                        if (Point2.Key != Point.Key)
                        {
                            if (Point2.Value.ContainsKey(Point.Key))
                            {
                                sum += 1;
                            }
                        }
                    }
                    Console.WriteLine("{0} {1}", Point.Key, sum);
                }
            }
        }

        ////////////////////////////// 2 Задача

        public void Neighbours(string Point1, string Point2)
        {
            if (this.type == "t1" || this.type == "t2")
            {
                bool flag = true;
                foreach (var item in MainGraph)
                {
                    if (MainGraph[item.Key].ContainsKey(Point1) && MainGraph[item.Key].ContainsKey(Point2) && Point1 != item.Key && Point2 != item.Key)
                    {
                        flag = false;
                        Console.WriteLine(item.Key);
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine("Такой точки нет");
                }
            }
            if (this.type == "t3" || this.type == "t4")
            {
                bool flag = true;
                foreach (var item in MainGraph)
                {
                    if (MainGraph[Point1].ContainsKey(item.Key) && MainGraph[Point2].ContainsKey(item.Key))
                    {
                        flag = false;
                        Console.WriteLine(item.Key);
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine("Такой точки нет");
                }
            }
        }

        ////////////////////////////// 3 Задача

        public void Dopolnevanie()
        {
            if (this.type == "t3")
            {
                foreach (var Point in MainGraph)
                {
                    if (MainGraph[Point.Key].ContainsKey(Point.Key))
                    {
                        this.DeleteWayFromPoints(Point.Key, Point.Key);
                    }
                    foreach (var Point2 in Point.Value)
                    {
                        if (!MainGraph[Point2.Key].ContainsKey(Point.Key))
                        {
                            this.AddWayFromPoints(Point2.Key, Point.Key);
                        }
                    }
                }
            }
            if (this.type == "t4")
            {
                foreach (var Point in MainGraph)
                {
                    foreach (var Point2 in Point.Value)
                    {
                        if (!MainGraph[Point2.Key].ContainsKey(Point.Key))
                        {
                            this.AddWayFromPoints(Point2.Key, Point.Key, this.MainGraph[Point.Key][Point2.Key]);
                        }
                    }
                }
            }
        }

        ////////////////////////////// 4 Задача

        public void Dostegimost_i_Obratno(string v)
        {
            if (this.type == "t3" || this.type == "t4")
            {
                Bfs(v);
                Dictionary<string, int> temp_nov = new Dictionary<string, int>();
                foreach (var key in this.nov)
                {
                    if (key.Value == 1)
                    {
                        temp_nov.Add(key.Key, key.Value);
                    }
                }
                ClearNov();
                foreach (string key in temp_nov.Keys)
                {
                    Bfs(key);
                    if (nov[v] == 1 && key != v)
                    {
                        Console.Write("{0} ", key);
                    }
                    ClearNov();
                }
            }
        }

        ////////////////////////////// 5 Задача

        public void MinCycles()
        {
            if (this.type == "t3" || this.type == "t4")
            {
                List<List<string>> AllCycles = new List<List<string>>();
                foreach (var Point in MainGraph)
                {
                    Dictionary<string, string> from = new Dictionary<string, string>();
                    foreach (string key in MainGraph.Keys)
                    {
                        from.Add(key, key);
                    }
                    Dfs(Point.Key, from, AllCycles);
                    ClearNov();
                }
                int min = int.MaxValue;
                foreach (List<string> ls in AllCycles)
                {
                    if (ls.Count - 1 <= min)
                    {
                        min = ls.Count - 1;
                    }
                }
                foreach (List<string> ls in AllCycles)
                {
                    if (ls.Count - 1 == min)
                    {
                        foreach (string key in ls)
                        {
                            Console.Write("{0} ", key);
                        }
                        Console.WriteLine();
                    }
                }
                ClearNov();
            }
        }

        ////////////////////////////// 6 задача

        public void Prim(string v)
        {
            this.Bfs(this.MainGraph.Keys.First());
            bool flag = true;
            foreach (var temp in this.nov)
            {
                if (temp.Value == 0)
                {
                    flag = false;
                    break;
                }
            }
            this.ClearNov();
            if (flag)
            {
                Graph Copy = new Graph();
                Copy.type = "t2";
                List<string> Selected = new List<string>();
                List<string> Unselected = new List<string>();
                foreach (string key in this.MainGraph.Keys)
                {
                    Copy.AddPoint(key);
                    Unselected.Add(key);
                }
                string p = v;
                Selected.Add(v);
                Unselected.Remove(v);
                while (Unselected.Count != 0)
                {
                    double minv = double.MaxValue;
                    string mink = "";
                    foreach (string key in Selected)
                    {
                        foreach (var Points in MainGraph[key])
                        {
                            if (Points.Value < minv && !Selected.Contains(Points.Key))
                            {
                                minv = Points.Value;
                                mink = Points.Key;
                                p = key;
                            }
                        }
                    }
                    Copy.AddWayFromPoints(p, mink, minv);
                    Selected.Add(mink);
                    Unselected.Remove(mink);
                }
                Copy.ShowMainGraph();
            }
            else
            {
                Console.WriteLine("Граф не связный");
            }
        }

        //////////////////////////////Фалкерсон-Форд 10 задача

        public void FordFulkersonAlgorithm(string x1,string x2)
        {
            ClearNov();
            SetSopostavitels();
            double[,] graph = CreateMatrix();
            int start = DeSopostavitel[x1];
            int end = DeSopostavitel[x2];
            int V = MainGraph.Keys.Count; // MainGraph поле класса хранящее список смежности ( public Dictionary<string, Dictionary<string, double>> MainGraph = new Dictionary<string, Dictionary<string, double>>(); )
            double[,] residualGraph = new double[V, V];
            Array.Copy(graph, residualGraph, graph.Length);

            int[] parent = new int[V];
            double maxFlow = 0;

            while (Bfs(residualGraph, start, end, parent))
            {
               double pathFlow = double.MaxValue;

                for (int v = end; v != start; v = parent[v])
                {
                    int u = parent[v];
                    pathFlow = Math.Min(pathFlow, residualGraph[u, v]);
                }

                for (int v = end; v != start; v = parent[v])
                {
                    int u = parent[v];
                    residualGraph[u, v] -= pathFlow;
                    residualGraph[v, u] += pathFlow;
                }

                maxFlow += pathFlow;
            }

            Console.WriteLine(maxFlow);
        }
    }

    public class Program
    {
        public static void Main()
        {
            Graph MyGraph = new Graph();
            Graph MyGraphCop = new Graph();
            string type = "";
            Console.WriteLine("////////////////////////////////////////////////////////");
        start:
            Console.WriteLine("Выберите тип графа");
            Console.WriteLine("Неориентированный невзвешенный граф: t1");
            Console.WriteLine("Неориентированный взвешенный граф: t2");
            Console.WriteLine("Ориентированный невзвешенный граф: t3");
            Console.WriteLine("Ориентированный взвешенный граф: t4");
            string key = Console.ReadLine();
            switch (key)
            {
                case "t1":
                    Console.WriteLine("Выход из программы: написать .");
                    Console.WriteLine("Выход из программы: написать help");
                    Console.WriteLine("Показать оригинальный граф: написать 0");
                    Console.WriteLine("Показать копию графа: написать 1");
                    Console.WriteLine("Опустошить граф: написать 2");
                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                    Console.WriteLine("Записать граф в файл: написать 4");
                    Console.WriteLine("Добавить вершину: написать A");
                    Console.WriteLine("Добавить ребро: написать AW");
                    Console.WriteLine("Удалить вершину: написать D");
                    Console.WriteLine("Удалить ребро: написать DW");
                    Console.WriteLine("Функция задания 2: написать N");
                    Console.WriteLine("Функция задания 7: написать AlgD");
                    Console.WriteLine("Функция задания 8: написать AlgF");
                    Console.WriteLine("Функция задания 9: написать AlgBF");
                    type = "t1";
                    MyGraph.type = type;
                    while (true)
                    {
                        string key1 = Console.ReadLine();
                        if (key1 == ".")
                        {
                            break;
                        }
                        else
                        {
                            switch (key1)
                            {
                                case "0":
                                    MyGraph.ShowMainGraph();
                                    break;
                                case "help":
                                    Console.WriteLine("Выход из программы: написать .");
                                    Console.WriteLine("Окно помощи: написать help");
                                    Console.WriteLine("Показать оригинальный граф: написать 0");
                                    Console.WriteLine("Показать копию графа: написать 1");
                                    Console.WriteLine("Опустошить граф: написать 2");
                                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                                    Console.WriteLine("Записать граф в файл: написать 4");
                                    Console.WriteLine("Добавить вершину: написать A");
                                    Console.WriteLine("Добавить ребро: написать AW");
                                    Console.WriteLine("Удалить вершину: написать D");
                                    Console.WriteLine("Удалить ребро: написать DW");
                                    Console.WriteLine("Функция задания 2: написать N");
                                    Console.WriteLine("Функция задания 7: написать AlgD");
                                    Console.WriteLine("Функция задания 8: написать AlgF");
                                    Console.WriteLine("Функция задания 9: написать AlgBF");
                                    break;
                                case "1":
                                    MyGraphCop.ShowMainGraph();
                                    break;
                                case "2":
                                    MyGraph = new Graph(type);
                                    break;
                                case "3":
                                    Console.WriteLine("Введите путь к файлу");
                                    string inway = Console.ReadLine();
                                    MyGraph = new Graph(inway, type);
                                    MyGraphCop = new Graph(MyGraph);
                                    break;
                                case "4":
                                    Console.WriteLine("Введите путь сохранения");
                                    string outway = Console.ReadLine();
                                    MyGraph.WrtiteToFile(outway);
                                    break;
                                case "A":
                                    Console.WriteLine("Введите добавляемую вершину");
                                    MyGraph.AddPoint(Console.ReadLine());
                                    break;
                                case "AW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointA = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointA = Console.ReadLine();
                                    MyGraph.AddWayFromPoints(FirstPointA, SecondPointA);
                                    break;
                                case "D":
                                    Console.WriteLine("Введите удаляемую вершину");
                                    MyGraph.DeletePoint(Console.ReadLine());
                                    break;
                                case "DW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointD = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointD = Console.ReadLine();
                                    MyGraph.DeleteWayFromPoints(FirstPointD, SecondPointD);
                                    break;
                                case "AlgD":
                                    Console.WriteLine("Введите первую вершину");
                                    string u = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v = Console.ReadLine();
                                    MyGraph.Dijkstr(u,v);
                                    break;
                                case "AlgF":
                                    MyGraph.Floyd();
                                    break;
                                case "AlgBF":
                                    Console.WriteLine("Введите первую вершину");
                                    string u1 = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v1 = Console.ReadLine();
                                    Console.WriteLine("Введите третью вершину");
                                    string v2 = Console.ReadLine();
                                    MyGraph.BellmanFord(u1,v1,v2);
                                    break;
                                case "N":
                                    Console.WriteLine("Введите вершины");
                                    string p1 = Console.ReadLine();
                                    string p2 = Console.ReadLine();
                                    MyGraph.Neighbours(p1, p2);
                                    break;
                                default:
                                    Console.WriteLine("Такой команды нет");
                                    break;
                            }
                        }
                        Console.WriteLine("////////////////////////////////////////////////////////");
                    }
                    break;
                case "t2":
                    Console.WriteLine("Выход из программы: написать .");
                    Console.WriteLine("Окно помощи: написать help");
                    Console.WriteLine("Показать оригинальный граф: написать 0");
                    Console.WriteLine("Показать копию графа: написать 1");
                    Console.WriteLine("Опустошить граф: написать 2");
                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                    Console.WriteLine("Записать граф в файл: написать 4");
                    Console.WriteLine("Добавить вершину: написать A");
                    Console.WriteLine("Добавить ребро: написать AW");
                    Console.WriteLine("Удалить вершину: написать D");
                    Console.WriteLine("Удалить ребро: написать DW");
                    Console.WriteLine("Функция задания 2: написать N");
                    Console.WriteLine("Функция задания 6: P");
                    Console.WriteLine("Функция задания 7: написать AlgD");
                    Console.WriteLine("Функция задания 8: написать AlgF");
                    Console.WriteLine("Функция задания 9: написать AlgBF");
                    type = "t2";
                    MyGraph.type = type;
                    while (true)
                    {
                        string key2 = Console.ReadLine();
                        if (key2 == ".")
                        {
                            break;
                        }
                        else
                        {
                            switch (key2)
                            {
                                case "0":
                                    MyGraph.ShowMainGraph();
                                    break;
                                case "help":
                                    Console.WriteLine("Выход из программы: написать .");
                                    Console.WriteLine("Выход из программы: написать help");
                                    Console.WriteLine("Показать оригинальный граф: написать 0");
                                    Console.WriteLine("Показать копию графа: написать 1");
                                    Console.WriteLine("Опустошить граф: написать 2");
                                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                                    Console.WriteLine("Записать граф в файл: написать 4");
                                    Console.WriteLine("Добавить вершину: написать A");
                                    Console.WriteLine("Добавить ребро: написать AW");
                                    Console.WriteLine("Удалить вершину: написать D");
                                    Console.WriteLine("Удалить ребро: написать DW");
                                    Console.WriteLine("Функция задания 2: написать N");
                                    Console.WriteLine("Функция задания 6: P");
                                    Console.WriteLine("Функция задания 7: написать AlgD");
                                    Console.WriteLine("Функция задания 8: написать AlgF");
                                    Console.WriteLine("Функция задания 9: написать AlgBF");
                                    break;
                                case "1":
                                    MyGraphCop.ShowMainGraph();
                                    break;
                                case "2":
                                    MyGraph = new Graph(type);
                                    break;
                                case "3":
                                    Console.WriteLine("Введите путь к файлу");
                                    string inway = Console.ReadLine();
                                    MyGraph = new Graph(inway, type);
                                    MyGraphCop = new Graph(MyGraph);
                                    break;
                                case "4":
                                    Console.WriteLine("Введите путь сохранения");
                                    string outway = Console.ReadLine();
                                    MyGraph.WrtiteToFile(outway);
                                    break;
                                case "A":
                                    Console.WriteLine("Введите добавляемую вершину");
                                    MyGraph.AddPoint(Console.ReadLine());
                                    break;
                                case "AW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointA = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointA = Console.ReadLine();
                                    Console.WriteLine("Введите длину пути");
                                    int Way = int.Parse(Console.ReadLine());
                                    MyGraph.AddWayFromPoints(FirstPointA, SecondPointA, Way);
                                    break;
                                case "D":
                                    Console.WriteLine("Введите удаляемую вершину");
                                    MyGraph.DeletePoint(Console.ReadLine());
                                    break;
                                case "DW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointD = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointD = Console.ReadLine();
                                    MyGraph.DeleteWayFromPoints(FirstPointD, SecondPointD);
                                    break;
                                case "P":
                                    MyGraph.Prim(MyGraph.MainGraph.Keys.First());
                                    break;
                                case "AlgD":
                                    Console.WriteLine("Введите первую вершину");
                                    string u = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v = Console.ReadLine();
                                    MyGraph.Dijkstr(u, v);
                                    break;
                                case "AlgF":
                                    MyGraph.Floyd();
                                    break;
                                case "AlgBF":
                                    Console.WriteLine("Введите первую вершину");
                                    string u1 = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v1 = Console.ReadLine();
                                    Console.WriteLine("Введите третью вершину");
                                    string v2 = Console.ReadLine();
                                    MyGraph.BellmanFord(u1, v1, v2);
                                    break;
                                case "N":
                                    Console.WriteLine("Введите вершины");
                                    string p1 = Console.ReadLine();
                                    string p2 = Console.ReadLine();
                                    MyGraph.Neighbours(p1, p2);
                                    break;
                                default:
                                    Console.WriteLine("Такой команды нет");
                                    break;
                            }
                        }
                        Console.WriteLine("////////////////////////////////////////////////////////");
                    }
                    break;
                case "t3":
                    Console.WriteLine("Выход из программы: написать .");
                    Console.WriteLine("Окно помощи: написать help");
                    Console.WriteLine("Показать оригинальный граф: написать 0");
                    Console.WriteLine("Показать копию графа: написать 1");
                    Console.WriteLine("Опустошить граф: написать 2");
                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                    Console.WriteLine("Записать граф в файл: написать 4");
                    Console.WriteLine("Добавить вершину: написать A");
                    Console.WriteLine("Добавить ребро: написать AW");
                    Console.WriteLine("Удалить вершину: написать D");
                    Console.WriteLine("Удалить ребро: написать DW");
                    Console.WriteLine("Функция задания 1: написать GPD");
                    Console.WriteLine("Функция задания 2: написать N");
                    Console.WriteLine("Функция задания 3: написать Dop");
                    Console.WriteLine("Функция задания 4: написать DiO");
                    Console.WriteLine("Функция задания 5: написать MC");
                    Console.WriteLine("Функция задания 7: написать AlgD");
                    Console.WriteLine("Функция задания 8: написать AlgF");
                    Console.WriteLine("Функция задания 9: написать AlgBF");
                    type = "t3";
                    MyGraph.type = type;
                    while (true)
                    {
                        string key3 = Console.ReadLine();
                        if (key3 == ".")
                        {
                            break;
                        }
                        else
                        {
                            switch (key3)
                            {
                                case "0":
                                    MyGraph.ShowMainGraph();
                                    break;
                                case "help":
                                    Console.WriteLine("Выход из программы: написать .");
                                    Console.WriteLine("Выход из программы: написать help");
                                    Console.WriteLine("Показать оригинальный граф: написать 0");
                                    Console.WriteLine("Показать копию графа: написать 1");
                                    Console.WriteLine("Опустошить граф: написать 2");
                                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                                    Console.WriteLine("Записать граф в файл: написать 4");
                                    Console.WriteLine("Добавить вершину: написать A");
                                    Console.WriteLine("Добавить ребро: написать AW");
                                    Console.WriteLine("Удалить вершину: написать D");
                                    Console.WriteLine("Удалить ребро: написать DW");
                                    Console.WriteLine("Функция задания 1: написать GPD");
                                    Console.WriteLine("Функция задания 2: написать N");
                                    Console.WriteLine("Функция задания 3: написать Dop");
                                    Console.WriteLine("Функция задания 4: написать DiO");
                                    Console.WriteLine("Функция задания 5: написать MC");
                                    Console.WriteLine("Функция задания 7: написать AlgD");
                                    Console.WriteLine("Функция задания 8: написать AlgF");
                                    Console.WriteLine("Функция задания 9: написать AlgBF");
                                    break;
                                case "1":
                                    MyGraphCop.ShowMainGraph();
                                    break;
                                case "2":
                                    MyGraph = new Graph(type);
                                    break;
                                case "3":
                                    Console.WriteLine("Введите путь к файлу");
                                    string inway = Console.ReadLine();
                                    MyGraph = new Graph(inway, type);
                                    MyGraphCop = new Graph(MyGraph);
                                    break;
                                case "4":
                                    Console.WriteLine("Введите путь сохранения");
                                    string outway = Console.ReadLine();
                                    MyGraph.WrtiteToFile(outway);
                                    break;
                                case "A":
                                    Console.WriteLine("Введите добавляемую вершину");
                                    MyGraph.AddPoint(Console.ReadLine());
                                    break;
                                case "AW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointA = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointA = Console.ReadLine();
                                    MyGraph.AddWayFromPoints(FirstPointA, SecondPointA);
                                    break;
                                case "D":
                                    Console.WriteLine("Введите удаляемую вершину");
                                    MyGraph.DeletePoint(Console.ReadLine());
                                    break;
                                case "DW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointD = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointD = Console.ReadLine();
                                    MyGraph.DeleteWayFromPoints(FirstPointD, SecondPointD);
                                    break;
                                case "GPD":
                                    Console.WriteLine("Введите вершину");
                                    MyGraph.Get_Points_Degree();
                                    break;
                                case "N":
                                    Console.WriteLine("Введите вершины");
                                    string p1 = Console.ReadLine();
                                    string p2 = Console.ReadLine();
                                    MyGraph.Neighbours(p1, p2);
                                    break;
                                case "Dop":
                                    MyGraphCop = new Graph(MyGraph);
                                    MyGraphCop.Dopolnevanie();
                                    MyGraphCop.ShowMainGraph();
                                    break;
                                case "DiO":
                                    Console.WriteLine("Введите вершину");
                                    MyGraph.Dostegimost_i_Obratno(Console.ReadLine());
                                    break;
                                case "MC":
                                    MyGraph.MinCycles();
                                    break;
                                case "AlgD":
                                    Console.WriteLine("Введите первую вершину");
                                    string u = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v = Console.ReadLine();
                                    MyGraph.Dijkstr(u, v);
                                    break;
                                case "AlgF":
                                    MyGraph.Floyd();
                                    break;
                                case "AlgBF":
                                    Console.WriteLine("Введите первую вершину");
                                    string u1 = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v1 = Console.ReadLine();
                                    Console.WriteLine("Введите третью вершину");
                                    string v2 = Console.ReadLine();
                                    MyGraph.BellmanFord(u1, v1, v2);
                                    break;
                                default:
                                    Console.WriteLine("Такой команды нет");
                                    break;
                            }
                        }
                        Console.WriteLine("////////////////////////////////////////////////////////");
                    }
                    break;
                case "t4":
                    Console.WriteLine("Выход из программы: написать .");
                    Console.WriteLine("Окно помощи: написать help");
                    Console.WriteLine("Показать оригинальный граф: написать 0");
                    Console.WriteLine("Показать копию графа: написать 1");
                    Console.WriteLine("Опустошить граф: написать 2");
                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                    Console.WriteLine("Записать граф в файл: написать 4");
                    Console.WriteLine("Добавить вершину: написать A");
                    Console.WriteLine("Добавить ребро: написать AW");
                    Console.WriteLine("Удалить вершину: написать D");
                    Console.WriteLine("Удалить ребро: написать DW");
                    Console.WriteLine("Функция задания 1: написать GPD");
                    Console.WriteLine("Функция задания 2: написать N");
                    Console.WriteLine("Функция задания 3: написать Dop");
                    Console.WriteLine("Функция задания 4: написать DiO");
                    Console.WriteLine("Функция задания 5: написать MC");
                    Console.WriteLine("Функция задания 7: написать AlgD");
                    Console.WriteLine("Функция задания 8: написать AlgF");
                    Console.WriteLine("Функция задания 9: написать AlgBF");
                    Console.WriteLine("Функция задания 10: написать AlgFF");
                    type = "t4";
                    MyGraph.type = type;
                    while (true)
                    {
                        string key4 = Console.ReadLine();
                        if (key4 == ".")
                        {
                            break;
                        }
                        else
                        {
                            switch (key4)
                            {
                                case "0":
                                    MyGraph.ShowMainGraph();
                                    break;
                                case "help":
                                    Console.WriteLine("Выход из программы: написать .");
                                    Console.WriteLine("Выход из программы: написать help");
                                    Console.WriteLine("Показать оригинальный граф: написать 0");
                                    Console.WriteLine("Показать копию графа: написать 1");
                                    Console.WriteLine("Опустошить граф: написать 2");
                                    Console.WriteLine("Заполнить граф по файлу: написать 3");
                                    Console.WriteLine("Записать граф в файл: написать 4");
                                    Console.WriteLine("Добавить вершину: написать A");
                                    Console.WriteLine("Добавить ребро: написать AW");
                                    Console.WriteLine("Удалить вершину: написать D");
                                    Console.WriteLine("Удалить ребро: написать DW");
                                    Console.WriteLine("Функция задания 1: написать GPD");
                                    Console.WriteLine("Функция задания 2: написать N");
                                    Console.WriteLine("Функция задания 3: написать Dop");
                                    Console.WriteLine("Функция задания 4: написать DiO");
                                    Console.WriteLine("Функция задания 5: написать MC");
                                    Console.WriteLine("Функция задания 7: написать AlgD");
                                    Console.WriteLine("Функция задания 8: написать AlgF");
                                    Console.WriteLine("Функция задания 9: написать AlgBF");
                                    Console.WriteLine("Функция задания 10: написать AlgFF");
                                    break;
                                case "1":
                                    MyGraphCop.ShowMainGraph();
                                    break;
                                case "2":
                                    MyGraph = new Graph(type);
                                    break;
                                case "3":
                                    Console.WriteLine("Введите путь к файлу");
                                    string inway = Console.ReadLine();
                                    MyGraph = new Graph(inway, type);
                                    MyGraphCop = new Graph(MyGraph);
                                    break;
                                case "4":
                                    Console.WriteLine("Введите путь сохранения");
                                    string outway = Console.ReadLine();
                                    MyGraph.WrtiteToFile(outway);
                                    break;
                                case "A":
                                    Console.WriteLine("Введите добавляемую вершину");
                                    MyGraph.AddPoint(Console.ReadLine());
                                    break;
                                case "AW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointA = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointA = Console.ReadLine();
                                    Console.WriteLine("Введите длину пути");
                                    int Way = int.Parse(Console.ReadLine());
                                    MyGraph.AddWayFromPoints(FirstPointA, SecondPointA, Way);
                                    break;
                                case "D":
                                    Console.WriteLine("Введите удаляемую вершину");
                                    MyGraph.DeletePoint(Console.ReadLine());
                                    break;
                                case "DW":
                                    Console.WriteLine("Введите первую вершину");
                                    string FirstPointD = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string SecondPointD = Console.ReadLine();
                                    MyGraph.DeleteWayFromPoints(FirstPointD, SecondPointD);
                                    break;
                                case "GPD":
                                    Console.WriteLine("Введите вершину");
                                    MyGraph.Get_Points_Degree();
                                    break;
                                case "N":
                                    Console.WriteLine("Введите вершины");
                                    string p1 = Console.ReadLine();
                                    string p2 = Console.ReadLine();
                                    MyGraph.Neighbours(p1, p2);
                                    break;
                                case "DiO":
                                    Console.WriteLine("Введите вершину");
                                    MyGraph.Dostegimost_i_Obratno(Console.ReadLine());
                                    break;
                                case "Dop":
                                    MyGraphCop = new Graph(MyGraph);
                                    MyGraphCop.Dopolnevanie();
                                    break;
                                case "MC":
                                    MyGraph.MinCycles();
                                    break;
                                case "AlgD":
                                    Console.WriteLine("Введите первую вершину");
                                    string u = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v = Console.ReadLine();
                                    MyGraph.Dijkstr(u, v);
                                    break;
                                case "AlgF":
                                    MyGraph.Floyd();
                                    break;
                                case "AlgBF":
                                    Console.WriteLine("Введите первую вершину");
                                    string u1 = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string v1 = Console.ReadLine();
                                    Console.WriteLine("Введите третью вершину");
                                    string v2 = Console.ReadLine();
                                    MyGraph.BellmanFord(u1, v1, v2);
                                    break;
                                case "AlgFF":
                                    Console.WriteLine("Введите первую вершину");
                                    string start = Console.ReadLine();
                                    Console.WriteLine("Введите вторую вершину");
                                    string end = Console.ReadLine();
                                    MyGraph.FordFulkersonAlgorithm(start,end);
                                    break;
                                default:
                                    Console.WriteLine("Такой команды нет");
                                    break;
                            }
                        }
                        Console.WriteLine("////////////////////////////////////////////////////////");
                    }
                    break;
                default:
                    Console.WriteLine("Такой команды нет");
                    goto start;
            }
        }
    }
}