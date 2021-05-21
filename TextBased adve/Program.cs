using System;
using System.Data;
using System.Linq;

namespace GameInstance
{
	class MainClass
	{
		public static void Main()
		{
		//Starts off the game by calling the animation class with an annimation
		Animations.StartScreen();
		}
	}
	public class Decisions
	{
		//declared userInput to check what the user has inputted
		public static String userInput; 
		//declared String array to keep reference to which instance the game was in
		public static String[] instanceRef = { "Solar System", "Notifications", "Drone Config", "Ship Display", "Galaxy Map" };
		//Declared String array to reference Ships by an alphabetical ID
		public static char[] shipID = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
		//Declared Int array to give an id to each room to reference later
		public static int[] roomNum = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		//declared int array that could have been a bool array but checks if a room exists. 0 = false; 1 = True
		public static int[] roomExists = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		//declared char to store ShipID and to be used to be inserted to the Ship creation method
		public static char shipInput;
		//Declared int to keep track of the current instance to accurately 
		public static int currentInstance;
		//temp int to store cursor position to set back later
		public static int tempCursorX;
		public static int tempCursorY;

		//When decisions is first called set Current Instance to 0
		public Decisions()
		{
			currentInstance = 0;
		}

		//method that takes the input from the console and gives the respective output based on which screen the player is currently in.
		public static void Checker()
		{
			//set the color of the text to cyan
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Issue a Command:"); //Switching commenting method to make stuff easier. Wrote line to indicate playe
			userInput = Console.ReadLine(); //set user input to userInput
			if (Decisions.currentInstance == 0) //if the current instance is 0 then the following can occur:
			{
				if (userInput == "D" || userInput == "d")// Entering D will send the player to the Drone Menu
				{
					DroneMenu.ShowDrone(); //Calls the show drone method
				}
				else if (userInput == "N" || userInput == "n") //Entering N will send the Player to the Notification menu
				{
					Notifications.ShowNotifications(); //calls the Show notifications method
				}
				else if (userInput == "G" || userInput == "g") //Entering G will cause an error sending the player to the Galaxy as it doesn't exist
				{
					Console.WriteLine("Galaxy Map Incomplete. Cannot Procede"); //writing to console
					Decisions.Checker(); //calling the checker again to allow the user to input another command
				}
				else if (userInput[0] == 'T' || userInput[0] == 't') //Entering T will start the Travel process
				{
					userInput = userInput + "  "; //buffer added to userInput to lessen breaks in code
					if (userInput[2] == 'A' || userInput[2] == 'B' || userInput[2] == 'C' || userInput[2] == 'D' || userInput[2] == 'E' || userInput[2] == 'F' || userInput[2] == 'G' || userInput[2] == 'H') { //Traveling is called by "T [ShipID]". If checks if a valid ID was entered.
						for (int i = 0; i < shipID.Length; i++)
						{
							if (shipID[i] == userInput[2]) //compares ShipID to the character entered and confirms it in the array 
							{
								shipInput = shipID[i]; //ship Input is made equal to the confirmed array value to ease issues from using UserInput[2] or shipId[at whatever]
							}
						}
						if (Galaxy.CheckStatus(shipInput) == true) //checks the status of the ship to make sure one has not been there before. If it has, travel to the ship is denied
                        {
							Console.WriteLine("Invalid Request: Have Already Visited");
							Decisions.Checker(); //calls the checker method again
						} else
                        {
							Decisions.Travel(); //all passes then the Travel method is called
						}
						
					}
					else // if the second part of the command is invalid. An error message is displayed
                    {
						Console.WriteLine("Invalid Request");
						Decisions.Checker(); //calls the checker method again

					}
						
				}
				else if (userInput == "Ship Info" || userInput == "ship info" || userInput == "ship Info" || userInput == "Ship info") //Ship Info displays the shipID, Type, and Status
				{
					Console.WriteLine("ShipID : Ship Type : Additional Info");
					for (int i = 0; i < Galaxy.ShipNum(); i++) //Using a for loop to create a list of the available ships and their status
                    {
						Console.WriteLine(Galaxy.ShipID(i) + ": " + Galaxy.Ships(i) + " - " + Galaxy.ShipStatus(i));
                    }
					Decisions.Checker();//Calling Checker Method as it only displays text in console rather than a new screen

				}
				else //unicheck is called for repeating checks (help and exit commands)
                {
					UniCheck();
                }
				
			}
			else if (Decisions.currentInstance == 1) //Decision Tree for notifications
            {
				if (userInput == "D" || userInput == "d") //Goes to Drone Menu
				{
					DroneMenu.ShowDrone();
				}
				else if (userInput == "N" || userInput == "n") //Notifies Player that they're already in Notifications
				{
					Console.WriteLine("Already in Notifications");
				}
				else if (userInput == "G" || userInput == "g") //Denies Player access to Galaxy Map
				{
					Console.WriteLine("Galaxy Map Incomplete. Cannot Procede");
					Decisions.Checker();
				}
				else if (userInput == "S" || userInput == "s")//Goes to Solar System
				{
					Galaxy.ShowSystem();
				}
				else //Universal Check
                {
					UniCheck();
                }
			}
			else if (Decisions.currentInstance == 2) //Decision Tree for Drones
			{
				if (userInput == "D" || userInput == "d") //Notifies Player they're already in Drone menu
				{
					Console.WriteLine("Already in Drone Configurations");
					Decisions.Checker();
				}
				else if (userInput == "N" || userInput == "n") //Goes to Notifications
				{
					Notifications.ShowNotifications();
				}
				else if (userInput == "G" || userInput == "g") //Denies Acces to Galaxy Map
				{
					Console.WriteLine("Galaxy Map Incomplete. Cannot Procede");
					Decisions.Checker();
				}
				else if (userInput == "S" || userInput == "s")// Goes to Solar System
				{
					Galaxy.ShowSystem();
				}
				else //Univeral Check
				{
					UniCheck();
				}
			}
			else if (Decisions.currentInstance == 3) //Decision Tree for Ships
			{
				
				if (LongCheck(userInput)) //following 3 if statements deny acess during Ship inspection
				{
					Console.WriteLine("Moving Drone");
					Decisions.Checker();

				}
				else if (userInput == "N" || userInput == "n")
				{
					Console.WriteLine("Cannot Access");
					Decisions.Checker();
				}
				else if (userInput == "G" || userInput == "g")
				{
					Console.WriteLine("Cannot Access");
					Decisions.Checker();
				}
				else if (userInput == "S" || userInput == "s") //goes back to solar system and makes sure that the user is sure of their actions
				{
					Console.WriteLine("Are You Sure? You Won't Be Able To Come Back. Y/N");
					userInput = Console.ReadLine();
					if (userInput == "Y" || userInput == "y") //if Y then back to solar system
					{
						Galaxy.ShowSystem();
					}
					else if (userInput == "N" || userInput == "n") //if N then stay on current ship and calls the checker again
					{
						Decisions.Checker();
					}
					else
					{
						Console.WriteLine("Command Not Recognized"); //error recognition
						Decisions.Checker();
					}
					
				}
				else if (userInput == "Print" || userInput == "print") //debugging command that prints active rooms in current ship
                {
					int[] tempIndex = new int[12];
					int tempCount = 0;
					for (int i = 0; i < Ships.ReturnArrayLength();i++)
                    {
						Console.Write(Ships.ReturnArray(i) + " ");
                    }
					Console.WriteLine("");
					for (int i = 0; i < 11; i++) //checks available rooms and availability and inputs into an array
					{
						if (Ships.ReturnArray(i) == 1)
						{
							tempIndex[tempCount] = i;
							tempCount++;
						}
					}
					roomNum = new int[tempCount + 1];
					for (int i = 0; i < roomNum.Length; i++)
					{
						roomNum[i] = tempIndex[i];
					}
					for (int i = 0; i < roomNum.Length; i++)
					{
						Console.Write(roomNum[i] + " ");
					}
					Console.WriteLine("");
					Decisions.Checker();

				}
				else //universal check
				{
					UniCheck();
				}
			}
		}
		public static int Instance() //accessor method for current instance
		{
			return currentInstance;
		}

