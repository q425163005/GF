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
                openList.Remove(nowHexagon);                        //����ǰ�ڵ��openList���Ƴ�  
                closeList.Add(nowHexagon);                          //����ǰ�ڵ���ӵ��ر��б���  
                Hexagon[] neighbors = nowHexagon.getNeighborList(); //��ȡ��ǰ�����ε�����������  
                //print("��ǰ���ڽڵ���----" + neighbors.size());  
                foreach (Hexagon neighbor in neighbors)
                {
                    if (neighbor == null) continue;

                    if (neighbor == targetHexagon)
                    {
                        //�ҵ�Ŀ��ڵ�  
                        //System.out.println("�ҵ�Ŀ���");  
                        finded = true;
                        neighbor.setFatherHexagon(nowHexagon);
                    }

                    if (closeList.Contains(neighbor) || !neighbor.canPass())
                    {
                        //�ڹر��б���  
                        //print("�޷�ͨ���������ڹر��б�");  
                        continue;
                    }

                    if (openList.Contains(neighbor))
                    {
                        //�ýڵ��Ѿ��ڿ����б���  
                        //print("���ڿ����б��ж��Ƿ���ĸ��ڵ�");  
                        float assueGValue =
                            neighbor.computeGValue(nowHexagon) + nowHexagon.getgValue(); //�������ӵ�ǰ�ڵ���룬�ýڵ��g��ֵ  
                        if (assueGValue < neighbor.getgValue())
                        {
                            //�����g��ֵС����ԭ����g��ֵ  
                            openList.Remove(neighbor);       //��������ýڵ���openList��λ��  
                            neighbor.setgValue(assueGValue); //��������g��ֵ  
                            openList.Add(neighbor);          //��������openList��  
                        }
                    }
                    else
                    {
                        //û���ڿ����б���  
                        //print("���ڿ����б����");  
                        neighbor.sethValue(neighbor.computeHValue(targetHexagon)); //���������h��ֵ  
                        neighbor.setgValue(neighbor.computeGValue(nowHexagon) +
                                           nowHexagon.getgValue()); //����ýڵ��g��ֵ������ǰ�ڵ��g��ֵ���ϵ�ǰ�ڵ��g��ֵ��  
                        openList.Add(neighbor);                     //��ӵ������б���  
                        neighbor.setFatherHexagon(nowHexagon);      //����ǰ�ڵ�����Ϊ�ýڵ�ĸ��ڵ�  
                    }
                }

                if (openList.Count <= 0)
                {
                    //print("�޷������Ŀ��");  
                    break;
                }
                else
                {
                    nowHexagon = openList[0]; //�õ�f��ֵ��͵Ľڵ�����Ϊ��ǰ�ڵ�  
                }
            }

            openList.Clear();
            closeList.Clear();

            List<Hexagon> route = new List<Hexagon>();
            if (finded)
            {
                //�ҵ���·�ߴ���·�߼���  
                Hexagon hex = targetHexagon;
                while (hex != thisHexagon)
                {
                    route.Add(hex); //���ڵ���ӵ�·���б���  

                    Hexagon fatherHex = hex.getFatherHexagon(); //��Ŀ��ڵ㿪ʼ��Ѱ���ڵ������Ҫ��·��  
                    hex = fatherHex;
                }

                route.Add(hex);
            }

            route.Reverse();
            return route;
            //      resetMap();  
        }

        //ͨ�����赲Ѱ·ȷ�����������εľ���
        public static int GetRouteDis(Hexagon thisHexagon, Hexagon targetHexagon)
        {
            Hexagon nowHexagon = thisHexagon;

            openList.Add(nowHexagon);
            bool finded = false;
            while (!finded)
            {
                openList.Remove(nowHexagon);                        //����ǰ�ڵ��openList���Ƴ�  
                closeList.Add(nowHexagon);                          //����ǰ�ڵ���ӵ��ر��б���  
                Hexagon[] neighbors = nowHexagon.getNeighborList(); //��ȡ��ǰ�����ε�����������  
                //print("��ǰ���ڽڵ���----" + neighbors.size());  
                foreach (Hexagon neighbor in neighbors)
                {
                    if (neighbor == null) continue;

                    if (neighbor == targetHexagon)
                    {
                        //�ҵ�Ŀ��ڵ�  
                        //System.out.println("�ҵ�Ŀ���");  
                        finded = true;
                        neighbor.setFatherHexagon(nowHexagon);
                    }

                    if (closeList.Contains(neighbor))
                    {
                        //�ڹر��б���  
                        //System.out.println("�޷�ͨ���������ڹر��б�");  
                        continue;
                    }

                    if (openList.Contains(neighbor))
                    {
                        //�ýڵ��Ѿ��ڿ����б���  
                        //System.out.println("���ڿ����б��ж��Ƿ���ĸ��ڵ�");  
                        float assueGValue =
                            neighbor.computeGValue(nowHexagon) + nowHexagon.getgValue(); //�������ӵ�ǰ�ڵ���룬�ýڵ��g��ֵ  
                        if (assueGValue < neighbor.getgValue())
                        {
                            //�����g��ֵС����ԭ����g��ֵ  
                            openList.Remove(neighbor);       //��������ýڵ���openList��λ��  
                            neighbor.setgValue(assueGValue); //��������g��ֵ  
                            openList.Add(neighbor);          //��������openList��  
                        }
                    }
                    else
                    {
                        //û���ڿ����б���  
                        //System.out.println("���ڿ����б����");  
                        neighbor.sethValue(neighbor.computeHValue(targetHexagon)); //���������h��ֵ  
                        neighbor.setgValue(neighbor.computeGValue(nowHexagon) +
                                           nowHexagon.getgValue()); //����ýڵ��g��ֵ������ǰ�ڵ��g��ֵ���ϵ�ǰ�ڵ��g��ֵ��  
                        openList.Add(neighbor);                     //��ӵ������б���  
                        neighbor.setFatherHexagon(nowHexagon);      //����ǰ�ڵ�����Ϊ�ýڵ�ĸ��ڵ�  
                    }
                }

                if (openList.Count <= 0)
                {
                    //System.out.println("�޷������Ŀ��");  
                    break;
                }
                else
                {
                    nowHexagon = openList[0]; //�õ�f��ֵ��͵Ľڵ�����Ϊ��ǰ�ڵ�  
                }
            }

            openList.Clear();
            closeList.Clear();

            List<Hexagon> route = new List<Hexagon>();
            if (finded)
            {
                //�ҵ���·�ߴ���·�߼���  
                Hexagon hex = targetHexagon;
                while (hex != thisHexagon)
                {
                    route.Add(hex); //���ڵ���ӵ�·���б���  

                    Hexagon fatherHex = hex.getFatherHexagon(); //��Ŀ��ڵ㿪ʼ��Ѱ���ڵ������Ҫ��·��  
                    hex = fatherHex;
                }

                route.Add(hex);
            }

            return route.Count - 1;
        }
        */
    }
}