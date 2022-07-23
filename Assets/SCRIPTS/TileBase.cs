using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [SerializeField]
    public TileBase connection = null;

    public int G = -1;
    public int H = 0;
    public int F => G + H;

    public int posX => (int)this.transform.position.x;
    public int posY => (int)this.transform.position.y;

    public List<Vector2> neighbor = new List<Vector2>();

    public bool isWalkable = true;

    // Start is called before the first frame update
    void Start()
    {
        neighbor.Add(new Vector2(this.posX, this.posY + 1));
        neighbor.Add(new Vector2(this.posX + 1, this.posY + 1));

        neighbor.Add(new Vector2(this.posX + 1, this.posY));
        neighbor.Add(new Vector2(this.posX + 1, this.posY - 1));

        neighbor.Add(new Vector2(this.posX, this.posY - 1));
        neighbor.Add(new Vector2(this.posX -1, this.posY - 1));

        neighbor.Add(new Vector2(this.posX -1, this.posY));
        neighbor.Add(new Vector2(this.posX -1, this.posY + 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int calDistance(TileBase t)
    {
        int X = Mathf.Abs(t.posX - this.posX);
        int Y = Mathf.Abs(t.posY - this.posY);

        int remain = Mathf.Abs(X - Y);

        return remain * 10 + Mathf.Min(X, Y) * 14;
    }

    private void OnMouseDown()
    {
        if (!GridManager.Instant.start)
        {
            GridManager.Instant.start = this;
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }

        else
        {
            GridManager.Instant.target = this;
            this.GetComponent<SpriteRenderer>().color = Color.green;
        }
           

    }
}
