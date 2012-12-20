using UnityEngine;
using System.Collections;
using System.IO;

public class Maze : MonoBehaviour {
	
	// n rows m columns
	public static int n = 10;
	public static int m = 10;
	
	// Walls state (true : visible, false invisible)
	static bool[,] ns_walls = new bool[n,m-1];
	static bool[,] we_walls = new bool[n-1,m];
	
	// Messages displayed on the wall
	static string[,,] ns_walls_msg = new string[n,m-1,2];
	static string[,,] we_walls_msg = new string[n-1,m,2];
	
	//files containig messages
	public TextAsset truthsFile;
	public TextAsset liesFile;
	private static string[] truths;
	private static string[] lies;

	// Use this for initialization
	void Start () {
		
		truths = truthsFile.text.Split("\n"[0]);
		lies = liesFile.text.Split("\n"[0]);
		CreatePerfectMaze();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Get the wall state (true : visible, false invisible)
	public static bool GetState(string orientation, int row, int col){
		if(orientation == "NS"){
			return ns_walls[row,col];
		}else{
			return we_walls[row,col];
		}
	}
	
	// Get the message for the given wall coordinates (face is either 0 or 1, because each wall has two faces)
	public static string GetMessage(string orientation, int row, int col, int face){
		if(orientation == "NS"){
			return ns_walls_msg[row,col,face];
		}else{
			return we_walls_msg[row,col,face];
		}
	}
	
	// Get the message for the given wall coordinate (face is either 0 or 1, because each wall has two faces)
	public static void SetMessage(string orientation, int row, int col, int face, string message = ""){
		if(orientation == "NS"){
			ns_walls_msg[row,col,face] = message;
		}else{
			we_walls_msg[row,col,face] = message;
		}
	}
	
	// Set the walls state to be a perfect maze
	public static void CreatePerfectMaze(){
		
		// Initialize the walls (close them all)
		for(int i = 0; i < n; i++)
			for(int j = 0; j < m-1; j++)
				ns_walls[i,j] = true;
		
		for(int i = 0; i < n-1; i++)
			for(int j = 0; j < m; j++)
				we_walls[i,j] = true;
		
		// Numbered the cells
		int[,] cells = new int[n,m];
		int cpt = 0;
		for(int i = 0; i < n; i++)
			for(int j = 0; j < m; j++)
				cells[i,j] = cpt++;
		
		// Main loop
		int n_open_walls = 0;
		while(n_open_walls < n * m - 1){
	
			
			// Choose a random closed wall
			int rand_orientation = Random.Range(0,2);
			bool[,] walls;
			int rand_row;
			int rand_col;
			if(rand_orientation == 0){//orientation 0 : NS, 1: WE
				walls = ns_walls;
				rand_row = Random.Range(0,n);
				rand_col = Random.Range(0,m-1);
			}else{
				walls = we_walls;
				rand_row = Random.Range(0,n-1);
				rand_col = Random.Range(0,m);
			}
			// if the wall is already opened (false), 
			// then skip this iteration
			if(!walls[rand_row,rand_col])
				continue;
			
			// Check if the two adjacent cells have different values
			// otherwise, skip the loop
			
			int cell_value;
			int cell_neighbour;
			if(rand_orientation == 0){
				cell_value = cells[rand_row,rand_col];
				cell_neighbour = cells[rand_row,rand_col + 1];
				if(cell_value == cell_neighbour)
					continue;				
			}else{
				cell_value = cells[rand_row,rand_col];
				cell_neighbour = cells[rand_row + 1,rand_col];
				if(cell_value == cell_neighbour)
					continue;
			}
			
			// Replace the value of all the cells from the first path
			// with the value of the cells from the second path
			for(int i = 0; i < n; i++)
					for(int j = 0; j < m; j++)
						if(cells[i,j] == cell_value)
							cells[i,j] = cell_neighbour;
			
			// Open the wall
			walls[rand_row, rand_col] = false;
			n_open_walls++;		
		}
		
	}
	
	// Attach a given message to a given wall
	public static void AddMessage(string message, string orientation, int row, int col, int face){
		if(orientation == "NS")
			ns_walls_msg[row,col,face] = message;
		else
			we_walls_msg[row,col,face] = message;
	}
	
	// Attach random messages to the walls
	// It may be that messages are assigned to invisible wall
	// Truths are displayed on walls that will not disappear.
	public static void AddRandomMessages(int n_truths = 1, int n_lies = 1){
		// Clear all messages
		for(int i = 0; i < n; i++)
			for(int j = 0; j < m -1; j++)
				for(int k = 0; k < 2; k++)
					ns_walls_msg[i,j,k] = "";
		
		for(int i = 0; i < n - 1; i++)
			for(int j = 0; j < m; j++)
				for(int k = 0; k < 2; k++)
					we_walls_msg[i,j,k] = "";
		
		// Assign random truths to random walls
		for (int i = 0; i < n_truths; i++){
			int[] rand_wall_coord = GetRandWallCoord(true);
			int rand_orientation = rand_wall_coord[0];
			int rand_row = rand_wall_coord[1];
			int rand_col = rand_wall_coord[2];
			int rand_face = Random.Range(0,2);
			if(rand_orientation == 0 ) //NS
				ns_walls_msg[rand_row,rand_col,rand_face] = truths[Random.Range(0,truths.Length)];
			else //WE
				we_walls_msg[rand_row,rand_col,rand_face] = truths[Random.Range(0,truths.Length)];
		}
		
		// Assign random lies to random walls
		for (int i = 0; i < n_lies; i++){
			int[] rand_wall_coord = GetRandWallCoord(false);
			int rand_orientation = rand_wall_coord[0];
			int rand_row = rand_wall_coord[1];
			int rand_col = rand_wall_coord[2];
			int rand_face = Random.Range(0,2);
			if(rand_orientation == 0 ) //NS
				ns_walls_msg[rand_row,rand_col,rand_face] = lies[Random.Range(0,lies.Length)];
			else //WE
				we_walls_msg[rand_row,rand_col,rand_face] = lies[Random.Range(0,lies.Length)];
		}
		
	}
	
	// Get the coordinates of a random wall [orientation,row,col] with orientation 0 : NS, 1 : WE
	// It is possible to choose a given wall state
	static int[] GetRandWallCoord(bool? wall_state = null){
		int rand_orientation;
		bool[,] walls;
		int rand_row;
		int rand_col;
		while(true){
			rand_orientation = Random.Range(0,2);
			if(rand_orientation == 0){//orientation 0 : NS, 1: WE
				walls = ns_walls;
				rand_row = Random.Range(0,n);
				rand_col = Random.Range(0,m-1);
			}else{
				walls = we_walls;
				rand_row = Random.Range(0,n-1);
				rand_col = Random.Range(0,m);
			}
			// ckeck if the wall has the asked state
			if(wall_state == null || walls[rand_row,rand_col] == wall_state)
					break;
		}
		return new int[] {rand_orientation,rand_row,rand_col};
	}
}