		public static void ChangeInstance(int instance) //mutator method for current instance
		{
			currentInstance = instance;
		}

		public static void Help() //method that displays help information
        {
			Console.WriteLine("Usable Commands:");
			Console.WriteLine("\"D\" : Goes to Drone Config");
			Console.WriteLine("\"S\" : Goes to Solar System Map");
			Console.WriteLine("\"N\" : Checks Notifications");
			Console.WriteLine("\"G\" : Goes to Galaxy Map");
			Console.WriteLine("\"Ship Info\" : Shows Information about Ships");
			Console.WriteLine("\"T\" [Ship Number] : Travels to Assigned Ship");
			Console.WriteLine("\"Exit\" : Exits the Program");
			Decisions.Checker();
		}
		public static void RecognitionError() //method that displays an error and calls the Checker method again
        {
			Console.WriteLine("Command Not Recognized");
			Decisions.Checker();
		}
		public static void UniCheck() //method that repeats useful user inputs to cut back on lines
        {
			if (userInput == "H" || userInput == "Help" || userInput == "help" || userInput == "h") //if any of the statements are met calls help method
			{
				Decisions.Help();
			}
			else if (userInput == "exit" || userInput == "Exit") //calls the exit method if exit is typed
			{
				Decisions.Exit();
			}
			else
			{
				Decisions.RecognitionError(); //calls the recognition error method
			}
		}
		public static void Exit() //method that asks the player if they are sure about exiting
        {
			Console.WriteLine("Are You Sure? Y/N");
			userInput = Console.ReadLine();
			if (userInput == "Y" || userInput == "y")//if Y then exit the program
			{
				System.Environment.Exit(0);
			}
			else if (userInput == "N" || userInput == "n")//if N then stay on current ship and calls the checker again
			{
				Decisions.Checker();
			}
			else
			{
				Console.WriteLine("Command Not Recognized"); //error recognition
				Decisions.Checker();
			}
		}
		public static void Travel() //confirming travel method
        {
			
			Console.WriteLine("Confirm Travel to ShipID: " + userInput[2] + ". Y/N"); //displays shipID of target ship
			userInput = Console.ReadLine();
			if (userInput == "Y" || userInput == "y") //if Y:
            {
				Console.WriteLine("Confirming Flight Plan..."); //displays text
				Console.Clear(); //clears the console
				Ships.ResetRoomNum(); //clears the ship room numbers to confirm prior ship usage doesn't impact new usage
				Galaxy.ChangeStatus(shipInput); //changes the status of ship from active to disabled so that players are unable to return to the ship later on
				Ships.DisplayInternal(shipInput); //displays the layout of the ship with the letter of the shipID

			}
			else if (userInput == "N" || userInput == "n")// if N: Flight plan aborted and Checker is called
			{
				Console.WriteLine("Canceling Flight Plan...");
				Decisions.Checker();

			}
			else
            {
				Console.WriteLine("Please Enter a Valid Input."); //error recognition
				Decisions.Travel();
            }
		}
		public static bool LongCheck(String userInput) //method used to check which movement command was inputted by the user
        {
			if (userInput == "D1 R0" || userInput == "D1 R1" || userInput == "D1 R2" || userInput == "D1 R3" || userInput == "D1 R4" || userInput == "D1 R5" || userInput == "D1 R6" || userInput == "D1 R7" || userInput == "D1 R8" || userInput == "D1 R9" || userInput == "D1 R10" || userInput == "D1 R11" )
            {
				
				tempCursorX = Console.CursorLeft;
				tempCursorY = Console.CursorTop;
				char tempChar = userInput[4];
				int tempVal = (int)Char.GetNumericValue(tempChar);
				DroneControl.MoveDrone(1, tempVal);
				return true;
			}
			else if (userInput == "D2 R0" || userInput == "D2 R1" || userInput == "D2 R2" || userInput == "D2 R3" || userInput == "D2 R4" || userInput == "D2 R5" || userInput == "D2 R6" || userInput == "D2 R7" || userInput == "D2 R8" || userInput == "D2 R9" || userInput == "D2 R10" || userInput == "D2 R11")
			{
				char tempChar = userInput[4];
				int tempVal = (int)Char.GetNumericValue(tempChar);
				DroneControl.MoveDrone(2, tempVal);
				return true;
				
			}
			else if (userInput == "D3 R0" || userInput == "D3 R1" || userInput == "D3 R2" || userInput == "D3 R3" || userInput == "D3 R4" || userInput == "D3 R5" || userInput == "D3 R6" || userInput == "D3 R7" || userInput == "D3 R8" || userInput == "D3 R9" || userInput == "D3 R10" || userInput == "D3 R11")
			{
				char tempChar = userInput[4];
				int tempVal = (int)Char.GetNumericValue(tempChar);
				DroneControl.MoveDrone(3, tempVal);
				
				return true;
			} else
            {
				return false;
            }

		}
		public static int GetCursorX()
        {
			return tempCursorX;
        }
		public static int GetCursorY()
		{
			return tempCursorY;
		}

	}
	public class Ships
    {
		public static int mainRoom; //declared int for the number of main rooms
		public static int subRoomOne; //declare int for the number of upper left sub rooms
		public static int subRoomTwo; //declare int for the number of upper right sub rooms
		public static int subRoomThree;//declare int for the number of lower left sub rooms
		public static int subRoomFour;//declare int for the number of lower right sub rooms
		public static int roomChance; //declared int to store the chance of either room appearing when only one room is called.
		public static int roomNum; //declared int to use in the number of rooms on the ship
		public static String cubeDashTop; //String to hold dash to art purposes
		public static String cubeDashBot; //ditto
		public static bool topLeftExist; //boolean that checks if the top left room exists to allow subrooms to be generated
		public static bool botLeftExist; ////boolean that checks if the bottom left room exists to allow subrooms to be generated
		public static int[] roomID = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }; //roomID reference
		public static int[] roomExists = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //int array that couldve been a boolean array that keeps track if a room exists. 0 = false; 1 = true
		public static int[] cubeXPos = { 45, 66, 28, 43, 64, 85, 28, 43, 64, 85, 46, 67 }; //X and Y pos arrays to be accessed later
		public static int[] cubeYPos = { 3, 3, 10, 9, 9, 10, 21, 20, 20, 31, 31, 31 };

