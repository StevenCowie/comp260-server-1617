﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server
{
    public class Dungeon
    {        
        public Dictionary<String, Room> roomMap;

        Room currentRoom;

        public void Init()
        {
            roomMap = new Dictionary<string, Room>();
            {
                var room = new Room("Room 0", "You are standing in the entrance hall\nAll adventures start here");
                room.north = "Room 1";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 1", "You are in room 1");
                room.south = "Room 0";
                room.west = "Room 3";
                room.east = "Room 2";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 2", "You are in room 2");
                room.north = "Room 4";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 3", "You are in room 3");
                room.east = "Room 1";
                room.north = "Room 6";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 4", "You are in room 4");
                room.south = "Room 2";
                room.west = "Room 5";
                room.north = "Room 11";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 5", "You are in room 5");
                room.south = "Room 1";
                room.east = "Room 4";
                room.west = "Room 6";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 6", "You are in room 6");
                room.south = "Room 3";
                room.east = "Room 5";
                room.west = "Room 7";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 7", "You are in room 7");
                room.north = "Room 10";
                room.east = "Room 6";
                room.west = "Room 8";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 8", "You are in room 8");
                room.south = "Room 9";
                room.east = "Room 7";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 9", "You are in room 9");
                room.north = "Room 8";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 10", "You are in room 10");
                room.north = "Room 15";
                room.south = "Room 7";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 11", "You are in room 11");
                room.north = "Room 12";
                room.south = "Room 4";
                room.east = "Room 18";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 12", "You are in room 12");
                room.south = "Room 11";
                room.west = "Room 13";
                room.east = "Room 17";
                roomMap.Add(room.name, room);  
            }
            
            {
                var room = new Room("Room 13", "You are in room 13");
                room.west = "Room 14";
                room.east = "Room 12";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 14", "You are in room 14");
                room.north = "Room 16";
                room.west = "Room 15";
                room.east = "Room 13";
                roomMap.Add(room.name, room);
            }
            
            {
                var room = new Room("Room 15", "You are in room 15");
                room.south = "Room 10";
                roomMap.Add(room.name, room);                
            }
            
            {
                var room = new Room("Room 16", "You are in room 16");
                room.south = "Room 14";
                roomMap.Add(room.name, room);                
            }
            
            {
                var room = new Room("Room 17", "You are in room 17");
                room.south = "Room 18";
                room.north = "Room 19";
                room.west = "Room 12";
                roomMap.Add(room.name, room);                
            }
            
            {
                var room = new Room("Room 18", "You are in room 18");
                room.north = "Room 17";
                roomMap.Add(room.name, room);                
            }
            
            {
                var room = new Room("Room 19", "You are in room 19");
                room.south = "Room 17";
                room.east = "Room 20";
                roomMap.Add(room.name, room);                
            }
            
            {
                var room = new Room("Room 20", "You are in room 20");
                room.west = "Room 19";
                roomMap.Add(room.name, room);                
            }

            currentRoom = roomMap["Room 0"];
        }

        public String SendInfo(Player player)
        {
            currentRoom = player.currentRoom;
            String info = "";
            info += currentRoom.desc;
            info += "\nExits\n";
            for (var i = 0; i < currentRoom.exits.Length; i++)
            {
                if (currentRoom.exits[i] != null)
                {
                    info += (Room.exitNames[i] + " ");
                }
            }
            return info;

        }

        public string Process(string Key, Player player, int PlayerID)
        {
            currentRoom = player.currentRoom;
            String returnString = "";
            var input = Key.Split(' ');

            switch (input[0].ToLower())
            {
                case "help":
                    Console.Clear();
                    returnString += ("\nCommands are ....");
                    returnString += ("help - for this screen");
                    returnString += ("look - to look around");
                    returnString += ("go [north | south | east | west]  - to travel between locations");
                    returnString += ("\nPress any key to continue");
                    returnString += ("\n" + currentRoom.desc);
                    returnString += ("\nExits");
                    for (var i = 0; i < currentRoom.exits.Length; i++)
                    {
                        if (currentRoom.exits[i] != null)
                        {
                            returnString += (" " + Room.exitNames[i] + " ");
                        }
                    }
                    return returnString;

                case "look":
                    Thread.Sleep(1000);
                    returnString += SendInfo(player);
                    return returnString;

                case "say":
                    returnString += ("Player " + PlayerID + " : ");
                    for (var i = 1; i < input.Length; i++)
                    {
                        returnString += (input[i] + " ");
                    }

                    Thread.Sleep(1000);
                    returnString += SendInfo(player);
                    return returnString;

                case "go":
                    if ((input[1].ToLower() == "north") && (currentRoom.north != null))
                    {
                        player.currentRoom = roomMap[currentRoom.north];
                    }
                    else
                    {
                        if ((input[1].ToLower() == "south") && (currentRoom.south != null))
                        {
                            player.currentRoom = roomMap[currentRoom.south];
                        }
                        else
                        {
                            if ((input[1].ToLower() == "east") && (currentRoom.east != null))
                            {
                                player.currentRoom = roomMap[currentRoom.east];
                            }
                            else
                            {
                                if ((input[1].ToLower() == "west") && (currentRoom.west != null))
                                {
                                    player.currentRoom = roomMap[currentRoom.west];
                                }
                                else
                                {
                                    //handle error
                                    returnString += ("\nERROR");
                                    returnString += ("\nCan not go " + input[1] + " from here");
                                    returnString +=  ("\nPress any key to continue");
                                }
                            }
                        }
                    }
                    returnString += SendInfo(player);
                    return returnString;

                default:
                    //handle error
                    returnString += ("\nERROR");
                    returnString += ("\nCan not " + Key);
                    returnString += ("\nPress any key to continue");
                    //Console.ReadKey(true);
                    return returnString;
            }


            return returnString;
        }
    }
}