﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static RogueboyLevelEditor.TextureHandler.TextureManager;
using System.Xml.Linq;

namespace RogueboyLevelEditor.map.Component
{
    public class Sprite
    {
        public string Name { get; private set; }
        public string TextureID { get; private set; }

        public Sprite(string Name, string TextureID)
        {
            this.Name = Name;
            this.TextureID = TextureID;
        }
    }

    public class SpriteManager
    {
        static Dictionary<int, Sprite> Sprites;

        public ExceptionReport Load(string Filepath)
        {
            
            try
            {
                XDocument xmlDoc = XDocument.Load(Filepath);
                List<XElement> Nodes = (from element in xmlDoc.Descendants("sprites").Elements() where element.Name == "sprite" select element).ToList();
                foreach (XElement x in Nodes)
                {
                    List<XElement> Daughters = x.Descendants().ToList();
                    int ID = int.Parse(Daughters.Find(o => o.Name == "id").Value);
                    string Name = Daughters.Find(o => o.Name == "name").Value;
                    string TextureID = Daughters.Find(o => o.Name == "texture").Value;

                    AddSprite(ID, new Sprite(Name, TextureID));
                }
            }
            catch (Exception e)
            {
                return new ExceptionReport() { Failed = true, exception = e };
            }
            return new ExceptionReport() { Failed = false, exception = null };
        }

        public SpriteManager()
        {

            if (Sprites == null)
            {
                Sprites = new Dictionary<int, Sprite>();
            }

        }

        public List<Tuple<int, Sprite>> GetAllSprites()
        {
            List<Tuple<int, Sprite>> OutSprites = new List<Tuple<int, Sprite>>();
            foreach (KeyValuePair<int, Sprite> i in Sprites)
            {
                OutSprites.Add(new Tuple<int, Sprite>(i.Key, i.Value));
            }
            return OutSprites;
        }


        public void AddSprite(int ID, Sprite Sprite)
        {
            if (Sprites.ContainsKey(ID))
            {
                return;
            }
            Sprites.Add(ID, Sprite);
        }

        public Sprite GetSprite(int ID)
        {
            if (Sprites.ContainsKey(ID))
            {
                return Sprites[ID];
            }
            return new Sprite("Null", "Null");
        }
    }
}
