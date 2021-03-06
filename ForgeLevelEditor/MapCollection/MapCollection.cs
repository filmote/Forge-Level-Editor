﻿using ForgeLevelEditor.map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForgeLevelEditor.mapCollection
{
    public class MapCollection
    {
        public string FilePath = "";
        public string FileName = "";

        int ViewWidth = 10;
        int ViewHeight = 10;
        int DrawOffsetX = 0;
        int DrawOffsetY = 0;
        public int drawOffsetX { get => DrawOffsetX; set { DrawOffsetX = value;  UpdateViewParams(); } }
        public int drawOffsetY { get => DrawOffsetY; set { DrawOffsetY = value; UpdateViewParams(); } }
        public int viewWidth { get => ViewWidth; set { ViewWidth = value; UpdateViewParams(); } }
        public int viewHeight { get => ViewHeight; set { ViewHeight = value; UpdateViewParams(); } }

        void UpdateViewParams()
        {
            foreach(Map map in OpenMaps)
            {
                map.drawOffsetX = DrawOffsetX;
                map.drawOffsetY = DrawOffsetY;
                map.viewHeight = ViewHeight;
                map.viewWidth = ViewWidth;
            }
        }

        bool DrawBackground = true;
        bool DrawSprites = true;
        bool DrawConnections = true;
        bool DrawPlayer = true;

        List<Map> OpenMaps = new List<Map>();
        public Map CurrentMap = null;

        public void MoveCurrentMapUp() {

            int index = OpenMaps.IndexOf(CurrentMap);
            OpenMaps.Remove(CurrentMap);
            OpenMaps.Insert(index - 1, CurrentMap);

        }

        public void MoveCurrentMapDown() {

            int index = OpenMaps.IndexOf(CurrentMap);
            OpenMaps.Remove(CurrentMap);
            OpenMaps.Insert(index + 1, CurrentMap);

        }

        public int OpenCount
        {
            get
            {
                return OpenMaps.Count;
            }
        }

        public bool drawBackground { get => DrawBackground; set => DrawBackground = value; }
        public bool drawSprites { get => DrawSprites; set => DrawSprites = value; }
        public bool drawConnections { get => DrawConnections; set => DrawConnections = value; }
        public bool drawPlayer { get => DrawPlayer; set => DrawPlayer = value; }

        public void AddMap(Map map)
        {
            map.drawOffsetX = DrawOffsetX;
            map.drawOffsetY = DrawOffsetY;
            map.viewHeight = ViewHeight;
            map.viewWidth = ViewWidth;
            if (CurrentMap == null)
                CurrentMap = map;
            OpenMaps.Add(map);
        }

        public void AddMaps(IEnumerable<Map> maps)
        {
            foreach(Map map in maps)
                if(map != null)
                    this.AddMap(map);
        }


        public void RemoveMap(Map map)
        {
            OpenMaps.Remove(map);
            if (map == CurrentMap)
            {
                CurrentMap = null;
                if(OpenMaps.Count > 0)
                    CurrentMap = OpenMaps[0];
            }
        }

        public bool ChangeMap(Map map)
        {
            if(CurrentMap == null)
            {
                if (OpenMaps.Count > 0)
                {
                    CurrentMap = OpenMaps[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if(CurrentMap != map)
            {
                CurrentMap = map;
                return true;
            }
            return false;
        }

        public void Draw(Graphics graphics)
        {
            if (this.CurrentMap == null)
                return;

            if (DrawBackground)
                CurrentMap.DrawBackground(graphics);

            if (DrawSprites)
                CurrentMap.DrawSprites(graphics);

            if (DrawConnections)
                CurrentMap.DrawConnections(graphics);

            if (DrawPlayer)
                CurrentMap.DrawPlayer(graphics);
        }

        private static readonly Regex mapLoadRegex = new Regex(@"const\suint8_t\s*\S+\[\s*\]\s*\=\s*\{.*?\}\;");
        private static readonly HashSet<char> mapLoadCharacters = new HashSet<char>() { ',', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        
        public static IEnumerable<Map> LoadMaps(string FilePath)
        {
            var fileData = string.Join(" ", File.ReadLines(FilePath));

            for (Match reg = mapLoadRegex.Match(fileData);  reg.Success; reg = reg.NextMatch())
            {
                string[] Vals = reg.Value.Split('{', '}');
                if (Vals.Length == 3)
                {
                    string NameFind = Vals[0];
                    int endofName = NameFind.IndexOf('[');
                    int startofName = NameFind.IndexOf("uint8_t") + 7;
                    string Name = NameFind.Substring(startofName, endofName - startofName).Trim();
                        
                    string Data = string.Join("", Vals[1].Where(mapLoadCharacters.Contains));

                    string[] SplitData = Data.Split(',');
                    byte[] DataArray = new byte[SplitData.Length];
                    for (int i = 0; i < SplitData.Length; i++)
                    {
                        if (byte.TryParse(SplitData[i], out DataArray[i]))
                        {
                                
                        }
                    }

                    Map newmap = Map.ParseMapArray(DataArray, Name, FilePath);

                    if (newmap != null)
                        yield return newmap;
                }
            }
        }

        public void SaveMaps()
        {
            try
            {
                var savePath = Path.Combine(FilePath, FileName);

                using (var writer = new StreamWriter(savePath))
                {
                    writer.WriteLine("#pragma once");
                    writer.WriteLine();
                    writer.WriteLine("#include <stdint.h>");
                    writer.WriteLine();

                    foreach (var map in this.OpenMaps)
                    {
                        map.WriteMap(writer);
                        writer.WriteLine();
                    }

                    writer.WriteLine("constexpr const uint8_t numberOfMaps = {0};", OpenMaps.Count);
                    writer.WriteLine();

                    writer.Write("constexpr const uint8_t* maps[numberOfMaps] = ");
                    writer.Write("{ ");
                    foreach (var map in this.OpenMaps)
                    {
                        writer.Write(map.Name);
                        writer.Write(',');
                    }
                    writer.WriteLine(" };");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<Map> GetMaps()
        {
            return OpenMaps;
        }


    }
}
