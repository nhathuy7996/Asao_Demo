using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GridManager : Singleton<GridManager>
{
    [Header("Example")]
     [SerializeField] public TileBase target = null, start = null; 

    [Header("Require")]
    [SerializeField] TileBase _tile;
    [SerializeField] int _width, _heigh;

    Dictionary<Vector2, TileBase> _tiles = new Dictionary<Vector2, TileBase>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }
    public List<TileBase> path = new List<TileBase>();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if (target == null || start == null)
            return;

       
        if (Input.GetKey(KeyCode.Space))
           path = pathFinding(start, target);

        for (int i = 0; i< path.Count; i++)
        {
            path[i].GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < _width; x ++)
        {
            for (int y = 0; y< _heigh; y++)
            {
                TileBase t = Instantiate<TileBase>(_tile, new Vector2(x, y), Quaternion.identity,this.transform);

                _tiles.TryAdd<Vector2, TileBase>(new Vector2(x, y), t);

                t.gameObject.name = string.Format("tile {0} - {1}", x, y);

                t.isWalkable = Random.Range(0, 100) < 30 ? false : true;

                if (!t.isWalkable)
                    t.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }

        Camera.main.transform.position = new Vector3(_width/2, _heigh/2, -10);
    }

    public TileBase GetTileAtPos(Vector2 pos)
    {
        TileBase t = null;

        if(! _tiles.TryGetValue(pos, out t))
        {
            Debug.LogError("tile at pos "+ pos +" not exist!");
        }

        return t;
    }

    public List<TileBase> toSearch = new List<TileBase>();
    public List<TileBase> processed = new List<TileBase>();

    public List<TileBase> pathFinding(TileBase start, TileBase target)
    {
        toSearch.Clear();
        processed.Clear();

        toSearch.Add(start);

        while (toSearch.Count != 0)
        {
            TileBase currentSearch = toSearch[0];

            foreach (var t in toSearch)
            {
                if (t.F < currentSearch.F || (t.F == currentSearch.F && t.H < currentSearch.H))
                {
                    currentSearch = t;
                }
            }

            processed.Add(currentSearch);
            toSearch.Remove(currentSearch);

            if (currentSearch == target)
            {
                TileBase current = target;
                List<TileBase> path = new List<TileBase>();

                while (current != start)
                {
                    path.Add(current.connection);
                    current = current.connection;
                }

                return path;
            }

            foreach (var pos in currentSearch.neighbor)
            {
                TileBase t_neighbor = GridManager.Instant.GetTileAtPos(pos);

                if (!t_neighbor)
                    continue;
                if (!t_neighbor.isWalkable)
                    continue;

                if (processed.Contains(t_neighbor))
                    continue;

                bool isInSearch = toSearch.Contains(t_neighbor);

                int costG = currentSearch.G + currentSearch.calDistance(t_neighbor);

                if (!isInSearch || costG < t_neighbor.G || t_neighbor.G < 0)
                {
                    t_neighbor.G = costG;
                    t_neighbor.connection = currentSearch;

                    if (!isInSearch)
                    {
                        t_neighbor.H = t_neighbor.calDistance(target);
                        toSearch.Add(t_neighbor);
                    }

                }
            }


        }

        return null;
    }
}