		public static void GenerateInternal() //method generates unique interval variables to be used for the layout of the ship and resets booleans to false
        {
			Random rand = new Random(); //new random is called and variables are stored
			mainRoom = rand.Next(3,5); //only variables 3 and 4 are accepted to make things a little easier for drone manipulation and time constraints. 3 rooms or 4 rooms
			subRoomOne = rand.Next(3); //0 = no subrooms, 1 = 1 subrroom, 2 = 2 subrooms
			subRoomTwo = rand.Next(3);
			subRoomThree = rand.Next(3);
			subRoomFour = rand.Next(2); //two options are called because only 1 subrroom is available
			roomChance = rand.Next(2); //room chance is used in scenarios when 1 room is called but 2 options are available and decides which option is chosen
			topLeftExist = false; //booleans set to false
			botLeftExist = false;

		}
		public static void DisplayInternal(char ShipId) //displays the ship layout menus
		{
			Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗"); //upper banner with shipID
			Console.WriteLine("║                                                              Ship " + ShipId  + " Layout                                                                         ║");
			Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
			Decisions.ChangeInstance(3); //changes the check to the ship instance
			ResetArray(); //calls reset method to reset existing room array
			GenerateInternal(); //calls the generate variables method
			if (mainRoom == 3) //if room is 3 then the one room can appear in the either bottom left or top left
            {
				if (roomChance == 0) //if room chance is 0 then bottom left room is created
                {
					CreateCube(64, 9, 18, 9); //top right -Creat Cube Method creates customizable cubes based on cursor position of X and Y and Width and Height of desire cube
					CreateCube(43, 20, 18, 9); //bot left
					CreateCube(64, 20, 18, 9);
					botLeftExist = true; //bottom left boolean is set to true
					roomExists[4] = 1; //updates array to set roomExists array to true(1)
					roomExists[7] = 1;
					roomExists[8] = 1;
				}
				else if (roomChance == 1)//if room chance is 1 then top left room is created
				{
					CreateCube(43, 9, 18, 9); //top left
					CreateCube(64, 9, 18, 9); //top right
					CreateCube(64, 20, 18, 9);
					topLeftExist = true;// top left boolean is set to true
					roomExists[3] = 1;
					roomExists[4] = 1;
					roomExists[8] = 1;
				}
				//Reference for Cubes and their Respecive Static Room ID
				//CreateCube(45, 3, 14, 4); //top left top - 0
				//CreateCube(66, 3, 14, 4); //top right top - 1
				//CreateCube(28, 10, 12, 6); // sub top left left - 2
				//CreateCube(43, 9, 18, 9); //top left - 3
				//CreateCube(64, 9, 18, 9); //top right - 4
				//CreateCube(85, 10, 12, 6); //top right right - 5
				//CreateCube(28, 21, 12, 6); // sub bot left left - 6
				//CreateCube(43, 20, 18, 9); //bot left - 7
				//CreateCube(64, 20, 18, 9); //bot right - 8
				//CreateDock(85, 31, 12, 6); //bottom right right - 9
				//CreateCube(46, 31, 12, 6); // sub bot left bot - 10
				//CreateCube(67, 31, 12, 6); //sub bot right bot - 11
			}
			else if (mainRoom == 4) //if room is 4 then all cubes are created and set to true
            {
				CreateCube(43, 9, 18, 9); //top left
				CreateCube(64, 9, 18, 9); //top right
				CreateCube(43, 20, 18, 9); //bot left
				CreateCube(64, 20, 18, 9);
				topLeftExist = true;
				botLeftExist = true;
				roomExists[3] = 1;
				roomExists[4] = 1;
				roomExists[7] = 1;
				roomExists[8] = 1;
			}
			DroneControl.DroneTable(110, 10, 25, 18); //calling drone table method to create a drone table with info
			DroneControl.DroneShipRep(); //creates the drones rep in the docking station
			CreateSubRooms(); //create sub room method is called
			DroneControl.CanDroneMove(); //method that checks if room can be moved to.
			Console.SetCursorPosition(0, 40); //set cursor to below map layout to continue checker
			Decisions.Checker(); //checker called
		} 
		public static void CreateCube(int xPos, int yPos, int width, int height) //method that creates a cube based on Xpos, YPos, width, and height
        {
			Console.ForegroundColor = ConsoleColor.Blue;
			cubeDashTop = "╔" + String.Concat(Enumerable.Repeat("═", width)) + "╗"; //top part of cube
			cubeDashBot = "╚" + String.Concat(Enumerable.Repeat("═", width)) + "╝"; //bottom part of cube
			Console.SetCursorPosition(xPos, yPos); //cursor set to top left of cube
			
			Console.WriteLine(cubeDashTop); //writes top part
			for(int i = 1; i < height + 1; i++) //for loop to create sides of cube
            {
				Console.SetCursorPosition(xPos, yPos + i);
				Console.WriteLine("║");
				Console.SetCursorPosition(xPos + width + 1, yPos + i);
				Console.WriteLine("║");
			}
			Console.SetCursorPosition(xPos + (width/2), yPos + (height/2)); //set cursor position to inside the cube to write room num
			Console.WriteLine(RoomNum()); //writes roomNum
			ChangeRoomNum(); //changes room num. Increases it by 1. Room Num is different from RoomID as it is respective to the number of rooms that currently exists rather than its direct identifier
			Console.SetCursorPosition(xPos, yPos + height + 1); //sets cursor to bottom of cube
			Console.WriteLine(cubeDashBot); //writes bottom part
		}
		public static void CreateDock(int xPos, int yPos, int width, int height) //exact same instance of createcube but only difference is cololr and internal text
        {
			Console.ForegroundColor = ConsoleColor.Blue;
			cubeDashTop = "╔" + String.Concat(Enumerable.Repeat("═", width)) + "╗";
			cubeDashBot = "╚" + String.Concat(Enumerable.Repeat("═", width)) + "╝";
			Console.SetCursorPosition(xPos, yPos);

			Console.WriteLine(cubeDashTop);
			for (int i = 1; i < height + 1; i++)
			{
				Console.SetCursorPosition(xPos, yPos + i);
				Console.WriteLine("║");
				Console.SetCursorPosition(xPos + width + 1, yPos + i);
				Console.WriteLine("║");
			}
			Console.SetCursorPosition(xPos + 1, yPos + (height / 2));
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Docking Port");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.SetCursorPosition(xPos, yPos + height + 1);
			Console.WriteLine(cubeDashBot);
		}
		public static void CreateSubRooms() //method to create sub rooms. process is as goes. Checks if section exists > if it does then how many rooms were created by the rand > create rooms based on different odds
        {
			if (topLeftExist == true) //if top left exists then
            {
				if (subRoomOne == 1) //if subroom is 1, it checks room chance to see which subroom is created. 2 options are possible
                {
					if (roomChance == 0) //if room chance is 0 then
                    {
						CreateCube(28, 10, 12, 6); //create cube is called
						roomExists[2] = 1; //and subroom is set to exist
					}
					else if (roomChance == 1)
                    {
						CreateCube(45, 3, 14, 4);
						roomExists[0] = 1;
					}
                }
				else if (subRoomOne == 2) // if both rooms are called then both rooms are created
                {
					CreateCube(28, 10, 12, 6);
					CreateCube(45, 3, 14, 4);
					roomExists[2] = 1;
					roomExists[0] = 1;
				}
            }
			if (botLeftExist == true) //if bottom left exists then >(repeat process as above)
			{
				if (subRoomThree == 1)
				{
					if (roomChance == 0)
					{
						CreateCube(28, 21, 12, 6);
						roomExists[6] = 1;
					}
					else if (roomChance == 1)
					{
						CreateCube(46, 31, 12, 6);
						roomExists[10] = 1;
					}
				}
				else if (subRoomThree == 2)
				{
					CreateCube(28, 21, 12, 6); // sub bot left left
					CreateCube(46, 31, 12, 6);
					roomExists[6] = 1;
					roomExists[10] = 1;
				}
			}
			if (subRoomTwo == 1) //since Top Right always exists only room chance is considered when sub room is 1
			{
				if (roomChance == 0)
				{
					CreateCube(66, 3, 14, 4);
					roomExists[1] = 1;
				}
				else if (roomChance == 1)
				{
					CreateCube(85, 10, 12, 6);
					roomExists[5] = 1;
				}
			}
			if (subRoomTwo == 2) //when subroom two is 2 then both are called
			{
				CreateCube(66, 3, 14, 4); //top right top
				CreateCube(85, 10, 12, 6);
				roomExists[1] = 1;
				roomExists[5] = 1;
			}
			if (subRoomFour == 1) //only one option exists for subroom 4
            {
				CreateCube(67, 31, 12, 6);
				roomExists[11] = 1;
			}
			CreateDock(85, 21, 12, 6); //dock is called and array is set to 1
			roomExists[9] = 1;

		}
		public static string RoomNum() //accessor string for current room num
        {
			Console.ForegroundColor = ConsoleColor.White;
			return "R" + roomNum;

        }
		public static void ChangeRoomNum() //mutator string for roomNum
        {
			roomNum++;
			Console.ForegroundColor = ConsoleColor.Blue;

		}
		public static void ResetRoomNum() //reset method from roomNum
		{
			roomNum = 0;
			

		}
		public static void ResetArray() //resets roomExists array for next ship using a for loop
        {
			for (int i = 0; i < roomExists.Length; i++)
            {
				roomExists[i] = 0;
            }
		}
		public static int ReturnArray(int index) //accessor method for roomExists
        {
			return roomExists[index];
        }
		public static int ReturnArrayLength() //accessor method for the length of roomExists
        {
			return roomExists.Length;
        }
		public static int ReturnXPos(int index) //accessor method for CubeXPos
		{
			return cubeXPos[index];
        }
		public static int ReturnYPos(int index) //accessor method for CubeYPos
		{
			return cubeYPos[index];
		}
	}
	public class Galaxy
    {
		public static String[] ships; //String array to be set later on
		public static int[] shipsPosX; //int array to store the Xpos of the ships for display
		public static int[] shipsPosY;//int array to store the Ypos of the ships for display
		public static int indexAt; //int to store index of active true later
		public static char[] shipID = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //ship ID array for list and display purposes
		public static int[] activeTrue = {0, 0, 0, 0, 0, 0, 0, 0, 0 , 0 }; //int that could've been a boolean array to store if a ship is active/available
		public static String isActive = "Active"; //string storing active
		public static String isDisabled = "Disabled"; //string storing disabled
		public static int shipNum; //stores number of ships that were created
		public static Boolean HasGenerated = false; //used to indicate if ships have been already created or not
		
		public Galaxy()
        {
			Console.Clear(); //clears console
			
		}
		public static void GenerateShips() //method to generate number of ships, their positions, and what type they are
        {
			Random rand = new Random();
		    shipNum = rand.Next(6, 9); //random number of ships set to shipNum
			ships = new string[shipNum]; //ships array set with number of ships
			shipsPosX = new int[shipNum]; //ships Xpos array set with number of ships
			shipsPosY = new int[shipNum]; //ships Ypos array set with number of ships
			int shipType; //local var type created
			int tempPos; //temp var to hold xpos variable
			for (int i = 0; i < shipNum ; i++) //for loop that decides type of ship by calling a random number in 100; percentanges are drawn be sectioning out different values to skew results. Freight > Military > Personal
            {
				shipType = rand.Next(100);
				if (shipType >= 0 && shipType <= 45) { //if value falls between a certain point then the ship array is updated with its type
					ships[i] = "Freight";
				}
				if (shipType >= 46 && shipType <= 80)
				{
					ships[i] = "Military";
				}
				if (shipType >= 81 && shipType <= 99)
				{
					ships[i] = "Personal";
				}

			}
			for (int i = 0; i < shipNum; i++ ) //for loop to set x and y position for ship and set the position to their respective array
            {
				tempPos = rand.Next(3, 148);
				shipsPosX[i] = tempPos;
				tempPos = rand.Next(3, 30);
				shipsPosY[i] = tempPos;
			}
        }
		public static void SolarReference() //method that displays the position for the ships using its respective array and ShipID and calls the checker
        {
			for (int i = 0; i < shipNum; i++)
            {
				Console.SetCursorPosition(shipsPosX[i], shipsPosY[i]);
				Console.WriteLine("╔═╗");
				Console.SetCursorPosition(shipsPosX[i], shipsPosY[i]+1);
				Console.WriteLine("║" + shipID[i] + "║");
				Console.SetCursorPosition(shipsPosX[i], shipsPosY[i] + 2);
				Console.WriteLine("╚═╝");
			}
			Console.SetCursorPosition(0, 32);
			Decisions.Checker();
        }
		public static void ShowSystem() //shows the solar system
        {
			Console.Clear(); //clears the console
			Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗"); //sets the banner
			Console.WriteLine("║                                    Drone Config [D] ║ Galaxy Map [G] ║ Travel [T] ║ Help [Help] ║ Notifications[] [N]                              ║");
			Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
			Decisions.ChangeInstance(0); //changes the instance to solar system
			if (HasGenerated == false) //checks if the ships have generated before. if they haven't then:
            {
				Galaxy.GenerateShips(); //generate new ships
				HasGenerated = true; //set state to true
            }
			Galaxy.SolarReference(); //call method to show positions of the ships
			
		}
		public static int ShipNum() //accessor for the ship num
        {
			return shipNum;
        }
		public static string Ships(int ArrPlace) //accessor for the ships array
        {
			return ships[ArrPlace];
        }
		public static char ShipID(int arrPlace) //accessor for the shipID
        {
			return shipID[arrPlace];
        }
		public static string ShipStatus(int check) //accessor is that checks the status of a ship at the same index and returns a message based on its state. 0 = active; 1 is disabled.
        {
			if (activeTrue[check] == 1 )
            {
				return isDisabled;
            }
			else
            {
				return isActive;
            }
        }
		public static bool CheckStatus(char check) //accessor that checks the status and returns a true or false
		{
			for (int i = 0; i < shipID.Length; i++)
			{
				if (shipID[i] == check)
				{
					indexAt = i;
				}
			}
			if (activeTrue[indexAt] == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static void ChangeStatus(char shipChar)//mutator method that chances the status of a ship at a certain index
        {
			for (int i = 0; i < shipID.Length; i++)
            {
				if (shipID[i] == shipChar)
                {
					activeTrue[i] = 1;
                }
            }
        }
    }
	public class Notifications
    {
		static string dash = string.Concat(Enumerable.Repeat("-", 149)); //String var for dash
		public static void ShowNotifications()//method to show notifications menu
        {
			Console.Clear(); //clears console and sets banner
			Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                                    Drone Config [D] ║ Galaxy Map [G] ║ Travel [T] ║ Help [Help] ║ Solar System [S]                                 ║");
			Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
			Decisions.ChangeInstance(1); //changes the instance to the notification instance
			Console.SetCursorPosition(71, 5); //sets cursor position
			Console.WriteLine("Messages");
			FormatNotifications(); //calls format notifications to display the rest of notifications
			Decisions.Checker(); //calls for the checker
			
		}
		static void FormatNotifications() //method that displays an inbox like menu for notifications that were sent.
        {
			Console.SetCursorPosition(2, 7);
			Console.WriteLine("Entry #");
			Console.SetCursorPosition(23, 7);
			Console.WriteLine("Sender");
			Console.SetCursorPosition(71, 7);
			Console.WriteLine("Subject");
			Console.SetCursorPosition(0, 8);
			Console.WriteLine(dash);
			for (int i = 0; i < 21; i++)
            {
				Console.SetCursorPosition(2, i + 9);
				Console.WriteLine(i);
				Console.SetCursorPosition(11, i + 9);
				Console.WriteLine("|");
				Console.SetCursorPosition(43, i + 9);
				Console.WriteLine("|");
			}
		}
		public static void UpdateNotifications()
        {

        }
    }
	public class DroneMenu
    {
		public static void ShowDrone() //shows the drone menu
		{
			Console.Clear(); //clears console and sets banner
			Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                                    Solar System [S] ║ Galaxy Map [G] ║ Travel [T] ║ Help [Help] ║ Notifications[] [N]                              ║");
			Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
			Decisions.ChangeInstance(2); //changes instance to drone menu
			Console.SetCursorPosition(66, 5); //writes and formats drone menu
			Console.WriteLine("Drone Configuration");
			Console.SetCursorPosition(69, 6);
			Console.WriteLine("Active Drones");
			FormatDrone(8); //calls format drone method with a y parameter to continue writing out the drone menu for active drones
			Console.SetCursorPosition(67, 14 );
			Console.WriteLine("Deactivated Drones");
			FormatDrone(16); //calls format drone method with a y parameter to continue writing out the drone menu for inactive drones
			Decisions.Checker();

		}
		static void FormatDrone(int yVal) //method that creates a layout for drones to be inserted and information to be written about them given a y parameter
		{
			Console.SetCursorPosition(2, yVal);
			Console.WriteLine("Drone #");
			Console.SetCursorPosition(23, yVal);
			Console.WriteLine("Drone Name");
			Console.SetCursorPosition(45, yVal);
			Console.WriteLine("Ability 1");
			Console.SetCursorPosition(56, yVal);
			Console.WriteLine("Ability 2");
			Console.SetCursorPosition(68, yVal);
			Console.WriteLine("Health");
			Console.SetCursorPosition(0, yVal + 1);
			for (int i = 1; i < 4; i++)
			{
				Console.SetCursorPosition(2, i + yVal + 1);
				Console.WriteLine(i);
				Console.SetCursorPosition(11, i + yVal + 1);
				Console.WriteLine("-");
				Console.SetCursorPosition(43, i + yVal + 1);
				Console.WriteLine("-");
				Console.SetCursorPosition(54, i + yVal + 1);
				Console.WriteLine("-");
				Console.SetCursorPosition(66, i + yVal + 1);
				Console.WriteLine("-");
			}
		}
	}
	public class DroneControl
	{
		public static int[] droneHealth = new int[3];
		public static bool[] upMovement = new bool[12]; //boolean array to set if a drone can access a room using the cardinal directions and if the room exists
		public static bool[] downMovement = new bool[12];
		public static bool[] leftMovement = new bool[12];
		public static bool[] rightMovement = new bool[12];
		public static String[] droneName = { "Larry", "Moe", "Curly" }; //Static Drone Names due to Time
		public static int[] droneXPos = new int[3]; //int array to store drone positions
		public static int[] droneYPos = new int[3];
		public static int[] roomNum; //local variable to deal with room Numbers
		public static char stringChecker; //used to check strings to move drone




		public static void GenerateArray() //sets all of the arrays to false
		{
			for (int i = 0; i < upMovement.Length; i++)
			{
				upMovement[i] = false;
			}
			for (int i = 0; i < downMovement.Length; i++)
			{
				downMovement[i] = false;
			}
			for (int i = 0; i < leftMovement.Length; i++)
			{
				leftMovement[i] = false;
			}
			for (int i = 0; i < rightMovement.Length; i++)
			{
				rightMovement[i] = false;
			}

		}
		public static void MapCheck() //unused method to check if a drone could be moved
		{
			GenerateArray();
			for (int i = 0; i < Ships.ReturnArrayLength(); i++)//uses the length of the array to run roomcheck to set which rooms are accesible through the cardinal directions
			{
				RoomCheck(i);
			}
		}

		public static void RoomCheck(int roomNum) //method that uses the room exists array to set if movement in said direction is possible. RoomId is used to identify the room and if statments are used to check given the room number and the result if a room exists
		{
			if (roomNum == 0) //if room is 0(refer back to the reference block) then 
			{
				if (Ships.ReturnArray(1) == 1) // if the room to the right exists then 
				{
					rightMovement[0] = true; //movement to the right is available
				}
				if (Ships.ReturnArray(3) == 1) //if room to the bottom exists then 
				{
					downMovement[0] = true; //movement to the bottom is available

				}
			}
			else if (roomNum == 1) //rinse and repeat
			{
				if (Ships.ReturnArray(0) == 1)
				{
					leftMovement[1] = true;
				}
				if (Ships.ReturnArray(4) == 1)
				{
					downMovement[0] = true;

				}
			}
			else if (roomNum == 2)
			{
				if (Ships.ReturnArray(3) == 1)
				{
					rightMovement[2] = true;
				}
				if (Ships.ReturnArray(6) == 1)
				{
					downMovement[2] = true;

				}
			}
			else if (roomNum == 3)
			{
				if (Ships.ReturnArray(4) == 1)
				{
					rightMovement[3] = true;
				}
				if (Ships.ReturnArray(7) == 1)
				{
					downMovement[3] = true;

				}
				if (Ships.ReturnArray(2) == 1)
				{
					leftMovement[3] = true;

				}
				if (Ships.ReturnArray(0) == 1)
				{
					upMovement[3] = true;

				}
			}
			else if (roomNum == 4)
			{
				if (Ships.ReturnArray(5) == 1)
				{
					rightMovement[4] = true;

				}
				if (Ships.ReturnArray(8) == 1)
				{
					downMovement[4] = true;

				}
				if (Ships.ReturnArray(3) == 1)
				{
					leftMovement[4] = true;

				}
				if (Ships.ReturnArray(1) == 1)
				{
					upMovement[4] = true;

				}
			}
			else if (roomNum == 5)
			{
				if (Ships.ReturnArray(4) == 1)
				{
					leftMovement[5] = true;

				}
			}
			else if (roomNum == 6)
			{
				if (Ships.ReturnArray(7) == 1)
				{
					rightMovement[6] = true;

				}
				if (Ships.ReturnArray(2) == 1)
				{
					upMovement[6] = true;

				}
			}
			else if (roomNum == 7)
			{
				if (Ships.ReturnArray(8) == 1)
				{
					rightMovement[7] = true;

				}
				if (Ships.ReturnArray(10) == 1)
				{
					downMovement[7] = true;

				}
				if (Ships.ReturnArray(6) == 1)
				{
					leftMovement[7] = true;

				}
				if (Ships.ReturnArray(3) == 1)
				{
					upMovement[7] = true;

				}
			}
			else if (roomNum == 8)
			{
				if (Ships.ReturnArray(9) == 1)
				{
					rightMovement[8] = true;

				}
				if (Ships.ReturnArray(11) == 1)
				{
					downMovement[8] = true;

				}
				if (Ships.ReturnArray(7) == 1)
				{
					leftMovement[8] = true;

				}
				if (Ships.ReturnArray(4) == 1)
				{
					upMovement[8] = true;

				}
			}
			else if (roomNum == 9)
			{
				if (Ships.ReturnArray(8) == 1)
				{
					leftMovement[9] = true;

				}
			}
			else if (roomNum == 10)
			{
				if (Ships.ReturnArray(11) == 1)
				{
					rightMovement[10] = true;

				}
				if (Ships.ReturnArray(7) == 1)
				{
					upMovement[10] = true;

				}
			}
			else if (roomNum == 11)
			{
				if (Ships.ReturnArray(10) == 1)
				{
					leftMovement[11] = true;

				}
				if (Ships.ReturnArray(8) == 1)
				{
					upMovement[11] = true;

				}
			}
		}

		public static void DroneShipRep() //method that starts off the drone in the Docking Port/Station
		{
			int tempX = 90;
			int tempY = 24;
			
			for (int i = 1; i <= 3; i++)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.SetCursorPosition(tempX, tempY + i);
				Console.WriteLine("[D" + i + "]");
				droneXPos[i - 1] = tempX;
				droneYPos[i - 1] = tempY + i;
				Console.ForegroundColor = ConsoleColor.Cyan;

			}
			

		}
		public static void CanDroneMove()
        {
			int tempCount = 0;
			int tempCount2 = 0;
			int[] tempIndex = new int[12];
			int[] baseRoom = { 3, 4, 7, 8 }; //Main rooms are given first priority and need to be added to the array first
			for (int i = 0; i < 11; i++) //checks available rooms and availability and inputs into an array
			{
				if (Ships.ReturnArray(i) == 1)
				{
					tempIndex[tempCount] = i;
					tempCount++;
				}
			}
			roomNum = new int[tempCount + 1];

			for (int i = 0; i < 11; i++)
			{
				if (Ships.ReturnArray(i) == 1)
				{
					for(int j = 0; j < 4; j++)
                    {
						if (i == baseRoom[j])
						{
							roomNum[tempCount2] = i;
							tempCount2++;
						}
					}
				}
			}
			if (Ships.ReturnArray(2) == 1)
            {
				roomNum[tempCount2] = 2;
				tempCount2++;
			}
			for (int i = 0; i < 2; i++) //checks available rooms and availability and inputs into an array
			{
				if (Ships.ReturnArray(i) == 1)
				{
					roomNum[tempCount2] = i;
					tempCount2++;
				}
			}
			for (int i = 5; i < 7; i++) //checks available rooms and availability and inputs into an array
			{
				if (Ships.ReturnArray(i) == 1)
				{
					roomNum[tempCount2] = i;
					tempCount2++;
				}
			}
			for (int i = 9; i < 11; i++) //checks available rooms and availability and inputs into an array
			{
				if (Ships.ReturnArray(i) == 1)
				{
					roomNum[tempCount2] = i;
					tempCount2++;
				}
			}
		}
		public static void DroneTable(int xPos, int yPos, int width, int height) //same base as createCub() but creates a drone menu with active drones and additional info
		{
			Console.ForegroundColor = ConsoleColor.Red;
			string cubeDashTop = "╔" + String.Concat(Enumerable.Repeat("═", width)) + "╗";
			string cubeDashBot = "╚" + String.Concat(Enumerable.Repeat("═", width)) + "╝";
			int counter = 1; //int counter seperate from the i for loop for proper position and use in the index of drone health array

			GenerateDroneHealth();

			Console.SetCursorPosition(xPos, yPos);
			Console.WriteLine(cubeDashTop);

			for (int i = 1; i < height + 1; i++)
			{
				Console.SetCursorPosition(xPos, yPos + i);
				Console.WriteLine("║");
				Console.SetCursorPosition(xPos + width + 1, yPos + i);
				Console.WriteLine("║");
			}

			Console.SetCursorPosition(xPos + 6, yPos + 1);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Active Drones");

			for (int i = 0; i < 15; i += 5) //for loop to generate internal elements
			{
				Console.SetCursorPosition(xPos + 1, yPos + 3 + i);
				Console.WriteLine("Drone " + counter + ":");

				Console.SetCursorPosition(xPos + 10, yPos + 3 + i);
				Console.WriteLine(droneName[counter - 1]);

				Console.SetCursorPosition(xPos + 3, yPos + 4 + i);
				Console.WriteLine("Health: ");

				Console.SetCursorPosition(xPos + 11, yPos + 4 + i);
				Console.WriteLine(droneHealth[counter - 1]);

				Console.SetCursorPosition(xPos + 3, yPos + 5 + i);
				Console.WriteLine("Ability: ");

				counter++;
			}
			Console.SetCursorPosition(xPos, yPos + height + 1);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(cubeDashBot);

			Console.ForegroundColor = ConsoleColor.Blue;
		}
		public static void GenerateDroneHealth()
		{
			for (int i = 0; i < droneHealth.Length; i++) //when DroneHealth is first called set drone health to 70;
			{
				droneHealth[i] = 70;
			}
		}
		public static void MoveDrone(int droneNum, int rmNum) //method to clear the drones previous location and move it towars an available site.
		{
			Console.SetCursorPosition(droneXPos[droneNum - 1], droneYPos[droneNum - 1]);
			Console.WriteLine("    ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(Ships.ReturnXPos(roomNum[rmNum]) + 8, Ships.ReturnYPos(roomNum[rmNum]) + 4 + droneNum);
			Console.WriteLine("[D" + droneNum + "]");
			droneXPos[droneNum - 1] = Ships.ReturnXPos(roomNum[rmNum]) + 8;
			droneYPos[droneNum - 1] = Ships.ReturnYPos(roomNum[rmNum]) + 4 + droneNum;
			Console.SetCursorPosition(Decisions.GetCursorX(), Decisions.GetCursorY());
			Console.ForegroundColor = ConsoleColor.Cyan;
			for (int i = 0; i < roomNum.Length; i++)
            {
				Console.Write(roomNum[i] + " ");
            }
			Console.WriteLine("");

		}
	
	}
	public class Animations
	{
		int counter; //simple int counter to keep track of moving dots

		//Following animation Code was Adapted:
		//Author: João Lebre
		//Site: https://gist.github.com/jplebre/fc2979cf2d1f23f93c89
		public Animations()
		{
			counter = 0; //sets counter to 0
			Console.SetWindowSize(150, 50); //sets console size
			Console.ForegroundColor = ConsoleColor.Cyan; //sets console font color

		}
		public void Counter() //mutator method that increases count
        {
			counter++;
        }
		public int GetCounter() //accessor method that gives counter back
		{
			return counter;
		}
		public void LoadingBar(string loadingTest, int row, int column) //method that mimics a loading bar but was changed to display dots
		{
			Console.SetCursorPosition(column, row); //set cursor to parameters
			string loadingText = loadingTest; //set text to paramets
			Console.Write(loadingText); //write text

			for (int i = 0; i < counter % 12; i++)
			{
				if (counter == 0) //if counter is 0 write loading text at set positions. 
				{
					Console.SetCursorPosition(column, row);
					Console.Write(loadingText);
				}

				Console.SetCursorPosition(loadingText.Length + i, row);//change cursor position and increase each loop
				Console.Write("."); //write in a period(.)
			}
		}
		public static void StartScreen()//method to start off the game
		{
			Animations spin = new Animations();//calling a new animation
			string[] loadingText = { "Initializing Software", "Seeking Connection", "Securing Connection", "Finalizing" }; //string array to ease the use of using multiple animations
			string userInput; //userInput storage variable
			Console.CursorVisible = false; //set cursor to false for prettier animation
			
				for (int i = 0; i < 4; i++) //for loop that runs through the initial animation through the array and increases the y each time to seperate the lines
				{
					for (int j = 0; j < 12; j++) {
						spin.LoadingBar(loadingText[i], i, 0);
						spin.Counter();
						System.Threading.Thread.Sleep(100);
				}
			}
			Console.WriteLine(""); //title displayed with options
			Console.WriteLine("");
			Console.WriteLine("		███╗   ███╗██╗██████╗ ███╗   ██╗██╗ ██████╗ ██╗  ██╗████████╗    ██████╗  █████╗ ██╗   ██╗███████╗███╗   ██╗███████╗");
			Console.WriteLine("		████╗ ████║██║██╔══██╗████╗  ██║██║██╔════╝ ██║  ██║╚══██╔══╝    ██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║██╔════╝");
			Console.WriteLine("		██╔████╔██║██║██║  ██║██╔██╗ ██║██║██║  ███╗███████║   ██║       ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║███████╗");
			Console.WriteLine("		██║╚██╔╝██║██║██║  ██║██║╚██╗██║██║██║   ██║██╔══██║   ██║       ██╔══██╗██╔══██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║╚════██║");
			Console.WriteLine("		██║ ╚═╝ ██║██║██████╔╝██║ ╚████║██║╚██████╔╝██║  ██║   ██║       ██║  ██║██║  ██║ ╚████╔╝ ███████╗██║ ╚████║███████║");
			Console.WriteLine("		╚═╝     ╚═╝╚═╝╚═════╝ ╚═╝  ╚═══╝╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝       ╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝╚═╝  ╚═══╝╚══════╝");
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Select an Option. Type The Number And Press Enter:");
			Console.WriteLine("1.) Initiate Drone Program");
			Console.WriteLine("2.) Tutorial");
			Console.WriteLine("3.) Program Credits");
			Console.CursorVisible = true;
			userInput = Console.ReadLine(); //sets user Input to userInput
			if (userInput == "1") //if user inputs a 1 then continue on to show system
            {
				Galaxy.ShowSystem();

            }
			else if(userInput == "3") //if user inputs a 3 then displays my name and info
            {
				Console.WriteLine("Made by G.Rodriguez. C. 2020.");
				System.Threading.Thread.Sleep(5000);
				Console.WriteLine("Restarting");
				System.Threading.Thread.Sleep(500);
				Console.Clear(); //clears screen and starts the screen again
				Animations.StartScreen();
			}
			else if (userInput == "2")//if user inputs a 3 then displays a rather rude help message for the next menu
			{
				Console.WriteLine("You Should Have Learned How To Use This In Training. Type Help In Next Console For Available Commands.");
				System.Threading.Thread.Sleep(5000);
				Console.WriteLine("Restarting");
				System.Threading.Thread.Sleep(500);
				Console.Clear();
				Animations.StartScreen();
			}
			else //if a command is not recognized then input is denied and scene is restarted.
            {
				Console.WriteLine("Input Denied. Restarting");
				System.Threading.Thread.Sleep(500);
				Console.Clear();
				Animations.StartScreen();
			}
		}
	}
}