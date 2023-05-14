using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MapWriter
{
    public void Write(string path, int width, int height, Dictionary<Vector3, TileInfo> tileInfoDic) 
    {
        Vector3 position;
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(width + " " + height + "\n");

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (j > 0)
                    {
                        writer.Write(" ");
                    }

                    position = new Vector3(i, 0, j);
                    if (tileInfoDic.ContainsKey(position))
                    {
                        writer.Write(tileInfoDic[position].TileID);
                    }
                    else
                    {
                        writer.Write("X");
                    }
                }
                writer.Write("\n");
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (j > 0)
                    {
                        writer.Write(" ");
                    }

                    position = new Vector3(i, 0, j);
                    if (tileInfoDic.ContainsKey(position))
                    {
                        if (tileInfoDic[position].AttachID != null)
                        {
                            writer.Write(tileInfoDic[position].AttachID);
                        }
                        else
                        {
                            writer.Write("X");
                        }
                    }
                    else
                    {
                        writer.Write("X");
                    }
                }
                writer.Write("\n");
            }
        }
    }
}

