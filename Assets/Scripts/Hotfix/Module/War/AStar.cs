using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class Hexagon
    {
        Hexagon          father    = null;
        float            gValue    = 999f;
        float            hValue    = 999f;
        
        

        public void setFatherHexagon(Hexagon f)
        {
            father = f;
        }

        public Hexagon getFatherHexagon()
        {
            return father;
        }
        
        public float computeGValue(Hexagon hex)
        {
            return 1f;
        }

        public void setgValue(float v)
        {
            gValue = v;
        }

        public float getgValue()
        {
            return gValue;
        }

        public void sethValue(float v)
        {
            hValue = v;
        }

        public float gethValue()
        {
            return hValue;
        }

        public float computeHValue(Hexagon hex)
        {
            return 0;
            //return Vector3.Distance(transform.position, hex.transform.position);
        }
    }

    public class AStar
    {
        Dictionary<string, Hexagon> name2Hex = new Dictionary<string, Hexagon>();

        static List<Hexagon> openList  = new List<Hexagon>();
        static List<Hexagon> closeList = new List<Hexagon>();

        void Start()
        {
        }

        public Hexagon GetHexByName(string i)
        {
            Hexagon v = new Hexagon();
            name2Hex.TryGetValue(i, out v);
            return v;
        }

        public Dictionary<string, Hexagon> GetAllHex()
        {
            return name2Hex;
        }

        /*
        public static List<Hexagon> searchRoute(Hexagon thisHexagon, Hexagon targetHexagon)
        {
            Hexagon nowHexagon = thisHexagon;

            openList.Add(nowHexagon);
            bool finded = false;
            while (!finded)
            {
                openList.Remove(nowHexagon);                        //将当前节点从openList中移除  
                closeList.Add(nowHexagon);                          //将当前节点添加到关闭列表中  
                Hexagon[] neighbors = nowHexagon.getNeighborList(); //获取当前六边形的相邻六边形  
                //print("当前相邻节点数----" + neighbors.size());  
                foreach (Hexagon neighbor in neighbors)
                {
                    if (neighbor == null) continue;

                    if (neighbor == targetHexagon)
                    {
                        //找到目标节点  
                        //System.out.println("找到目标点");  
                        finded = true;
                        neighbor.setFatherHexagon(nowHexagon);
                    }

                    if (closeList.Contains(neighbor) || !neighbor.canPass())
                    {
                        //在关闭列表里  
                        //print("无法通过或者已在关闭列表");  
                        continue;
                    }

                    if (openList.Contains(neighbor))
                    {
                        //该节点已经在开启列表里  
                        //print("已在开启列表，判断是否更改父节点");  
                        float assueGValue =
                            neighbor.computeGValue(nowHexagon) + nowHexagon.getgValue(); //计算假设从当前节点进入，该节点的g估值  
                        if (assueGValue < neighbor.getgValue())
                        {
                            //假设的g估值小于于原来的g估值  
                            openList.Remove(neighbor);       //重新排序该节点在openList的位置  
                            neighbor.setgValue(assueGValue); //从新设置g估值  
                            openList.Add(neighbor);          //从新排序openList。  
                        }
                    }
                    else
                    {
                        //没有在开启列表里  
                        //print("不在开启列表，添加");  
                        neighbor.sethValue(neighbor.computeHValue(targetHexagon)); //计算好他的h估值  
                        neighbor.setgValue(neighbor.computeGValue(nowHexagon) +
                                           nowHexagon.getgValue()); //计算该节点的g估值（到当前节点的g估值加上当前节点的g估值）  
                        openList.Add(neighbor);                     //添加到开启列表里  
                        neighbor.setFatherHexagon(nowHexagon);      //将当前节点设置为该节点的父节点  
                    }
                }

                if (openList.Count <= 0)
                {
                    //print("无法到达该目标");  
                    break;
                }
                else
                {
                    nowHexagon = openList[0]; //得到f估值最低的节点设置为当前节点  
                }
            }

            openList.Clear();
            closeList.Clear();

            List<Hexagon> route = new List<Hexagon>();
            if (finded)
            {
                //找到后将路线存入路线集合  
                Hexagon hex = targetHexagon;
                while (hex != thisHexagon)
                {
                    route.Add(hex); //将节点添加到路径列表里  

                    Hexagon fatherHex = hex.getFatherHexagon(); //从目标节点开始搜寻父节点就是所要的路线  
                    hex = fatherHex;
                }

                route.Add(hex);
            }

            route.Reverse();
            return route;
            //      resetMap();  
        }

        //通过无阻挡寻路确定两个六边形的距离
        public static int GetRouteDis(Hexagon thisHexagon, Hexagon targetHexagon)
        {
            Hexagon nowHexagon = thisHexagon;

            openList.Add(nowHexagon);
            bool finded = false;
            while (!finded)
            {
                openList.Remove(nowHexagon);                        //将当前节点从openList中移除  
                closeList.Add(nowHexagon);                          //将当前节点添加到关闭列表中  
                Hexagon[] neighbors = nowHexagon.getNeighborList(); //获取当前六边形的相邻六边形  
                //print("当前相邻节点数----" + neighbors.size());  
                foreach (Hexagon neighbor in neighbors)
                {
                    if (neighbor == null) continue;

                    if (neighbor == targetHexagon)
                    {
                        //找到目标节点  
                        //System.out.println("找到目标点");  
                        finded = true;
                        neighbor.setFatherHexagon(nowHexagon);
                    }

                    if (closeList.Contains(neighbor))
                    {
                        //在关闭列表里  
                        //System.out.println("无法通过或者已在关闭列表");  
                        continue;
                    }

                    if (openList.Contains(neighbor))
                    {
                        //该节点已经在开启列表里  
                        //System.out.println("已在开启列表，判断是否更改父节点");  
                        float assueGValue =
                            neighbor.computeGValue(nowHexagon) + nowHexagon.getgValue(); //计算假设从当前节点进入，该节点的g估值  
                        if (assueGValue < neighbor.getgValue())
                        {
                            //假设的g估值小于于原来的g估值  
                            openList.Remove(neighbor);       //重新排序该节点在openList的位置  
                            neighbor.setgValue(assueGValue); //从新设置g估值  
                            openList.Add(neighbor);          //从新排序openList。  
                        }
                    }
                    else
                    {
                        //没有在开启列表里  
                        //System.out.println("不在开启列表，添加");  
                        neighbor.sethValue(neighbor.computeHValue(targetHexagon)); //计算好他的h估值  
                        neighbor.setgValue(neighbor.computeGValue(nowHexagon) +
                                           nowHexagon.getgValue()); //计算该节点的g估值（到当前节点的g估值加上当前节点的g估值）  
                        openList.Add(neighbor);                     //添加到开启列表里  
                        neighbor.setFatherHexagon(nowHexagon);      //将当前节点设置为该节点的父节点  
                    }
                }

                if (openList.Count <= 0)
                {
                    //System.out.println("无法到达该目标");  
                    break;
                }
                else
                {
                    nowHexagon = openList[0]; //得到f估值最低的节点设置为当前节点  
                }
            }

            openList.Clear();
            closeList.Clear();

            List<Hexagon> route = new List<Hexagon>();
            if (finded)
            {
                //找到后将路线存入路线集合  
                Hexagon hex = targetHexagon;
                while (hex != thisHexagon)
                {
                    route.Add(hex); //将节点添加到路径列表里  

                    Hexagon fatherHex = hex.getFatherHexagon(); //从目标节点开始搜寻父节点就是所要的路线  
                    hex = fatherHex;
                }

                route.Add(hex);
            }

            return route.Count - 1;
        }
        */
    }
}