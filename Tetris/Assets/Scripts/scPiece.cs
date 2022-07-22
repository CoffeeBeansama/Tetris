using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scPiece : MonoBehaviour
{
    public scBoard board { get; private set;}
    public Tetronimo_Data data { get; private set;}
    public Vector3Int position {get; private set;}
   
    private scScreenShake shake;
    public AudioSource Rotate_SFX;
    public AudioSource HardDrop_SFX;
    public AudioSource SoftDrop_SFX;
    public AudioSource Move_SFX;
    public Vector3Int[] cells { get; private set;}
    public int rotationIndex {get; private set;}
    public float stepDelay = 1f;
    public float lockDelay = 0.5f;
    private float stepTime;
    private float lockTime;
        public void Initialize(scBoard board,Vector3Int position, Tetronimo_Data data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;     ///Time.time is the curent time in game
                                                        ///Time.delta time is 1 frame delayed from Time.time
        this.lockTime = 0f;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }
        
        for(int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<scScreenShake>();
    }

    private void Update()
    {
        this.board.Clear(this); // references scBoards's "Clear function"
        this.lockTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
            Rotate_SFX.Play();
        }else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate_SFX.Play();
            Rotate(1);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            Move_SFX.Play();
            Move(Vector2Int.left);
        }else if(Input.GetKeyDown(KeyCode.D))
        {
            Move_SFX.Play();
            Move(Vector2Int.right);  
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Move_SFX.Play();
            Move(Vector2Int.down);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            shake.CamShake(); /// Calls the shake function
            HardDrop_SFX.Play();
            HardDrop();
        }

        if(Time.time >= this.stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);
        if(lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        SoftDrop_SFX.Play();
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();

    }


    private void HardDrop()
    {
        while(Move(Vector2Int.down)) // continous the block to go down until it cant move down
        {
           
            continue;
        }
        
        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition); // Referencing scBoard's IsValidPosition()

        if (valid)
        {
            
            this.position = newPosition;
            this.lockTime = 0f;

        }
        return valid;
    }

    private void Rotate(int direction) //for rotation
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0 , 4); // using 0, 1, 2, 3 for index rotation

        ApplyRotationMatrix(direction);

        if(!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
          for(int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];
            int x , y;

            switch(this.data.tetronimo)
            {
                case scTetronimo.I:
                case scTetronimo.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                     x = Mathf.CeilToInt((cell.x * scData.RotationMatrix[0] * direction) + (cell.y * scData.RotationMatrix[1] * direction));
                     y = Mathf.CeilToInt((cell.x * scData.RotationMatrix[2] * direction) + (cell.y * scData.RotationMatrix[3] * direction));
                    
                    break;

                default:
                     x = Mathf.RoundToInt((cell.x * scData.RotationMatrix[0] * direction) + (cell.y * scData.RotationMatrix[1] * direction));
                     y = Mathf.RoundToInt((cell.x * scData.RotationMatrix[2] * direction) + (cell.y * scData.RotationMatrix[3] * direction));

                     break;
            }
            this.cells[i] = new Vector3Int(x,y,0);
        }

    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKicksIndex = GetWallKicksIndex(rotationIndex, rotationDirection);

        for(int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKicksIndex, i];
            if(Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    public int GetWallKicksIndex(int rotationIndex, int rotationDirection)
    {
        int wallKicksIndex = rotationIndex * 2;
        if(rotationIndex < 0)
        {
            wallKicksIndex--;
        }

        return Wrap(wallKicksIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max) 
    {
        if(input < min)
        {
            return max -(min - input) % (max - min); // if index = 0 and it needs to go to index 3
        }else           
        {
            return min +(input - min) % (max - min);// if index > 3 it wraps back the index to 0 to avoid going out of bounds else vice versa
        }
    }

}

